using UnityEngine;
using UnityEngine.Events;

namespace aggrathon.ld36
{
	public class AnimationEvents : MonoBehaviour
	{
		public UnityEvent[] events;

		public void CallEvent(int num)
		{
			if(num < events.Length && num >= 0)
			{
				events[num].Invoke();
			}
			else
			{
				Debug.LogError("AnimationEvent: event index out of range ("+num+")");
			}
		}
	}
}
