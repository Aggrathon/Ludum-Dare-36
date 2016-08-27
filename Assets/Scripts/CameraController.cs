using System;
using UnityEngine;

namespace aggrathon.ld36
{
	public class CameraController : MonoBehaviour
	{
		public Rigidbody target;
		public float followMoveSpeed = 5f;
		public float followRotateSpeed = 30f;

		void FixedUpdate()
		{
			transform.position = Vector3.Lerp(transform.position, target.transform.position, Time.deltaTime * followMoveSpeed);

			transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0,target.transform.rotation.eulerAngles.y,0), Time.deltaTime * followRotateSpeed);

			//Quaternion q = Quaternion.LookRotation(new Vector3(target.velocity.x, target.velocity.y), new Vector3(0, 1, 0));
			//transform.rotation = Quaternion.Slerp(q, Quaternion.Euler(0, target.transform.rotation.eulerAngles.y, 0), 0.5f);
		}
	}
}
