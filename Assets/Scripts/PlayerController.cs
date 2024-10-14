using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float maxMovementSpeed = 10f;
    [SerializeField] private float movementSpeed = 20f;

    private Rigidbody2D rb;
    private PlayerInput playerInput;
    private Vector2 input;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        playerInput = GetComponent<PlayerInput>();
    }

    private void Update()
    {
        input = playerInput.actions["move"].ReadValue<Vector2>();

        // Removed for better development of the other features, as it was a Bonus point in Task 2
        // ChangeSizeOnInput();
    }

    private void ChangeSizeOnInput()
    {
        Vector3 newScale = new Vector3(1.0f, 1.0f, 1.0f);

        if (input.x > 0.0f) // Is moving right
            newScale.x = 1.4f;
        if (input.x < 0.0f) // Is moving left
            newScale.x = 0.6f;
        if(input.y > 0.0f) // Is moving forward
            newScale.y = 1.4f;
        if(input.y < 0.0f) // Is moving backwards
            newScale.y = 0.6f;
        

        transform.localScale = newScale;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector2 velocity = input * movementSpeed;
        rb.velocity = velocity;

        if (rb.velocity.magnitude > maxMovementSpeed)
        {
            rb.velocity = rb.velocity.normalized * maxMovementSpeed;
        }
    }
}
