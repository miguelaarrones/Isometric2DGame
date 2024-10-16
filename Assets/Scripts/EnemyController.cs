using System;
using System.Collections.Generic;
using UnityEngine;

enum State
{
    Idle,
    Patrol,
    Chase,
    Attack
}
public class EnemyController : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float movementSpeed = 5f;
    [SerializeField] private float maxPatrolSpeed = 2f;
    [SerializeField] private bool followPath = false;
    [SerializeField] private List<Transform> path;

    [Header("Detection Settings")]
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private float detectionRadius = 3f;

    [Header("Attack Settings")]
    [SerializeField] private float attackRadius = 1f;
    [SerializeField] private float hitDamage = 2f;
    [SerializeField] private float hitSpeed = 2f;

    [Header("Super-speed Settings")]
    [SerializeField] private float superSpeedActivation = 5f;
    [SerializeField] private float superSpeedAbilityAmount = 20f;
    [SerializeField] private float superSpeedAbilityDuration = 2f;

    [Header("Other Settings")]
    [SerializeField] private ParticleSystem deathVFX;


    private State state;

    private Rigidbody2D rb;
    private Animator animator;

    private PlayerController player;
    private HealthSystem healthSystem;
    
    private int currentPathPoint = 0;

    private float hitTimer;

    private float superSpeedActivationTimer;
    private float superSpeedAbilityTimer;
    private bool isSuperSpeedActivated = false;

    private void Start()
    {
        state = State.Idle;

        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        healthSystem = GetComponent<HealthSystem>();
    }

    private void Update()
    {
        switch (state)
        {
            case State.Idle:
                animator.SetBool("Idle", true);
                rb.velocity = Vector3.zero;
                ResetSuperSpeed();

                if (followPath)
                {
                    animator.SetBool("Idle", false);
                    state = State.Patrol;
                }
                else
                {
                    if (CheckPlayerInDetectionRadius())
                    {
                        animator.SetBool("Idle", false);
                        state = State.Chase;
                    }
                }
                break;
            case State.Patrol:
                animator.SetBool("Patrol", true);
                ResetSuperSpeed();

                FollowPath();
                if (CheckPlayerInDetectionRadius())
                {
                    animator.SetBool("Patrol", false);
                    state = State.Chase;
                }
                break;
            case State.Chase:
                animator.SetBool("Chase", true);

                ChasePlayer();

                if (CheckPlayerInAttackRadius())
                {
                    animator.SetBool("Chase", false);
                    state = State.Attack;
                }

                if (!CheckPlayerInDetectionRadius())
                {
                    animator.SetBool("Chase", false);
                    state = State.Idle;
                }
                break;
            case State.Attack:
                animator.SetBool("Attack", true);
                ResetSuperSpeed();

                Attack();
                if (!CheckPlayerInAttackRadius())
                {
                    animator.SetBool("Attack", false);
                    state = State.Chase;
                }
                break;
            default:
                Debug.LogError("ENEMY_CONTROLLER::UPDATE Invalid STATE");
                break;
        }

        if (healthSystem.GetCurrentHealth() <= 0)
            Die();
    }

    private void FollowPath()
    {
        if (path.Count < 0)
        {
            state = State.Idle;
            return;
        }

        Vector2 dir = (path[currentPathPoint].position - transform.position).normalized;
        Move(dir, maxPatrolSpeed);

        // TODO: Fix rotations and UI
        //float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        //transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));

        if (Vector2.Distance(path[currentPathPoint].position, transform.position) < 0.1)
        {
            currentPathPoint++;
            currentPathPoint = currentPathPoint % path.Count;
        }
    }

    private bool CheckPlayerInDetectionRadius()
    {
        RaycastHit2D hit = Physics2D.CircleCast(transform.position, detectionRadius, Vector3.zero, 10f, playerLayer);
        if (hit)
        {
            player = hit.transform.GetComponent<PlayerController>();
            return true;
        }
        else
        {
            player = null;
            return false;
        }
    }

    private bool CheckPlayerInAttackRadius()
    {
        return Vector2.Distance(player.transform.position, transform.position) <= attackRadius;
    }

    private void ChasePlayer()
    {
        Vector2 dir = (player.transform.position - transform.position).normalized;
        Move(dir, movementSpeed);

        if (isSuperSpeedActivated)
        {
            superSpeedAbilityTimer += Time.deltaTime;
            if (superSpeedAbilityTimer > superSpeedAbilityDuration)
            {
                ResetSuperSpeed();
            }
        }
        else
        {
            // Check if it can activate the ability, it will reset to 0 each time it enters chase mode, so enemy will only charge ability when chasing.
            superSpeedActivationTimer += Time.deltaTime;
            if (superSpeedActivationTimer > superSpeedActivation)
            {
                isSuperSpeedActivated = true;
                movementSpeed = superSpeedAbilityAmount;
            }
        }

        // TODO: Fix rotations and UI
        //float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        //transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
    }

    private void ResetSuperSpeed()
    {
        isSuperSpeedActivated = false;
        superSpeedActivationTimer = 0;
        superSpeedAbilityTimer = 0;
        movementSpeed = 5f;
    }

    private void Move(Vector2 dir, float speed)
    {
        Vector2 velocity = dir * speed;
        rb.velocity = velocity;
    }

    private void Attack()
    {
        RaycastHit2D hit = Physics2D.CircleCast(transform.position, attackRadius, Vector3.zero, 10f, playerLayer);
        if (hit)
        {
            if (hitTimer > hitSpeed)
            {
                player.Hit(hitDamage);
                hitTimer = 0;
            }

            hitTimer += Time.deltaTime;
        } 
        else
        {
            state = State.Chase;
            animator.SetBool("Attack", false);
        }   
    }

    private void Die()
    {
        Instantiate(deathVFX, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }

    public void Hit(float damage)
    {
        healthSystem.DecreaseCurrentHealth(damage);
    }

}
