using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PlayerSettings : MonoBehaviour
{
    private float DroneFlightSensativity = 50f;
    private float AimSensativity = 100f;
    private float ADSSensativityMultiplier = 1f;
    private float MusicVolume = 0.5f;
    private float SFXVolume = 1f;

	#region Properties
	public float DroneFlightSens
    {
        get {return this.DroneFlightSensativity;        }
        set { this.DroneFlightSensativity = value;}
    }

    public float AimSens
    {
        get { return this.AimSensativity; }
        set { this.AimSensativity = value;}
    }

    public float ADSMult
    {
        get { return this.ADSSensativityMultiplier; }
        set { this.ADSSensativityMultiplier = value;}
    }

    public float MusicVol
    {
        get { return this.MusicVolume; }
        set { this.MusicVolume = value;}
    }

    public float SFXVol
    {
        get { return this.SFXVolume; }
        set { this.SFXVolume = value;}
    }
	#endregion

	private void Start()
	{
        EventSystem.Subscribe(EventSystem.eEventType.updatePlayerPrefs, SettingsUpdated);
	}

	public void SettingsUpdated()
    {
        PlayerPrefs.SetFloat("DroneSens", DroneFlightSensativity);
        PlayerPrefs.SetFloat("AimSens", AimSensativity);
        PlayerPrefs.SetFloat("ADSMulti", ADSSensativityMultiplier);
        PlayerPrefs.SetFloat("MusicVol", MusicVolume);
        PlayerPrefs.SetFloat("SFXVol", SFXVolume);
        Debug.Log("DroneSens: " + DroneFlightSensativity);
        Debug.Log("AimSens: " + AimSensativity);
        Debug.Log("ADSMulti: " + ADSSensativityMultiplier);
        Debug.Log("MusicVol: " + MusicVolume);
        Debug.Log("SFXVol: " + SFXVolume);
        Debug.Log("Updated Settings");
    }
}
