using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{

    public float enemyMoveSpeed = 5f;

    Rigidbody2D myRigidBody;
    BoxCollider2D myBoxCollider;

    void Start()
    {
        myRigidBody = GetComponent<Rigidbody2D>();
        myBoxCollider = GetComponent<BoxCollider2D>();
    }

    void Update()
    {
        myRigidBody.velocity = new Vector2(enemyMoveSpeed, 0f);
    }

    void OnTriggerExit2D(Collider2D other)
    {
        enemyMoveSpeed = -enemyMoveSpeed;
        FlipEnemy();
        
    }

    void FlipEnemy()
    {
        transform.localScale = new Vector2(-(Mathf.Sign(myRigidBody.velocity.x)), 1f);
    }
}
