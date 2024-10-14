using System.Collections;
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
    [SerializeField] private float maxMovementSpeed = 5f;
    [SerializeField] private float movementSpeed = 10f;

    private State state;
    private Rigidbody2D rb;

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
                break;
            case State.Patrol:
                break;
            case State.Chase:
                break;
            case State.Attack:
                break;
        }
    }
}
