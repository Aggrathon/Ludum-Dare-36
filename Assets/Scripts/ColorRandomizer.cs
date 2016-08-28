using UnityEngine;
using UnityEngine.UI;

namespace aggrathon.ld36
{
	[RequireComponent(typeof(Image))]
	public class ColorRandomizer : MonoBehaviour
	{
		[System.NonSerialized] public Color color;
		Image img;

		void Start()
		{
			img = GetComponent<Image>();
			Randomize();
		}

		public void Randomize()
		{
			color = Random.ColorHSV(0f,1f,0.4f,1f,0.4f,1f);
			color.a = 1f;
			img.color = color;
		}
	}
}
