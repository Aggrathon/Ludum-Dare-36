using UnityEngine;
using UnityEngine.UI;

namespace aggrathon.ld36
{
	public class MainMenu : MonoBehaviour
	{

		public void Quit()
		{
			Application.Quit();
#if UNITY_EDITOR
			UnityEditor.EditorApplication.isPlaying = false;
#endif
		}
	}
}
