using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float maxMovementSpeed = 10f;

    [Header("Attack Settings")]
    [SerializeField] private Transform attackPoint;
    [SerializeField] private Bullet bulletPrefab;
    [SerializeField] private float meleeDamage = 4f;
    [SerializeField] private LayerMask enemyLayer;
    [SerializeField] private ParticleSystem meleeAttackVFX;

    [Header("Other Settings")]
    [SerializeField] private float pickupRadius = 1f;
    [SerializeField] private LayerMask pickupLayer;

    [Header("UI Settings")]
    [SerializeField] private GameObject inventoryUI;

    private Rigidbody2D rb;
    private SpriteRenderer sprite;

    private PlayerInput playerInput;
    private InputAction rangedAttackAction;
    private InputAction meleeAttackAction;
    private InputAction pickupAction;
    private InputAction openInventoryAction;

    private HealthSystem healthSystem;

    private Vector2 input;
    private Vector3 mousePos;

    private float currentSpeed;
    private float potionSpeedMaxTime = 3;
    private float potionSpeedTimer = 0;

    // Distance from the center of the player to the attack point
    private float attackPointRadius = 1.5f;

    private List<PickupItem> inventoryList = new List<PickupItem>();
    private bool isInventoryOpen = false;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();

        playerInput = GetComponent<PlayerInput>();
        
        healthSystem = GetComponent<HealthSystem>();

        currentSpeed = maxMovementSpeed;

        // Find the action map and the attack actions
        var actionMap = playerInput.actions.FindActionMap("player");
        rangedAttackAction = actionMap.FindAction("ranged_attack");
        meleeAttackAction = actionMap.FindAction("melee_attack");
        pickupAction = actionMap.FindAction("pickup");
        openInventoryAction = actionMap.FindAction("inventory");

        // Subscribe to the performed events
        rangedAttackAction.performed += OnRangedAttack;
        meleeAttackAction.performed += OnMeleeAttack;
        pickupAction.performed += OnPickup;
        openInventoryAction.performed += OnOpenInventory;
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

        // Speed increased using potions
        if (currentSpeed > maxMovementSpeed)
        {
            if (potionSpeedTimer > potionSpeedMaxTime)
            {
                currentSpeed = maxMovementSpeed;
            }

            potionSpeedTimer += Time.deltaTime;
        }
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
        Vector2 velocity = input * currentSpeed;
        rb.velocity = velocity;

        // TODO: Fix rotations wit UI.
        RotateTowardsMouse();
    }

    /* UNUSED FOR NOW */
    private void RotateTowardsMouse()
    {
        mousePos.z = 0;
        Vector3 objectPos = Camera.main.WorldToScreenPoint(transform.position);
        mousePos.x = mousePos.x - objectPos.x;
        mousePos.y = mousePos.y - objectPos.y;

        float angle = Mathf.Atan2(mousePos.y, mousePos.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
    }

    private void OnRangedAttack(InputAction.CallbackContext context)
    {
        AudioManager.Instance.Play("Player Ranged Attack");

        Bullet bullet = Instantiate(bulletPrefab, attackPoint.position, Quaternion.identity);

        Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        mouseWorldPosition.z = 0;

        Vector2 shootDirection = (mouseWorldPosition - attackPoint.position).normalized;
        bullet.GetComponent<Rigidbody2D>().velocity = shootDirection * bullet.GetBulletSpeed();
    }

    private void OnMeleeAttack(InputAction.CallbackContext context)
    {
        AudioManager.Instance.Play("Player Melee Attack");
        meleeAttackVFX.Play();

        RaycastHit2D hit = Physics2D.CircleCast(attackPoint.position, 1f, Vector3.zero, 10f, enemyLayer);
        if (hit)
        {
            hit.transform.GetComponent<EnemyController>().Hit(meleeDamage);
        }
    }

    private void OnPickup(InputAction.CallbackContext context)
    {
        AudioManager.Instance.Play("Player Pickup");

        RaycastHit2D hit = Physics2D.CircleCast(transform.position, 1f, Vector3.zero, 10f, pickupLayer);
        if (hit)
        {
            PickupItem item = hit.transform.GetComponent<PickupItem>();
            inventoryList.Add(item);
            item.gameObject.SetActive(false);

            if (isInventoryOpen)
                inventoryUI.GetComponent<InventoryUI>().UpdateVisual();
        }
    }
    
    private void OnOpenInventory(InputAction.CallbackContext context)
    {
        if (!isInventoryOpen)
        {
            isInventoryOpen = true;
            inventoryUI.SetActive(true);
            inventoryUI.GetComponent<InventoryUI>().UpdateVisual();
        }
        else
        {
            inventoryUI.SetActive(false);
            isInventoryOpen = false;
        }
    }

    public void Hit(float damage)
    {
        AudioManager.Instance.Play("Player Hit");

        healthSystem.DecreaseCurrentHealth(damage);
    }

    public List<PickupItem> GetInventoryItems() => inventoryList;

    public void UseItem(PickupType pickupType)
    {
        PickupItem item = inventoryList.Where(x => x.GetPickupType() == pickupType).First();
        if (item == null) return;

        switch (pickupType)
        {
            case PickupType.HealthPotion:
                if (healthSystem.GetCurrentHealth() <= healthSystem.GetMaxHealth())
                {
                    healthSystem.IncreaseCurrentHealth(item.GetAmount());
                    inventoryList.Remove(item);
                    Destroy(item);
                    if (isInventoryOpen)
                        inventoryUI.GetComponent<InventoryUI>().UpdateVisual();
                }
                break;
            case PickupType.SpeedPotion:
                if (currentSpeed <= maxMovementSpeed)
                {
                    currentSpeed += item.GetAmount();
                    inventoryList.Remove(item);
                    Destroy(item);
                    if (isInventoryOpen)
                        inventoryUI.GetComponent<InventoryUI>().UpdateVisual();
                }
                break;
        }
    }
}
