using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using System;

/// <summary>
/// Singleton for playing audio
/// </summary>
public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance; //For Singleton purposes

    [SerializeField]Sound[] musicSounds, sfxSounds; //Array holding Sound objects (custom class for audio) separated by music and sound effects
    [SerializeField] AudioSource musicSource, gunSfxSource; //Holds references to audioSources for different sound effects

    /// <summary>
    /// Singleton creation
    /// </summary>
	private void Awake()
	{
        //When no instance exists create one and set it do don't destroy on load
		if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        //Otherwise destroy duplicate
        else
        {
            Destroy(gameObject);
        }
	}

    /// <summary>
    /// Subscribing to events and loading player prefs values
    /// </summary>
	private void Start()
	{
        //Loading player Prefs
        musicSource.volume = PlayerPrefs.GetFloat("MusicVol");
        gunSfxSource.volume = PlayerPrefs.GetFloat("SFXVol");
        PlayMusic("HailToTheKing");
        EventSystem.Subscribe(EventSystem.eEventType.updateSettings, UpdateSettings);
        EventSystem.Subscribe(EventSystem.eEventType.pauseGame, pauseSounds);
		EventSystem.Subscribe(EventSystem.eEventType.unpauseGame, unpauseSounds);
	}

    /// <summary>
    /// Plays a music track based on the input song name
    /// </summary>
    /// <param name="name"> Name of the music to play </param>
	public void PlayMusic(string name)
    {
        //Searching for the sound
        Sound s = Array.Find(musicSounds, x => x.name == name);

        //Logging error if no sound found
        if(s == null)
        {
            Debug.Log("Error: Music Not Found");
        }
        //Setting the clip appropriately and playing 
        else
        {
            musicSource.clip = s.clip;
            musicSource.Play();
        }
    }

    /// <summary>
    /// Plays a sound effect (used for gun sounds)
    /// </summary>
    /// <param name="name">  </param>
    public void PlaySFX(string name)
    {
        Sound s = Array.Find(sfxSounds, x => x.name == name);
        if(s == null)
        {
            Debug.Log("Error: Sound Effect Not Found");
        }
        else
        {
            gunSfxSource.clip = s.clip;
            if (!gunSfxSource.isPlaying)
            {
                gunSfxSource.Play();
            }
        }
    }

    public void StopGunSFX()
    {
        gunSfxSource.Stop();
    }

    void UpdateSettings()
    {
		musicSource.volume = PlayerPrefs.GetFloat("MusicVol");
		gunSfxSource.volume = PlayerPrefs.GetFloat("SFXVol");
	}

    void pauseSounds()
    {
        musicSource.Pause();
        gunSfxSource.Pause();
    }

    void unpauseSounds()
    {
		musicSource.UnPause();
		gunSfxSource.UnPause();
	}
}
