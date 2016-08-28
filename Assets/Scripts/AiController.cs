using System;
using System.Text;
using UnityEngine;

namespace aggrathon.ld36
{
	public class AiController : MonoBehaviour
	{
		public float rotationCost = 50f / 180f;
		[Range(50, 250)]
		public float trackingRadius = 100f;
		[Range(2, 16)]
		public float closeRadius = 4f;
		[Range(0, 20)]
		public float randomTarget;
		public float reactionSpeed = 4f;
		[Range(0, 90)]
		public float accelerationAngle = 30f;
		public float stuckSpeed = 0.5f;
		public float stuckTimer = 2f;
		public float targetPrediction = 0.2f;

		CarController car;
		CarController target;
		float stuckTime = 0f;

		public void Setup(CarController car)
		{
			this.car = car;
			car.onDestroyed += (c) =>
			{
				this.enabled = false;
				this.target = null;
			};
		}

		void Update()
		{
			if (target == null)
			{
				FindTarget();
			}
			else
			{
				//Track Target
				float len = Vector3.Distance(car.transform.position, target.transform.position);
				if (len < closeRadius)
				{
					target = null;
					FindTarget();
					return;
				}

				len += Vector3.Angle(car.transform.forward, target.transform.position - car.transform.position) * rotationCost;
				if (len > trackingRadius)
				{
					target = null;
					FindTarget();
					return;
				}

				Vector3 pos = target.transform.position + target.rigidbody.velocity * (len * targetPrediction);
				if (Vector2.Angle(car.transform.forward, pos - car.transform.position) < accelerationAngle)
				{
					car.accelerator += Time.deltaTime * reactionSpeed;
				}
				else if(stuckTime != 0)
				{
					car.accelerator = -1f;
				}
				else
				{
					car.accelerator = 0f;
				}

				pos = car.transform.InverseTransformPoint(pos);
				if (Mathf.Atan2(pos.x, pos.z) < 0f)
				{
					car.steering -= Time.deltaTime;
				}
				else
				{
					car.steering += Time.deltaTime;
				}


				//Try to unstuck
				if (stuckTime > stuckTimer)
				{
					if (stuckTime > stuckTimer * 3f)
					{
						stuckTime = 0f;
						return;
					}
					stuckTime += Time.deltaTime;
					car.accelerator = -1f;
					if (Mathf.Atan2(pos.x, pos.z) < 0f)
					{
						car.steering += 2*Time.deltaTime;
					}
					else
					{
						car.steering -= 2*Time.deltaTime;
					}
					return;
				}
				//Check if stuck
				if (car.rigidbody.velocity.sqrMagnitude < stuckSpeed)
				{
					stuckTime += Time.deltaTime;
				}
				else
				{
					stuckTime = 0f;
				}
			}
		}

		void FindTarget()
		{
			CarController closest = null;
			float dist = float.PositiveInfinity;
			for (int i = 0; i < GameManager.instance.cars.Length; i++)
			{
				CarController temp = GameManager.instance.cars[i];
				if (temp == car || temp == target)
					continue;
				float nd = Vector3.Distance(car.transform.position, temp.transform.position) + Vector3.Angle(car.transform.forward, temp.transform.position - car.transform.position) * rotationCost + UnityEngine.Random.Range(0, randomTarget);
				if (nd < dist)
				{
					dist = nd;
					closest = temp;
				}
			}
			target = closest;
		}

		void OnDrawGizmos()
		{
			if(target != null)
			{
				Gizmos.color = Color.red;
				float len = Vector3.Distance(car.transform.position, target.transform.position) + Vector3.Angle(car.transform.forward, target.transform.position - car.transform.position) * rotationCost + UnityEngine.Random.Range(0, randomTarget);
				Vector3 pos = target.transform.position + target.rigidbody.velocity * (len * targetPrediction);
				Gizmos.DrawLine(car.transform.position, pos);
				Gizmos.DrawSphere(pos, 0.5f);
				Gizmos.DrawLine(pos, target.transform.position);
			}
		}
	}
}
