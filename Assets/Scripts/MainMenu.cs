using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace aggrathon.ld36
{
	public class MainMenu : MonoBehaviour
	{

		public void Tutorial()
		{
			PlayerData.Players = new PlayerData[]
			{
				new PlayerData("Player", PlayerData.Controller.player, Color.green, Upgrades.Upgrade.Front_Plow, Upgrades.Upgrade.Improved_Boosters),
				new PlayerData("Opponent", PlayerData.Controller.dummy, Color.red, Upgrades.Upgrade.Better_Engine)
			};
			SceneManager.LoadScene(1);
		}

		public void Quit()
		{
			Application.Quit();
#if UNITY_EDITOR
			UnityEditor.EditorApplication.isPlaying = false;
#endif
		}
	}
}
