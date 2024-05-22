using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimScript : MonoBehaviour
{
	[SerializeField] GameObject crosshair;
	private static bool isAiming;

	public static bool IsAiming
	{
		get { return isAiming; }
	}

	private void Update()
	{
		if(Input.GetMouseButtonDown(1))
		{
			EventSystem.Publish(EventSystem.eEventType.adsActivated);
			isAiming = true;
		}
		if(Input.GetMouseButtonUp(1))
		{
			EventSystem.Publish(EventSystem.eEventType.adsDeactivated);
			isAiming = false;
		}
	}
}
