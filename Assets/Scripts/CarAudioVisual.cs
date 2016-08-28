using UnityEngine;
using UnityEngine.UI;

namespace aggrathon.ld36
{
	public class CarAudioVisual : MonoBehaviour
	{
		[SerializeField] Transform smoke;
		[SerializeField] MeshRenderer coloredRenderer;
		[SerializeField] Transform steeringwheel;
		[SerializeField] AudioSource engineNoise;
		[SerializeField] float minRpm = 50f;
		[SerializeField] float soundInterval = 0.1f;
		[SerializeField] float soundPitchChange = 0.3f;

		CarController car;
		float audioDelay;

		void Awake()
		{
			car = GetComponent<CarController>();
			for (int i = 1; i < smoke.childCount; i++)
			{
				smoke.GetChild(i).gameObject.SetActive(false);
			}
		}

		public void SetColor(Color c)
		{
			coloredRenderer.materials[1].color = c;
		}

		public void SetHealth(float oldHealth, float newHealth)
		{
			int old = (int)((1f - oldHealth * 0.01f) * (float)smoke.childCount);
			int ne = (int)((1f - newHealth * 0.01f) * (float)smoke.childCount);
			if (ne != old)
			{
				if (old < smoke.childCount - 1)
					smoke.GetChild(old).gameObject.SetActive(false);
				smoke.GetChild(ne >= smoke.childCount ? smoke.childCount - 1 : ne).gameObject.SetActive(true);
			}
		}

		public void Destroy()
		{
			foreach (MeshRenderer mr in GetComponentsInChildren<MeshRenderer>())
				foreach (Material m in mr.materials)
					m.color = 0.45f * m.color + Color.black;
			enabled = false;
		}

		void Update()
		{
			steeringwheel.localRotation = Quaternion.Euler(0, 0, car.steering * 90f);
			float rpm = car.RPM;
			if (rpm < minRpm)
				rpm = minRpm;
			if(audioDelay > soundInterval/(rpm * Mathf.Pow(0.95f, (int)(rpm*0.01f))))
			{
				engineNoise.pitch = Random.Range(1f-soundPitchChange, 1f+soundPitchChange);
				engineNoise.PlayOneShot(engineNoise.clip);
				audioDelay = 0f;
			}
			else
			{
				audioDelay += Time.deltaTime;
			}

		}

	}
}
