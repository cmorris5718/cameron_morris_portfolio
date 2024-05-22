using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseLook : MonoBehaviour
{
    [SerializeField] Transform player;
    [SerializeField] float mouseSensativity = 1000f;
    [SerializeField] float vClampMin = -90f;
    [SerializeField] float vClampMax = 90f;

    float rotX = 0f;
    float rotY = 0f;
    // Start is called before the first frame update 
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        //Loading player prefs
        mouseSensativity = PlayerPrefs.GetFloat("DroneSens");
        EventSystem.Subscribe(EventSystem.eEventType.updateSettings, UpdateSettings);
    }

    // Update is called once per frame 
    void Update()
    {
        //Rotating the camera 
        float mouseX = Input.GetAxis("Mouse X") * mouseSensativity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensativity * Time.deltaTime;

        rotX -= mouseY;
        rotX = Mathf.Clamp(rotX, vClampMin, vClampMax);
        rotY += mouseX;

        player.localRotation = Quaternion.Euler(rotX, rotY, 0);
        player.Rotate(Vector3.up * mouseX);
    }

    void UpdateSettings()
    {
		mouseSensativity = PlayerPrefs.GetFloat("DroneSens");
	}
}
