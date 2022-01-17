using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    Vector2 moveInput;
    public Vector2 deathMove = new Vector2(25f, 25f);

    public float moveSpeed = 10f;
    public float jumpSpeed = 5f;
    public float climbingSpeed = 5f;

    public bool isAlive = true;

    Rigidbody2D myRigidBody;
    CapsuleCollider2D myCollider;
    Animator myAnimator;
    BoxCollider2D myFeetCollider;

    public LayerMask ground;
    public LayerMask climbing;

    void Start()
    {
        myRigidBody = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();
        myCollider = GetComponent<CapsuleCollider2D>();
        myFeetCollider = GetComponent<BoxCollider2D>();
    }

    void Update()
    {
        if(isAlive)
        {
            Run();
            FlipSprite();
            ClimbLadder();
            OnDeath();
        }
    }

    void OnMove(InputValue value)
    {
        if(isAlive)
        {
            moveInput = value.Get<Vector2>();
        }
    }

    void OnJump(InputValue value)
    {
        if(value.isPressed && isAlive)
        {
            if(myFeetCollider.IsTouchingLayers(LayerMask.GetMask("Ground")))
            {
                myRigidBody.velocity = new Vector2(0f, jumpSpeed);
            }
        }
        
    }

    void Run()
    {
        Vector2 playerVelocity = new Vector2(moveInput.x * moveSpeed, myRigidBody.velocity.y);
        myRigidBody.velocity = playerVelocity;

        bool playerHasHorizontalSpeed = Mathf.Abs(myRigidBody.velocity.x) > Mathf.Epsilon;
        if(playerHasHorizontalSpeed)
        {
            myAnimator.SetBool("isRunning", true);
        }
        else
        {
            myAnimator.SetBool("isRunning", false);
        }
        
    }

    void FlipSprite()
    {
        bool playerHasHorizontalSpeed = Mathf.Abs(myRigidBody.velocity.x) > Mathf.Epsilon;

        if(playerHasHorizontalSpeed)
        {
            transform.localScale = new Vector2(Mathf.Sign(myRigidBody.velocity.x) * 1.5f, 1.5f);
        }
    }

    void ClimbLadder()
    {

        bool playerHasTouchedLadder = myCollider.IsTouchingLayers(climbing);
        bool playerHasVerticalSpeed = Mathf.Abs(myRigidBody.velocity.y) > Mathf.Epsilon;

        if(playerHasTouchedLadder)
        {
            Vector2 climbingVelocity = new Vector2(myRigidBody.velocity.x, moveInput.y * climbingSpeed);
            myRigidBody.velocity = climbingVelocity;
            myRigidBody.gravityScale = 0;
        }
        else
        {
            myRigidBody.gravityScale = 5.5f;
        }

        if(playerHasVerticalSpeed && playerHasTouchedLadder)
        {
            myAnimator.SetBool("isClimbing", true);
        }
        else
        {
            myAnimator.SetBool("isClimbing", false);
        }
    }

    void OnDeath()
    {
        if(myCollider.IsTouchingLayers(LayerMask.GetMask("Enemies")))
        {
            isAlive = false;
            myAnimator.SetTrigger("Dying");
            myRigidBody.velocity = deathMove;
        }
    }
}