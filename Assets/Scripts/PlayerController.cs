using System;
using UnityEngine;

namespace aggrathon.ld36
{
	public class PlayerController : MonoBehaviour
	{
		public enum InputType
		{
			Both,
			Keyboard,
			Controller
		}

		public CarController car;
		new public CameraController camera;
		public InputType controller;

		public void Setup(CarController car, CameraController camera, InputType input)
		{
			this.car = car;
			this.controller = input;
			this.camera = camera;
			camera.Setup(this);
		}

		public void Setup(CarController car, CameraController camera, PlayerData.Controller input)
		{
			switch (input)
			{
				case PlayerData.Controller.player1:
					Setup(car, camera, InputType.Keyboard);
					break;
				case PlayerData.Controller.player2:
					Setup(car, camera, InputType.Controller);
					break;
				default:
					Setup(car, camera, InputType.Both);
					break;
			}
		}

		void Update()
		{
			//Debug.Log(car.RPM);
			switch (controller)
			{
				case InputType.Keyboard:
					car.steering = Input.GetAxis("P1 Horizontal");
					car.accelerator = Input.GetAxis("P1 Vertical");
					car.Boosting = Input.GetButton("P1 Boost");
					car.Handbrake = Input.GetButton("P1 Handbrake");
					break;
				case InputType.Controller:
					car.steering = Input.GetAxis("P2 Horizontal");
					car.accelerator = Input.GetAxis("P2 Vertical");
					car.Boosting = Input.GetButton("P2 Boost");
					car.Handbrake = Input.GetButton("P2 Handbrake");
					break;
				case InputType.Both:
					car.steering = Input.GetAxis("P2 Horizontal") + Input.GetAxis("P1 Horizontal");
					car.accelerator = Input.GetAxis("P1 Vertical") + Input.GetAxis("P2 Vertical");
					car.Boosting = Input.GetButton("P1 Boost") || Input.GetButton("P2 Boost");
					car.Handbrake = Input.GetButton("P1 Handbrake") || Input.GetButton("P2 Handbrake");
					break;
			}
		}
	}
}
