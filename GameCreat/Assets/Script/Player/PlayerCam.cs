using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerCam : MonoBehaviour
{
    public float sensX;
    public float sensY;

    PlayerInputSystem inputSystem;

    public Transform orientation;

    public Vector2 camDirection;

    float xRotation;
    float yRotation;

    private void Awake()
    {
        inputSystem = new PlayerInputSystem();       
    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void OnEnable()
    {
        inputSystem.Enable();
    }

    private void OnDisable()
    {
        inputSystem.Disable();
    }

    private void Update()
    {
        camDirection = inputSystem.Player.Look.ReadValue<Vector2>();

        InputDevice inputDevice = InputSystem.devices.FirstOrDefault(d => d.enabled);

        if (inputDevice is Pointer)
        {
            sensX = 5f;
            sensY = 5f;
        }
        else if(inputDevice is Gamepad)
        {
            sensX = 100f;
            sensY = 100f;
        }

        // get mouse input
        float X = camDirection.x * Time.deltaTime * sensX;
        float Y = camDirection.y * Time.deltaTime * sensY;

        yRotation += X;

        xRotation -= Y;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        // rotate cam and orientation
        transform.rotation = Quaternion.Euler(xRotation, yRotation, 0);
        orientation.rotation = Quaternion.Euler(0, yRotation, 0);
    }
}