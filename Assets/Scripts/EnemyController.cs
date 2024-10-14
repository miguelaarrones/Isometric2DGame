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
    [SerializeField] private float movementSpeed = 10f;
    [SerializeField] private bool followPath = false;
    [SerializeField] private LayerMask playerLayer;

    private State state;
    private Rigidbody2D rb;
    private float detectionRadius = 3f;
    private float attackRadius = .75f;
    private GameObject player;

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
                    state = State.Idle;
                break;
            default:
                Debug.LogError("ENEMY_CONTROLLER::UPDATE Invalid STATE");
                break;
        }
    }

    private void FollowPath()
    {
        throw new NotImplementedException();
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
        Debug.Log("ATTACKING PLAYER");
    }

}
