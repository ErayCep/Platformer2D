using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement2 : MonoBehaviour
{
    Vector2 moveInput;

    public float moveSpeed = 10f;
    public float jumpSpeed = 5f;
    public float attackRange = 1f;
    public float attackRate = 2f;
    public int attackDamage = 20;

    float nextAttackTime = 0;

    Rigidbody2D myRigidbody;
    Animator animator;
    BoxCollider2D myFeetCollider;
    public Transform attackPoint;
    public LayerMask enemyLayer;

    void Start()
    {
        myRigidbody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        myFeetCollider = GetComponent<BoxCollider2D>();
    }

    void Update()
    {   
        
        Run();
        FlipSprite();

        if(Time.time >= nextAttackTime)
        {
            if(Input.GetKeyDown(KeyCode.F))
            {
                Attack();
                nextAttackTime = Time.time +  1f / attackRate;
            }
        }
        
    }

    void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
    }

    void OnJump(InputValue value)
    {
        if(value.isPressed)
        {     
            if(myFeetCollider.IsTouchingLayers(LayerMask.GetMask("Ground")))
            {
                myRigidbody.velocity = new Vector2(0f, jumpSpeed);
                animator.SetBool("isJumping", false);
            }
            else
            {
                animator.SetBool("isJumping", true);
            }
        }
    }

    void Run()
    {
        Vector2 playerVelocity = new Vector2(moveInput.x * moveSpeed, myRigidbody.velocity.y);
        myRigidbody.velocity = playerVelocity;

        bool playerHasHorizontalSpeed = Mathf.Abs(myRigidbody.velocity.x) > Mathf.Epsilon;
        if(playerHasHorizontalSpeed && myFeetCollider.IsTouchingLayers(LayerMask.GetMask("Ground")))
        {
            animator.SetBool("isRunning", true);
        }
        else
        {
            animator.SetBool("isRunning", false);
        }
    }

    void Attack()
    {
        animator.SetTrigger("Attack");
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayer);

        foreach(Collider2D enemy in hitEnemies)
        {
            enemy.GetComponent<Enemy>().TakeDamage(attackDamage);
        }
    }

    void FlipSprite()
    {
        bool playerHasHorizontalSpeed = Mathf.Abs(myRigidbody.velocity.x) > Mathf.Epsilon;
        if(playerHasHorizontalSpeed)
        {
            transform.localScale = new Vector2(Mathf.Sign(myRigidbody.velocity.x) * 3f, 3f);
        }
    }

    void OnDrawGizmosSelected()
    {
        if(attackPoint == null)
        {
            return;
        }

        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}
