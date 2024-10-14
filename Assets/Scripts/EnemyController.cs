using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows;


enum State
{
    Idle,
    Patrol,
    Chase,
    Attack
}
public class EnemyController : MonoBehaviour
{
    [SerializeField] private float maxMovementSpeed = 5f;
    [SerializeField] private float maxPatrolSpeed = 2f;
    [SerializeField] private float movementSpeed = 10f;
    [SerializeField] private bool followPath = false;
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private List<Transform> path;
    [SerializeField] private float detectionRadius = 3f;
    [SerializeField] private float attackRadius = .75f;

    private State state;
    private Rigidbody2D rb;
    private GameObject player;
    private int currentPathPoint = 0;

    // Start is called before the first frame update
    void Start()
    {
        state = State.Idle;
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        switch (state)
        {
            case State.Idle:
                if (followPath)
                    state = State.Patrol;
                else
                    if (CheckPlayerInRadius())
                    state = State.Chase;
                break;
            case State.Patrol:
                FollowPath();
                if (CheckPlayerInRadius())
                    state = State.Chase;
                break;
            case State.Chase:
                ChasePlayer();
                if (!CheckPlayerInRadius())
                {
                    // TODO: Maybe I can do something better than that?
                    rb.velocity = new Vector3(0, 0, 0);
                    state = State.Idle;
                }
                break;
            case State.Attack:
                Attack();
                if (!CheckPlayerInRadius())
                {
                    // TODO: Maybe I can do something better than that?
                    rb.velocity = new Vector3(0, 0, 0);
                    state = State.Idle;
                }
                break;
            default:
                Debug.LogError("ENEMY_CONTROLLER::UPDATE Invalid STATE");
                break;
        }
    }

    private void FollowPath()
    {
        if (path.Count < 0)
        {
            state = State.Idle;
            return;
        }

        Vector2 dir = (path[currentPathPoint].position - transform.position).normalized;

        Vector2 velocity = dir * movementSpeed;
        rb.velocity = velocity;

        if (rb.velocity.magnitude > maxPatrolSpeed)
        {
            rb.velocity = rb.velocity.normalized * maxPatrolSpeed;
        }
        

        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));

        if (Vector2.Distance(path[currentPathPoint].position, transform.position) < 0.1)
        {
            // TODO: I know this could be better using modulo (&)
            currentPathPoint++;
            if (currentPathPoint == path.Count)
                currentPathPoint = 0;

            currentPathPoint = currentPathPoint % path.Count;
        }
    }

    private bool CheckPlayerInRadius()
    {
        RaycastHit2D hit = Physics2D.CircleCast(transform.position, detectionRadius, Vector3.zero, 10f, playerLayer);
        if (hit)
        {
            player = hit.transform.gameObject;
            return true;
        }
        else
        {
            player = null;
            return false;
        }

    }

    private void ChasePlayer()
    {
        Vector2 dir = (player.transform.position - transform.position).normalized;

        if (Vector2.Distance(player.transform.position, transform.position) > attackRadius)
        {
            Vector2 velocity = dir * movementSpeed;
            rb.velocity = velocity;

            if (rb.velocity.magnitude > maxMovementSpeed)
            {
                rb.velocity = rb.velocity.normalized * maxMovementSpeed;
            }
        }
        else
        {
            state = State.Attack;
            rb.velocity = Vector2.zero;
        }

        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
    }

    private void Attack()
    {
        if (Vector2.Distance(player.transform.position, transform.position) > attackRadius)
        {
            state = State.Patrol;
            return;
        }
        Debug.Log("ATTACKING PLAYER");
    }

}
