using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameramover : MonoBehaviour
{
    // Movement speed
    public float moveSpeed = 5f;
    // Rotation speed
    public float rotationSpeed = 100f;

    // Update is called once per frame
    void Update()
    {
        // Handle rotation input (E for right, Q for left)
        HandleRotationInput();

        // Get input for horizontal (A/D or Left Arrow/Right Arrow) and vertical (W/S or Up Arrow/Down Arrow)
        float moveX = Input.GetAxis("Horizontal");  // Handles A/D or Left/Right Arrow
        float moveY = Input.GetAxis("Vertical");    // Handles W/S or Up/Down Arrow

        // Create a movement vector based on current rotation
        Vector3 movement = transform.forward * moveY + transform.right * moveX;

        // Normalize movement vector to prevent faster movement diagonally
        if (movement.magnitude > 1f)
        {
            movement.Normalize();
        }

        // Move the player based on the input
        transform.Translate(movement * moveSpeed * Time.deltaTime, Space.World);
    }

    void HandleRotationInput()
    {
        // Rotate the player based on the input (E for right, Q for left)
        if (Input.GetKey(KeyCode.E))
        {
            transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
        }
        else if (Input.GetKey(KeyCode.Q))
        {
            transform.Rotate(Vector3.up, -rotationSpeed * Time.deltaTime);
        }
    }
}
