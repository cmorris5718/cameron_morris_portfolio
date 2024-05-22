using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class Crosshair : MonoBehaviour
{
    [SerializeField] GameObject crosshair;
    void Start()
    {
        EventSystem.Subscribe(EventSystem.eEventType.adsActivated, toggleCrosshair);
        EventSystem.Subscribe(EventSystem.eEventType.adsDeactivated, toggleCrosshair);
    } 

    void toggleCrosshair()
    {
        crosshair.SetActive(!crosshair.activeSelf);
    }
}
