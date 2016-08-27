using System;
using UnityEngine;
using UnityEngine.UI;

namespace aggrathon.ld36
{
	public class CameraController : MonoBehaviour
	{
		[Header("Chase Camera")]
		Rigidbody target;
		public float followMoveSpeed = 5f;
		public float followRotateSpeed = 30f;

		[Header("UI")]
		public Text playerName;
		public Text speedText;

		public void Setup(PlayerController player)
		{
			target = player.car.GetComponent<Rigidbody>();
			playerName.text = player.name;
			switch (player.controller)
			{
				case PlayerController.InputType.Keyboard:
					GetComponentInChildren<Camera>().rect = new Rect(0, 0.5f, 1, 0.5f);
					break;
				case PlayerController.InputType.Controller:
					GetComponentInChildren<Camera>().rect = new Rect(0, 0, 1, 0.5f);
					GetComponentInChildren<AudioListener>().enabled = false;
					break;
				default:
					playerName.gameObject.SetActive(false);
					break;
			}
		}

		void Update()
		{
			speedText.text = string.Format("Speed: {0: 0}", target.velocity.magnitude * 3.6f);
		}

		void FixedUpdate()
		{
			transform.position = Vector3.Lerp(transform.position, target.transform.position, Time.deltaTime * followMoveSpeed);

			transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0,target.transform.rotation.eulerAngles.y,0), Time.deltaTime * followRotateSpeed);

			//Quaternion q = Quaternion.LookRotation(new Vector3(target.velocity.x, target.velocity.y), new Vector3(0, 1, 0));
			//transform.rotation = Quaternion.Slerp(q, Quaternion.Euler(0, target.transform.rotation.eulerAngles.y, 0), 0.5f);
		}
	}
}
