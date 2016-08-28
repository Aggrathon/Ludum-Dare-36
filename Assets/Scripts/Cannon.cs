using UnityEngine;
using System.Collections;

namespace aggrathon.ld36
{

	public class Cannon : MonoBehaviour
	{
		public float cooldown = 10f;
		public GameObject canonBallPrefab;
		public Transform tip;
		public float launchForce = 100f;

		CarController car;
		CannonBall canonBall;
		
		void Start()
		{
			car = GetComponentInParent<CarController>();
			canonBall = (Instantiate(canonBallPrefab, new Vector3(transform.position.x, -200f, transform.position.z), new Quaternion(0, 0, 0, 1)) as GameObject).GetComponent<CannonBall>();
			car.boostMeter = -10f;
		}

		void Update()
		{
			if(car.Boosting && car.boostMeter > 0f)
			{
				car.boostMeter -= 10f;
				car.Boosting = false;
				canonBall.Launch(tip, launchForce);
			}
			else if(car.boostMeter > 1f)
			{
				car.boostMeter = 1f;
			}
		}
	}
}
