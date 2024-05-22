using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Drone : MonoBehaviour
{
    [SerializeField] CharacterController characterController;
    [SerializeField] float speed = 12f;
    [SerializeField] float gravity = 0f;
    [SerializeField] Camera droneCamera;

    bool isActive = false;
    float switchTime = 0f;

    // Start is called before the first frame update
    void Start()
    {
        EventSystem.Subscribe(EventSystem.eEventType.switchInput, switchInput);
    }

    // Update is called once per frame
    void Update()
    {
        if(isActive)
        {
            //Movement Controls
            float x = Input.GetAxis("DroneHorizontal");
            float z = Input.GetAxis("DroneVertical");
            float y = Input.GetAxis("DroneFront");

            Vector3 move = transform.right * x + transform.up * z + transform.forward * y;
            Debug.Log("Move Vector: " + move);
            characterController.Move(move * speed * Time.deltaTime);

            //Switching to player
            if(switchTime == Time.time)
            {
                return;
            }
            else if(Input.GetKeyDown(KeyCode.F))
            {
                Debug.Log("Switching Character");
                EventSystem.Publish(EventSystem.eEventType.switchInput);
                droneCamera.gameObject.SetActive(false);
            }
        }
    }

    void switchInput()
    {
        Debug.Log("Drone Switch Input");
		isActive = !isActive;
        switchTime = Time.time;
        if(isActive)
        {
            droneCamera.gameObject.SetActive(true);
        }
        else
        {
            droneCamera.gameObject.SetActive(false);
        }
	}
}
