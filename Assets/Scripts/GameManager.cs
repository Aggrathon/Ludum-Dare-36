using System;
using UnityEngine;

namespace aggrathon.ld36
{
	public class GameManager : MonoBehaviour
	{
		public GameObject carPrefab;
		public GameObject cameraPrefab;

		public Transform[] spawnLocations;

		void Start()
		{
			for (int i = 0; i < PlayerData.Players.Length; i++)
			{
				GameObject go = new GameObject(PlayerData.Players[i].name);
				CarController car = (Instantiate(carPrefab, spawnLocations[i].position, spawnLocations[i].rotation, go.transform) as GameObject).GetComponent<CarController>();
				if (PlayerData.Players[i].controller == PlayerData.Controller.ai)
				{

				}
				else
				{
					CameraController cam = (Instantiate(cameraPrefab, spawnLocations[i].position, spawnLocations[i].rotation, go.transform) as GameObject).GetComponent<CameraController>();
					go.AddComponent<PlayerController>().Setup(car, cam, PlayerData.Players[i].controller);
				}
			}
		}
	}
}
