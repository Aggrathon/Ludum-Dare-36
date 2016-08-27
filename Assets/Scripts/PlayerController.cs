using System;
using UnityEngine;

namespace aggrathon.ld36
{
	public class PlayerController : MonoBehaviour
	{
		enum InputType
		{
			Both,
			Keyboard,
			Controller
		}

		[SerializeField] private aggrathon.ld36.CarController car;
		[SerializeField] private InputType controller;

		void Update()
		{
			switch (controller)
			{
				case InputType.Keyboard:
					car.steering = Input.GetAxis("P1 Horizontal");
					car.accelerator = Input.GetAxis("P1 Vertical");
					car.boosting = Input.GetButton("P1 Boost");
					break;
				case InputType.Controller:
					car.steering = Input.GetAxis("P2 Horizontal");
					car.accelerator = Input.GetAxis("P2 Vertical");
					car.boosting = Input.GetButton("P2 Boost");
					break;
				case InputType.Both:
					car.steering = Input.GetAxis("Horizontal");
					car.accelerator = Input.GetAxis("Vertical");
					car.boosting = Input.GetButton("P1 Boost") || Input.GetButton("P2 Boost");
					break;
			}
		}
	}
}
