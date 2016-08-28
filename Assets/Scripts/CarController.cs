using System;
using UnityEngine;

namespace aggrathon.ld36
{
	[DisallowMultipleComponent]
	[RequireComponent(typeof(CarAudioVisual))]
	[RequireComponent(typeof(Rigidbody))]
	public class CarController : MonoBehaviour
	{
		public enum WheelDrive
		{
			FrontWheelDrive,
			RearWheelDrive,
			FourWheelDrive
		}

		[Tooltip("Requires exactly 4 wheels with the first 2 being the front wheels")]
		[SerializeField]
		private WheelCollider[] wheelColliders = new WheelCollider[4];
		[Tooltip("These visuals will be made to match the previous WheelColliders")]
		[SerializeField]
		private GameObject[] wheelVisuals = new GameObject[4];

		[Header("Driving")]
		[SerializeField]
		private float maximumSteerAngle = 20f;
		[SerializeField]
		public float maximumTorque = 2500f;
		[SerializeField]
		public float reverseTorque = 1500f;
		[SerializeField]
		private float brakeTorque = 2000f;
		[SerializeField]
		private WheelDrive driveType = WheelDrive.RearWheelDrive;
		[SerializeField]
		[Range(1, 2)]
		public float boostMultiplier = 1.2f;
		public float boostDrain = 8f;

		[Header("Health")]
		[SerializeField] private float health = 100f;
		[SerializeField] private float baseArmor = 10f;
		[SerializeField] public float frontArmor = 25f;
		[SerializeField] public float sideArmor = 10f;
		[SerializeField] private float damageMultiplier = 10f;

		CarAudioVisual visuals;
		public GameObject[] upgradeObjects;

		[NonSerialized]
		public float accelerator = 0f;
		[NonSerialized]
		public float steering = 0f;
		[NonSerialized]
		public bool handbrake = false;
		private bool boosting = false;
		[NonSerialized] public float boostMeter = 40f;
		public bool Boosting {
			get { return boosting; }
			set
			{
				if (value && boostMeter > 1f)
					boosting = true;
				else
					boosting = false;
			}
		}
		[NonSerialized]
		new public Rigidbody rigidbody;
		[NonSerialized]
		public Action<CarController> onDestroyed;

		private float currentTorque;
		Rigidbody lastCollider;
		float lastCollision;

		public float Health
		{
			get { return health; }
			set
			{
				visuals.SetHealth(health, value);
				health = value;
				if (value <= 0f)
				{
					onDestroyed(this);
				}
			}
		}

		public float RPM { get { return (wheelColliders[0].rpm + wheelColliders[1].rpm + wheelColliders[2].rpm + wheelColliders[3].rpm) * 0.25f; } }

		void Awake()
		{
			visuals = GetComponent<CarAudioVisual>();
			rigidbody = GetComponent<Rigidbody>();
			onDestroyed += (car) =>
			{
				wheelColliders[0].motorTorque =
					wheelColliders[1].motorTorque =
					wheelColliders[2].motorTorque =
					wheelColliders[3].motorTorque = 0f;
				wheelColliders[0].brakeTorque =
					wheelColliders[1].brakeTorque =
					wheelColliders[2].brakeTorque =
					wheelColliders[3].brakeTorque = brakeTorque * 0.5f;
				car.enabled = false;
				visuals.Destroy();
			};
		}

