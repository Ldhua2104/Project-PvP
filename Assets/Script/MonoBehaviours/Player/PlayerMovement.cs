using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class playermove : MonoBehaviour
{
    public float moveSpeed;
    Vector2 moveDirection;

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

    void Update()
    {
       UpdateMovement();
    }
    private void FixedUpdate()
    {
        MovePlayer();
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
    private void MovePlayer()
    {
        float MoveX = Input.GetAxisRaw("Horizontal");
        float MoveY = Input.GetAxisRaw("Vertical");

        moveDirection = new Vector2(MoveX, MoveY).normalized;

        rb.velocity = moveDirection * moveSpeed;
    }
}
