using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Planes
{
    public int lifes;
    public float fireRate;
    float nextFireTime;

    public AudioSource m_AudioSource;
    public AudioClip m_Clip;

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

    protected override void SetDefault()
    {
        belong = "PlayerPlane";
        hitPoints = 100;
        maxHitPoints = 100;
        atk = 34;
        moveSpeed = 10;
        lifes = 3;
        fireRate = 0.12f;
    }

    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        m_AudioSource = GetComponent<AudioSource>();
        m_AudioSource.clip = m_Clip;

        SetDefault();
    }

    protected override void PlaySounds()
    {
        if (m_Clip != null)
            m_AudioSource.PlayOneShot(m_Clip);
    }

    protected override void SpawnBullet()
    {
        if (Input.GetKey(KeyCode.Space) && nextFireTime >= fireRate)
        {
            spawnPointLeft = new Vector2(transform.position.x - 0.5f, transform.position.y);
            spawnPointRight = new Vector2(transform.position.x + 0.6f, transform.position.y);

            PlaySounds();

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

    protected override void Die()
    {
        throw new System.NotImplementedException();
    }

    protected override void HandleCollision(GameObject other)
    {
        if(other.CompareTag("Enemy"))
        {
            Die();
        }

        if(other.CompareTag("EnemyBullet"))
        {
            ReceiveDamage();
        }
    }
}
