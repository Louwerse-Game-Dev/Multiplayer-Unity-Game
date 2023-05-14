using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public float movementSpeed;

    PlayerControls controls;
    Vector2 moveInput;
    Vector2 rotateInput;

    // Awake is called before the Start function is called
    void Awake()
    {
        controls = new PlayerControls();

        // Configure events to listen to. 
        // Use lambda to get result and update moveInput.
        controls.Gameplay.Move.performed += context => moveInput = context.ReadValue<Vector2>();
        controls.Gameplay.Move.canceled += context => moveInput = Vector2.zero;

        // Use lambda to get result and update rotateInput.
        controls.Gameplay.RotateTest.performed += context => rotateInput = context.ReadValue<Vector2>();
        controls.Gameplay.RotateTest.canceled += context => rotateInput = Vector2.zero;
    }

    // Start is called before the first frame update
    void Start()
    {
        // ...
    }

    // Update is called once per frame
    void Update()
    {
        // Player movement * movementSpeed
        Vector2 move = new Vector2(moveInput.x, moveInput.y) * movementSpeed * Time.deltaTime;
        transform.Translate(move, Space.World);

        // Player rotation (Controller input testing, might remove or change this later)
        Vector3 rotate = new Vector3(0, 0, -rotateInput.x) * 300f * Time.deltaTime;
        transform.Rotate(rotate, Space.World);
    }

    // FixedUpdate is called once per physics step
    // Can run once, zero, or several times per frame, depending on how many physics frames per second are set in the time settings, 
    // and how fast/slow the framerate is.
    void FixedUpdate()
    {
        // ...
    }

    void OnEnable()
    {
        if (controls != null) {
            controls.Gameplay.Enable();
        }
    }

    void OnDisable()
    {
        if (controls != null) {
            controls.Gameplay.Disable();
        }
    }
}
