using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GoToMenu : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Invoke(nameof(LoadMainMenu), 5f);
    }

	private void Update()
	{
		if(Input.anyKeyDown)
        {
            CancelInvoke(nameof(LoadMainMenu));
            LoadMainMenu();
        }
	}

	void LoadMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
