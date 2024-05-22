using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] AudioSource menuMusic;
    /// <summary>
    /// Method to load level based on input string
    /// </summary>
    /// <param name="levelName"> Scene name input </param>
    public void LoadLevel(string levelName)
    {
        SceneManager.LoadScene(levelName);
    }

    public void ActivatePanel(GameObject panel)
    {
        panel.SetActive(true);
    }

    public void DeactivatePanel(GameObject panel)
    {
        panel.SetActive(false);
    }

    public void UpdateMenuMusicVolume()
    {
        menuMusic.volume = PlayerPrefs.GetFloat("MusicVol");
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
