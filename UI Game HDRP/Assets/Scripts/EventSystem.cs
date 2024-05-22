using System.Collections.Generic;
using UnityEngine.Events;

public class EventSystem
{
	public enum eEventType { shotBullet, reload, collectPrimaryAmmo, switchInput, playerDamaged, enemyKilled, enemySpawnerKilled, adsActivated, adsDeactivated,updatePlayerPrefs,updateSettings,pauseGame,unpauseGame,playerDied}
	private static readonly IDictionary<eEventType, UnityEvent> Events = new Dictionary<eEventType, UnityEvent>();

	public static void Subscribe(eEventType eventType, UnityAction listener)
	{
		UnityEvent thisEvent;

		if (Events.TryGetValue(eventType, out thisEvent))
		{
			thisEvent.AddListener(listener);
		}
		else
		{
			thisEvent = new UnityEvent();
			thisEvent.AddListener(listener);
			Events.Add(eventType, thisEvent);
		}
	}

	public static void Unsubscribe(eEventType eventType, UnityAction listener)
	{
		UnityEvent thisEvent;

		if (Events.TryGetValue(eventType, out thisEvent))
		{
			thisEvent.RemoveListener(listener);
		}
	}

	public static void Publish(eEventType eventType)
	{
		UnityEvent thisEvent;

		if (Events.TryGetValue(eventType, out thisEvent))
		{
			thisEvent.Invoke();
		}
	}
}
