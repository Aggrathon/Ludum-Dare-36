using System.Collections;
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

		[Header("Text")]
		[SerializeField] TextMesh nameText;
		[SerializeField] TextMesh damageText;

		CarController car;
		float audioDelay;

		void Awake()
		{
			car = GetComponent<CarController>();
			for (int i = 1; i < smoke.childCount; i++)
			{
				smoke.GetChild(i).gameObject.SetActive(false);
			}
			nameText.text = transform.parent.name;
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
			float diff = oldHealth - newHealth;
			if(diff >1f)
			{
				damageText.text = string.Format("- {0:0}", diff);
				StopAllCoroutines();
				StartCoroutine(FadeDamage());
			}
		}

		IEnumerator FadeDamage()
		{
			damageText.gameObject.SetActive(true);
			float a = 0f;
			while(a < 1f)
			{
				Color c = damageText.color;
				c.a += Time.deltaTime * 2f;
				a = c.a;
				damageText.color = c;
				yield return null;
			}
			yield return new WaitForSeconds(1f);
			while (a > 0f)
			{
				Color c = damageText.color;
				c.a -= Time.deltaTime * 2f;
				a = c.a;
				damageText.color = c;
				yield return null;
			}
			damageText.gameObject.SetActive(false);
		}

		public void Destroy()
		{
			foreach (MeshRenderer mr in GetComponentsInChildren<MeshRenderer>())
				foreach (Material m in mr.materials)
					m.color = 0.4f * m.color + Color.black;
			enabled = false;
			nameText.gameObject.SetActive(false);
		}

		void Update()
		{
			steeringwheel.localRotation = Quaternion.Euler(0, 0, car.steering * 90f);
			float rpm = car.RPM;
			if (rpm < minRpm)
				rpm = minRpm;
			if(audioDelay > soundInterval/(rpm * Mathf.Pow(0.93f, (int)(rpm*0.01f))))
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
