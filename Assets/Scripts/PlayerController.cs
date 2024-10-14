using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Timeline.Actions;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float maxMovementSpeed = 10f;
    [SerializeField] private float movementSpeed = 20f;
    [SerializeField] private Transform shootPoint;
    [SerializeField] private Bullet bulletPrefab;

    private Rigidbody2D rb;
    private PlayerInput playerInput;
    private Vector2 input;
    private Vector3 mousePos;

    private InputAction attackAction;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        playerInput = GetComponent<PlayerInput>();

        // Find the action map and the click action
        var actionMap = playerInput.actions.FindActionMap("gameplay");
        attackAction = actionMap.FindAction("attack");

        // Subscribe to the performed event
        attackAction.performed += OnAttack;
    }

    private void Update()
    {
        input = playerInput.actions["move"].ReadValue<Vector2>();
        mousePos = playerInput.actions["mouse"].ReadValue<Vector2>();

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

        RotateTowardsMouse();
    }

    private void RotateTowardsMouse()
    {
        mousePos.z = 0;
        Vector3 objectPos = Camera.main.WorldToScreenPoint(transform.position);
        mousePos.x = mousePos.x - objectPos.x;
        mousePos.y = mousePos.y - objectPos.y;

        float angle = Mathf.Atan2(mousePos.y, mousePos.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
    }

    public void OnAttack(InputAction.CallbackContext context)
    {
        Bullet bullet = Instantiate(bulletPrefab, shootPoint.position, Quaternion.identity);

        Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        mouseWorldPosition.z = 0;

        Vector2 shootDirection = (mouseWorldPosition - shootPoint.position).normalized;
        bullet.GetComponent<Rigidbody2D>().velocity = shootDirection * bullet.GetBulletSpeed();
    }
}
