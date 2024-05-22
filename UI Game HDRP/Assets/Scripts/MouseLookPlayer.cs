using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseLookPlayer : MonoBehaviour
{
    [SerializeField] Transform player;
    [SerializeField] float vClampMin = -90f;
    [SerializeField] float vClampMax = 90f;

	float mouseSensativity = 100f;
    float mouseADSMulti = 1f;
	float rotX = 0f;
    Camera gunCamera;
    // Start is called before the first frame update 
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        gunCamera = GetComponent<Camera>();
        //Loading values from player prefs
        mouseSensativity = PlayerPrefs.GetFloat("AimSens");
        mouseADSMulti = PlayerPrefs.GetFloat("ADSMulti");
        EventSystem.Subscribe(EventSystem.eEventType.updateSettings, UpdateSettings);
		EventSystem.Subscribe(EventSystem.eEventType.adsActivated, adsActivated);
		EventSystem.Subscribe(EventSystem.eEventType.adsDeactivated, adsDeactivated);
	}

    // Update is called once per frame 
    void Update()
    {
        //Rotating the camera 
        float mouseX = Input.GetAxis("Mouse X") * mouseSensativity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensativity * Time.deltaTime;

        rotX -= mouseY;
        rotX = Mathf.Clamp(rotX, vClampMin, vClampMax);

        transform.localRotation = Quaternion.Euler(rotX, 0, 0);
        player.Rotate(Vector3.up * mouseX);
    }

    void adsActivated()
    {
        gunCamera.fieldOfView = 45;
        mouseSensativity *= mouseADSMulti;
    }

    void adsDeactivated()
    {
        gunCamera.fieldOfView = 60;
        mouseSensativity /= mouseADSMulti;
    }

    void UpdateSettings()
    {
		mouseSensativity = PlayerPrefs.GetFloat("AimSens");
		mouseADSMulti = PlayerPrefs.GetFloat("ADSMulti");
	}
}