		void FixedUpdate()
		{
			if(rigidbody.velocity.sqrMagnitude < 0.05f && !wheelColliders[0].isGrounded && wheelColliders[1].isGrounded && wheelColliders[2].isGrounded && wheelColliders[3].isGrounded && Vector3.Angle(transform.up, new Vector3(0,1,0)) > 80f)
			{
				Health = 0f;
				return;
			}
			if(boosting)
			{
				boostMeter -= boostDrain * Time.deltaTime;
				if (boostMeter <= 0f)
					boosting = false;
			}
			else
			{
				if(boostMeter < 100f)
					boostMeter += Time.deltaTime;
			}

			for (int i = 0; i < 4; i++)
			{
				Quaternion rot;
				Vector3 pos;
				wheelColliders[i].GetWorldPose(out pos, out rot);
				wheelVisuals[i].transform.position = pos;
				wheelVisuals[i].transform.rotation = rot;
			}

			steering = Mathf.Clamp(steering, -1f, 1f);
			wheelColliders[0].steerAngle = maximumSteerAngle * steering;
			wheelColliders[1].steerAngle = maximumSteerAngle * steering;

			if (accelerator > 0f)
			{
				if (accelerator > 1f)
					accelerator = 1f;

				if (rigidbody.velocity.sqrMagnitude > 6 && Vector3.Angle(transform.forward, rigidbody.velocity) > 90f)
				{
					wheelColliders[0].brakeTorque =
						wheelColliders[1].brakeTorque =
						wheelColliders[2].brakeTorque =
						wheelColliders[3].brakeTorque = brakeTorque * accelerator;
					currentTorque = 0f;
				}
				else
				{
					currentTorque = Boosting ? maximumTorque * boostMultiplier : maximumTorque;
					wheelColliders[0].brakeTorque =
						wheelColliders[1].brakeTorque =
						wheelColliders[2].brakeTorque =
						wheelColliders[3].brakeTorque = 0f;
				}
			}
			else
			{
				if (accelerator < -1f)
					accelerator = -1f;

				if (rigidbody.velocity.sqrMagnitude > 6 && Vector3.Angle(transform.forward, rigidbody.velocity) < 90f)
				{
					wheelColliders[0].brakeTorque =
						wheelColliders[1].brakeTorque =
						wheelColliders[2].brakeTorque =
						wheelColliders[3].brakeTorque = -brakeTorque * accelerator;
					currentTorque = 0f;
				}
				else
				{
					currentTorque = reverseTorque;
					wheelColliders[0].brakeTorque =
						wheelColliders[1].brakeTorque =
						wheelColliders[2].brakeTorque =
						wheelColliders[3].brakeTorque = 0f;
				}
			}

			switch (driveType)
			{
				case WheelDrive.FrontWheelDrive:
					wheelColliders[0].motorTorque =
						wheelColliders[1].motorTorque = currentTorque * accelerator * 0.5f;
					break;
				case WheelDrive.RearWheelDrive:
					wheelColliders[2].motorTorque =
						wheelColliders[3].motorTorque = currentTorque * accelerator * 0.5f;
					break;
				case WheelDrive.FourWheelDrive:
					wheelColliders[0].motorTorque =
						wheelColliders[1].motorTorque =
						wheelColliders[2].motorTorque =
						wheelColliders[3].motorTorque = currentTorque * accelerator * 0.25f;
					break;
			}

			if (handbrake)
			{
				wheelColliders[0].brakeTorque =
					wheelColliders[1].brakeTorque =
					wheelColliders[2].brakeTorque =
					wheelColliders[3].brakeTorque = brakeTorque;
			}
		}

		void OnCollisionEnter(Collision collision)
		{
			float damage = 0f;
			Rigidbody othrig = collision.rigidbody;
			if(othrig == null)
			{
				//for (int i = 0; i < collision.contacts.Length; i++)
				//{
				//	damage += Vector3.Project(rigidbody.velocity, collision.contacts[i].normal).magnitude;
				//}
				//damage = damage / (float)collision.contacts.Length * damageMultiplier * 0.5f;
			}
			else if(othrig != rigidbody)
			{
				if (Time.time - lastCollision < 0.5f && othrig == lastCollider)
				{
					//Debug.Log("Rehit");
					return;
				}

				if (Vector3.Angle(othrig.velocity, transform.position - othrig.position) > 80f)
					return;
				damage = Vector3.Project(othrig.velocity, transform.position - othrig.position).magnitude * damageMultiplier * (othrig.mass / rigidbody.mass);

				if(Vector3.Angle(transform.forward, othrig.position - transform.position) < 45f)
				{
					damage -= frontArmor;
				}
				else if(Vector3.Angle(transform.right, othrig.position-transform.position) < 45f || Vector3.Angle(transform.right, othrig.position - transform.position) > 135f)
				{
					damage -= sideArmor;
				}
				else
				{
					damage -= baseArmor;
				}

				if (damage > 0f)
				{
					lastCollision = Time.time;
					lastCollider = othrig;
					//Debug.Log(damage);
					Health -= damage;
				}
				return;
			}
			return;
		}
	}
}
