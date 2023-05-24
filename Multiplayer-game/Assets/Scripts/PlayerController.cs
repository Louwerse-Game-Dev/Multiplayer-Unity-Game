using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public float movementSpeed;

    PlayerInput playerInput;
    Vector2 moveInput;
    Vector2 rotateInput;

    // Awake is called before the Start function is called
    void Awake()
    {
        playerInput = GetComponent<PlayerInput>();

        // Configure events to listen to. 
        // Use lambda to get result and update moveInput.
        playerInput.actions["Move"].performed += context => moveInput = context.ReadValue<Vector2>();
        playerInput.actions["Move"].canceled += context => moveInput = Vector2.zero;

        // Use lambda to get result and update rotateInput.
        playerInput.actions["RotateTest"].performed += context => rotateInput = context.ReadValue<Vector2>();
        playerInput.actions["RotateTest"].canceled += context => rotateInput = Vector2.zero;
    }

    // Update is called once per frame
    void Update()
    {
        // Player movement * movementSpeed
        Vector2 move = new Vector2(moveInput.x, moveInput.y) * movementSpeed * Time.deltaTime;
        transform.Translate(move, Space.World);

        // Player rotation (Controller input testing, might remove or change this later)
        Vector3 rotate = new Vector3(0, 0, -rotateInput.x) * 50f * Time.deltaTime;
        transform.Rotate(rotate, Space.World);
    }

    [ContextMenu("Toggle PlayerInput")]
    public void togglePlayerInput()
    {
        if (playerInput == null) return;
        
        playerInput.enabled = playerInput.enabled == false;
    }

    void OnEnable()
    {
        if (playerInput != null && playerInput.enabled == false) {
            togglePlayerInput();
        }
    }

    void OnDisable()
    {
        if (playerInput != null && playerInput.enabled == true) {
            togglePlayerInput();
        }
    }

    public void OnDeviceLost(PlayerInput playerInput)
    {
        // Todo: some sort of forced pause menu
        Debug.Log("Device Lost");
    }

    public void OnDeviceRegained(PlayerInput playerInput)
    {
        // Todo: close forced pause menu
        Debug.Log("Device Regained");
    }
}
