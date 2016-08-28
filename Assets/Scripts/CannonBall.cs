using UnityEngine;
using System.Collections.Generic;

namespace aggrathon.ld36 {

	[RequireComponent(typeof(ParticleSystem))]
	[RequireComponent(typeof(Rigidbody))]
	[RequireComponent(typeof(AudioSource))]
	public class CannonBall : MonoBehaviour
	{
		public float explosionRange = 10f;
		public float explosionDamage = 20f;

		ParticleSystem ps;
		Rigidbody rb;
		AudioSource au;
		bool active = false;

		void Awake()
		{
			ps = GetComponent<ParticleSystem>();
			rb = GetComponent<Rigidbody>();
			au = GetComponent<AudioSource>();
		}

		public void Launch(Transform point, float force)
		{
			rb.isKinematic = true;
			rb.angularVelocity = new Vector3(0, 0, 0);
			rb.velocity = new Vector3(0, 0, 0);
			rb.position = point.position;
			rb.isKinematic = false;
			rb.AddForce(point.forward * force, ForceMode.VelocityChange);
			active = true;
			Particles();
		}

		void OnCollisionEnter(Collision collision)
		{
			if (active)
			{
				active = false;
				ps.startLifetime = 2f;
				Particles();
				ps.startLifetime = 1f;
			}
		}

		void Explode()
		{
			Collider[] cols = Physics.OverlapSphere(transform.position, explosionRange);
			List<CarController> list = new List<CarController>();
			for (int i = 0; i < cols.Length; i++)
			{
				if(cols[i].attachedRigidbody != null)
				{
					CarController car = cols[i].attachedRigidbody.GetComponent<CarController>();
					if(car != null)
					{
						if(!list.Contains(car))
						{
							list.Add(car);
							car.Health -= Mathf.Lerp(0, explosionDamage, (transform.position - car.transform.position).magnitude / explosionRange);
						}
					}
				}
			}
		}

		void Particles()
		{
			au.Play();
			if (!ps.isStopped)
			{
				ps.Stop(true);
			}
			ps.Play(true);
		}
	}
}
