using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;


namespace aggrathon.ld36
{
	public class LoadoutSelection : MonoBehaviour
	{
		public Transform playerHolder;

		void Update()
		{
			if(Input.GetButtonDown("Cancel"))
			{
				gameObject.SetActive(false);
			}
		}

		public void Play()
		{
			PlayerData.Players = new PlayerData[playerHolder.childCount - 1];
			for (int i = 0; i < PlayerData.Players.Length; i++)
			{
				PlayerData.Players[i] = new PlayerData(
					playerHolder.GetChild(i + 1).GetComponentInChildren<InputField>().text,
					playerHolder.GetChild(i + 1).GetComponentInChildren<Toggle>().isOn ? PlayerData.Controller.ai : PlayerData.Controller.player
					);
			}
			if(PlayerData.Players.Length > 1 && PlayerData.Players[1].controller == PlayerData.Controller.player)
			{
				PlayerData.Players[0].controller = PlayerData.Controller.player1;
				PlayerData.Players[1].controller = PlayerData.Controller.player2;
			}

			SceneManager.LoadScene(1);
		}

		public void SetHumans(int count)
		{
			count++;
			while (playerHolder.childCount < count)
				Instantiate(playerHolder.GetChild(1), playerHolder);
			for (int i = playerHolder.childCount-1; i >= count; i--)
			{
				Destroy(playerHolder.GetChild(i).gameObject);
			}

			for (int i = 1; i < count; i++)
			{
				playerHolder.GetChild(i).GetComponentInChildren<Toggle>().isOn = false;
				playerHolder.GetChild(i).GetComponentInChildren<InputField>().text = "Player " + i;
				foreach (Dropdown dd in playerHolder.GetChild(i).GetComponentsInChildren<Dropdown>())
					dd.value = 0;
			}
		}

		public void SetBots(int count)
		{
			int start = playerHolder.childCount;

			while (playerHolder.childCount < start+count)
				Instantiate(playerHolder.GetChild(1), playerHolder);

			for (int i = 0; i < count; i++)
			{
				playerHolder.GetChild(i+start).GetComponentInChildren<Toggle>().isOn = true;
				playerHolder.GetChild(i+start).GetComponentInChildren<InputField>().text = "Bot " + (i+1);
				foreach (Dropdown dd in playerHolder.GetChild(i+start).GetComponentsInChildren<Dropdown>())
					dd.value = 0;
			}
		}
	}
}
