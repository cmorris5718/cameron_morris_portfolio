using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI ammoText;
    [SerializeField] Gun gun;
    [SerializeField] TextMeshProUGUI mainObjective;
    [SerializeField] TextMeshProUGUI secondaryObjective;
    [SerializeField] GameObject pauseMenu;
    [SerializeField] Slider droneSlider;
    [SerializeField] Slider aimSlider;
    [SerializeField] Slider ADSSlider;
    [SerializeField] Slider MusicSlider;
    [SerializeField] Slider SFXVol;
    [SerializeField] GameObject successScreen;
    [SerializeField] GameObject failureScreen;
    const string SLASH_SYMBOL = "/";
    const string SECONDARY_OBJ_TEXT = "Secondary Objective: Defeat Enemy Personel ";
    int enemiesKilled = 0;
    int enemiesForMission = 50;

    bool mainObjComplete = false;
    bool secObjComplete = false;

	void Start()
    {
        EventSystem.Subscribe(EventSystem.eEventType.shotBullet, UpdateBulletText);
        EventSystem.Subscribe(EventSystem.eEventType.reload, UpdateMaxAmmoText);
        EventSystem.Subscribe(EventSystem.eEventType.enemyKilled, UpdateSecondaryObjectiveText);
        EventSystem.Subscribe(EventSystem.eEventType.enemySpawnerKilled, UpdateMainObjectiveText);
        EventSystem.Subscribe(EventSystem.eEventType.playerDied, ShowFailureScreen);
    }

	private void Update()
	{
        if(Input.GetKeyDown(KeyCode.P))
        {
            if(!pauseMenu.activeSelf) 
            {
				ShowPauseMenu();
			}
            else
            {
                HidePauseMenu();
            }
        }
        if (mainObjComplete && GameObject.FindGameObjectWithTag("Enemy") == null)
        {
            ShowSuccessScreen();
            HideExtraMagText();
        }
	}

	void UpdateBulletText()
    {
        Debug.Log("Bullet Shot");
        ammoText.text = gun.AmmoCount.ToString() + SLASH_SYMBOL + gun.SpareAmmoCount.ToString();
    }

    void UpdateMaxAmmoText()
    {
        Debug.Log("Reloaded");
        ammoText.text = gun.AmmoCount.ToString() + SLASH_SYMBOL + gun.SpareAmmoCount.ToString();
    }

    void UpdateMainObjectiveText()
    {
        mainObjective.text = mainObjective.text + " COMPLETED!!";
        mainObjComplete = true;
        if (secObjComplete) ShowSuccessScreen();        
    }

    void UpdateSecondaryObjectiveText()
    {
        enemiesKilled++;
        secondaryObjective.text = SECONDARY_OBJ_TEXT + enemiesKilled.ToString() + SLASH_SYMBOL + enemiesForMission.ToString();
        if (enemiesKilled >= 50)
        {
            secObjComplete = true;
			if (mainObjComplete) ShowSuccessScreen();
		}    
    }


    void ShowPauseMenu()
    {
		//Pausing Everything
		EventSystem.Publish(EventSystem.eEventType.pauseGame);
        updateSliderValues();
		pauseMenu.SetActive(true);
		Time.timeScale = 0;
        Cursor.lockState = CursorLockMode.Confined;        
	}

    public void HidePauseMenu()
    {
        //Unpausing Everything
        EventSystem.Publish(EventSystem.eEventType.updatePlayerPrefs);
        EventSystem.Publish(EventSystem.eEventType.updateSettings);
		EventSystem.Publish(EventSystem.eEventType.unpauseGame);
		pauseMenu.SetActive(false);
        Time.timeScale = 1;
        Cursor.lockState = CursorLockMode.Locked;        
    }

    void updateSliderValues()
    {
        droneSlider.value = PlayerPrefs.GetFloat("DroneSens");
        aimSlider.value = PlayerPrefs.GetFloat("AimSens");
        ADSSlider.value = PlayerPrefs.GetFloat("ADSMulti");
        MusicSlider.value = PlayerPrefs.GetFloat("MusicVol");
        SFXVol.value = PlayerPrefs.GetFloat("SFXVol");
    }

    void ShowSuccessScreen()
    {
        successScreen.SetActive(true);
		Time.timeScale = 0;
        Cursor.lockState = CursorLockMode.Confined;
	}

    void ShowFailureScreen()
    {
        failureScreen.SetActive(true);
		Time.timeScale = 0;
        Cursor.lockState = CursorLockMode.Confined;
	}

    void HideExtraMagText()
    {
        successScreen.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text =
            "You completed the main objective good work!";
    }

    public void goToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
        Time.timeScale = 1;
    }
}
