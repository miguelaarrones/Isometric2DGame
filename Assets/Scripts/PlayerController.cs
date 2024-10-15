using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Timeline.Actions;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;
using static UnityEngine.Rendering.HableCurve;

public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float maxMovementSpeed = 10f;
    [SerializeField] private float movementSpeed = 20f;

    [Header("Attack Settings")]
    [SerializeField] private Transform attackPoint;
    [SerializeField] private Bullet bulletPrefab;
    [SerializeField] private float meleeDamage = 4f;
    [SerializeField] private LayerMask enemyLayer;

    private Rigidbody2D rb;
    private SpriteRenderer sprite;

    private PlayerInput playerInput;
    private InputAction attackAction;
    private InputAction meleeAttackAction;

    private HealthSystem healthSystem;

    private Vector2 input;
    private Vector3 mousePos;

    // Distance from the center of the player to the attack point
    private float attackPointRadius = 1.5f;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();

        playerInput = GetComponent<PlayerInput>();
        
        healthSystem = GetComponent<HealthSystem>();
        
        // Find the action map and the attack actions
        var actionMap = playerInput.actions.FindActionMap("player");
        attackAction = actionMap.FindAction("attack");
        meleeAttackAction = actionMap.FindAction("melee_attack");

        // Subscribe to the performed events
        attackAction.performed += OnAttack;
        meleeAttackAction.performed += OnMeleeAttack;
    }

    private void Update()
    {
        input = playerInput.actions["move"].ReadValue<Vector2>();
        mousePos = playerInput.actions["mouse"].ReadValue<Vector2>();

        if (healthSystem.GetCurrentHealth() <= 0)
            Die();

        // AttackPoint rotation
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(mousePos);
        mouseWorldPos.z = 0;
        Vector3 direction = mouseWorldPos - transform.position;
        Vector3 positionOnCircle = transform.position + direction.normalized * attackPointRadius;
        attackPoint.position = positionOnCircle;

        ChangeColorOnInput();
    }

    private void Die()
    {
        // TODO: Something cool when player death
        Debug.Log("YOU DIED");
    }

    private void ChangeColorOnInput()
    {
        if (input.x > 0.0f) // Is moving right
            sprite.color = Color.cyan;
        if (input.x < 0.0f) // Is moving left
            sprite.color = Color.magenta;
        if (input.y > 0.0f) // Is moving forward
            sprite.color = Color.blue;
        if (input.y < 0.0f) // Is moving backwards
            sprite.color = Color.green;
    }

    private void FixedUpdate()
    {
        // TODO: Does this make sense?
        Vector2 velocity = input * movementSpeed;
        rb.velocity = velocity;

        if (rb.velocity.magnitude > maxMovementSpeed)
        {
            rb.velocity = rb.velocity.normalized * maxMovementSpeed;
        }

        // TODO: Fix rotations wit UI.
        // RotateTowardsMouse();
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

    private void OnAttack(InputAction.CallbackContext context)
    {
        Bullet bullet = Instantiate(bulletPrefab, attackPoint.position, Quaternion.identity);

        Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        mouseWorldPosition.z = 0;

        Vector2 shootDirection = (mouseWorldPosition - attackPoint.position).normalized;
        bullet.GetComponent<Rigidbody2D>().velocity = shootDirection * bullet.GetBulletSpeed();
    }

    private void OnMeleeAttack(InputAction.CallbackContext context)
    {
        RaycastHit2D hit = Physics2D.CircleCast(attackPoint.position, 1f, Vector3.zero, 10f, enemyLayer);
        if (hit)
        {
            hit.transform.GetComponent<EnemyController>().Hit(meleeDamage);
        }
    }

    public void Hit(float damage)
    {
        healthSystem.DecreaseCurrentHealth(damage);
    }
}
