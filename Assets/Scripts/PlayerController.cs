using System;
using UnityEngine;

namespace aggrathon.ld36
{
	public class PlayerController : MonoBehaviour
	{
		[SerializeField] private aggrathon.ld36.CarController car;

		void Update()
		{
			car.steering = Input.GetAxis("Horizontal");
			car.accelerator = Input.GetAxis("Vertical");
		}
	}
}
