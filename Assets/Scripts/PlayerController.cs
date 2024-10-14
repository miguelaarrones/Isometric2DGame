using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D rb;
    private PlayerInput playerInput;
    private Vector2 input;
    private float maxMovementSpeed = 10f;
    private float movementSpeed = 20f;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        playerInput = GetComponent<PlayerInput>();
    }

    private void Update()
    {
        input = playerInput.actions["move"].ReadValue<Vector2>();
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
