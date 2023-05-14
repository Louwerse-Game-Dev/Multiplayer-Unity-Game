using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float movementSpeed;

    private new Rigidbody2D rigidbody2D;
    private Vector2 moveVelocity;

    // Start is called before the first frame update
    void Start()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        /*  Check if input for horizontal / vertical movement is pressed.
        *   To change this see 'Edit'->'Project Settings'->'Input'.
        *   Input.GetAxis("...") wil return -1, 0 or 1 depending on what keys are pressed.
        *   Use GetAxisRaw for a more 'snappy' style of movement, instead of the smooth movement of GetAxis.
        */
        Vector2 moveInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        // Now multiply this by movementSpeed. normalized makes sure we do not move faster when moving diagonally.
        moveVelocity = moveInput.normalized * movementSpeed;
    }

    // FixedUpdate is called once per physics step
    // Can run once, zero, or several times per frame, depending on how many physics frames per second are set in the time settings, 
    // and how fast/slow the framerate is.
    void FixedUpdate()
    {
        rigidbody2D.MovePosition(rigidbody2D.position + moveVelocity * Time.fixedDeltaTime);
    }
}
