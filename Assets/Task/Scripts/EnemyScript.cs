using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScript : MonoBehaviour
{
    public Transform target; // Reference to the player's transform
    public float speed; // Speed at which the enemy moves
    public float attackRange=2; // Range at which the enemy attacks

    private Rigidbody2D rb;
    private Animator animator;
    private bool isAttacking;
    private float attackCooldown = 1f;
    private float attackTimer;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        target = GameObject.FindGameObjectWithTag("Player").transform;
        attackTimer = attackCooldown;
    }

    private void Update()
    {
        // Check if the target (player) is not null
        if (target != null)
        {
            // Calculate the distance to the target
            float distanceToTarget = Vector2.Distance(transform.position, target.position);

            if (!isAttacking)
            {
                // Move towards the target if not within attack range
                if (distanceToTarget > attackRange)
                {
                    Vector2 direction = (target.position - transform.position).normalized;
                    rb.velocity = direction * speed;

                    // Set animation parameters based on movement direction
                    if (direction.magnitude > 0)
                    {
                        if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
                        {
                            animator.SetInteger("Direction", direction.x > 0 ? 2 : 3);
                        }
                        else
                        {
                            animator.SetInteger("Direction", direction.y > 0 ? 1 : 0);
                        }
                    }
                }
                else
                {
                    rb.velocity = Vector2.zero;
                    isAttacking = true;
                    attackTimer = attackCooldown;
                    StartCoroutine(Attack());
                }
            }
            else
            {
                attackTimer -= Time.deltaTime;
                if (attackTimer <= 0)
                {
                    isAttacking = false;
                }
            }
        }
    }

    IEnumerator Attack()
    {
        target.gameObject.GetComponent<TopDownCharacterController>().HealthReduce(10);
        yield return new WaitForSeconds(1f); // Wait for 1 second
    }
}
