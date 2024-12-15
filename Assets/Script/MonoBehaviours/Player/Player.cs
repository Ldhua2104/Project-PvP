using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Planes
{
    public int lifes;
    public float fireRate;
    float nextFireTime;

    private Vector2 spawnPointLeft;
    private Vector2 spawnPointRight;

    Animator animator;
    string animationState = "AnimationState";

    Rigidbody2D rb;

    enum CharState
    {
        moveLeft = 4,
        moveRight = 6,
        moveUp = 8,
        moveDown = 2,
        idle = 5
    }
    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    protected override void SpawnBullet()
    {
        if (Input.GetKey(KeyCode.Space) && nextFireTime >= fireRate)
        {
            spawnPointLeft = new Vector2(transform.position.x - 0.5f, transform.position.y);
            spawnPointRight = new Vector2(transform.position.x + 0.6f, transform.position.y);

            ObjectPool.Instance.SpawnObjectFromPool("PlayerBullet", spawnPointLeft);
            ObjectPool.Instance.SpawnObjectFromPool("PlayerBullet", spawnPointRight);

            nextFireTime = 0;
        }
    }

    protected override void Move()
    {
        float MoveX = Input.GetAxisRaw("Horizontal");
        float MoveY = Input.GetAxisRaw("Vertical");

        moveDirection = new Vector2(MoveX, MoveY).normalized;

        rb.velocity = moveDirection * moveSpeed;
    }

    private void UpdateMovement()
    {
        if (moveDirection.x > 0)
            animator.SetInteger(animationState, (int)CharState.moveRight);
        else if (moveDirection.x < 0)
            animator.SetInteger(animationState, (int)CharState.moveLeft);
        else if (moveDirection.y > 0)
            animator.SetInteger(animationState, (int)CharState.moveUp);
        else if (moveDirection.y < 0)
            animator.SetInteger(animationState, (int)CharState.moveDown);
        else
            animator.SetInteger(animationState, (int)CharState.idle);

    }

    private void HandleInput()
    {
        SpawnBullet();
        Move();
    }

    private void Update()
    {
        nextFireTime += Time.deltaTime;
        
        HandleInput();
    }

    private void FixedUpdate()
    {
        UpdateMovement();
    }

    protected override void ReceiveDamage()
    {
        throw new System.NotImplementedException();
    }

    protected override void SendDamage()
    {
        throw new System.NotImplementedException();
    }

    protected override void die()
    {
        throw new System.NotImplementedException();
    }
}
