using System.Collections;
using UnityEngine;
using UnityEngine.XR;

public class EnemyMovement : MonoBehaviour
{
    public float speed; // Speed at which the enemy moves
    private Rigidbody2D rb; // Reference to the Rigidbody2D component for physics-based movement
    private Transform Player; // Reference to the player's Transform component to track the player's position
    private int facingDirection = -1; // 1 for right, -1 for left 
    private Animator anim; // Reference to the Animator component for controlling animations
    private EnemyState enemyState; // Current state of the enemy 
    public float attackRange = 1; // Range within which the enemy will attack the player
    public float attackCooldown = 2; // Time between attacks    
    private float attackCooldownTimer; // Timer to track the cooldown between attacks
    public float playerDetectionRange = 5; // Range within which the enemy can detect the player
    public Transform detectionPoint; // Reference to the point from which the enemy will detect the player
    public LayerMask playerLayer; // Layer mask to specify which layers the enemy should consider as the player
    void Start()
    {
        rb = GetComponent<Rigidbody2D>(); // Get the Rigidbody2D component attached to the enemy
        anim = GetComponent<Animator>(); // Get the Animator component attached to the enemy
        ChangeState(EnemyState.Idle); // Set the initial state of the enemy to Idle
    }

    void Update()
    {
        if (enemyState != EnemyState.Knockback)
        { 
            CheckForPlayer(); // Check for the player's presence and update the enemy's state accordingly
            if (attackCooldownTimer > 0)
            {
                attackCooldownTimer -= Time.deltaTime;
            }
            if (enemyState == EnemyState.Chasing) // Check if the enemy is in chasing mode
            {
                EnemyChase(); // Call the Chase method to handle chasing behavior
            }
            else if (enemyState == EnemyState.Attacking) // Check if the enemy is in attacking mode
            {
                rb.linearVelocity = Vector2.zero; // Stop any velocity to prevent the enemy from rotating while attacking
            }
        }
    }
    void EnemyChase()
    {
        if (Player.position.x > transform.position.x && facingDirection == -1 || // Player is to the right of the enemy but the enemy is currently facing left
                Player.position.x < transform.position.x && facingDirection == 1) // Player is to the left of the enemy but the enemy is currently facing right
        {
            Flip(); // Flip the enemy's facing direction if necessary
        }
        Vector2 direction = (Player.position - transform.position).normalized; // Calculate the direction vector from the enemy to the player and normalize it
        rb.linearVelocity = direction * speed; // Set the enemy's velocity to move towards the player at the specified speed
    }


    void Flip()
    {
        facingDirection *= -1; // Invert the facing direction
        // Flip the enemy's scale to change its facing direction
        transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
    }
    private void CheckForPlayer()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(detectionPoint.position, playerDetectionRange, playerLayer); // Check for colliders within a certain radius around the enemy
        if (hits.Length > 0) // If there are any colliders detected
        {
            Player = hits[0].transform; // Store the player's Transform component for tracking
            // Change the enemy's state to Chasing if the player is detected within the detection range
            if (Vector2.Distance(transform.position, Player.transform.position) < attackRange && attackCooldownTimer <= 0) // Check if the player is within attack range
            {
                attackCooldownTimer = attackCooldown; // Reset the attack cooldown timer
                ChangeState(EnemyState.Attacking); // Change the enemy's state to Attacking if the player is within range
            }
            else if (Vector2.Distance(transform.position, Player.transform.position) > attackRange && enemyState != EnemyState.Attacking) // Check if the player is within detection range but not within attack range
            {
                ChangeState(EnemyState.Chasing); // Change the enemy's state to Chasing if the player is within detection range but not within attack range
            }
        }
        else
        {
            rb.linearVelocity = Vector2.zero; // Stop any velocity to prevent the enemy from rotating
            ChangeState(EnemyState.Idle); // Change the enemy's state to Idle if no player is detected within the detection range
        }
    }

    public void ChangeState(EnemyState newState)
    {
        // Reset the animation parameters for the previous state before transitioning to the new state
        if (enemyState == EnemyState.Idle)
            anim.SetBool("isIdle", false); // Set the "isIdle" parameter in the Animator to false when transitioning from Idle state
        else if (enemyState == EnemyState.Chasing)
            anim.SetBool("isChasing", false); // Set the "isChasing" parameter in the Animator to false when transitioning from Chasing state
        else if (enemyState == EnemyState.Attacking)
            anim.SetBool("isAttacking", false); // Set the "isAttacking" parameter in the Animator to false when transitioning from Attacking state

        enemyState = newState; // Update the enemy's state

        // Set the appropriate animation parameters based on the new state
        if (enemyState == EnemyState.Idle)
            anim.SetBool("isIdle", true); // Set the "isIdle" parameter in the Animator to true when transitioning to Idle state
        else if (enemyState == EnemyState.Chasing)
            anim.SetBool("isChasing", true); // Set the "isChasing" parameter in the Animator to true when transitioning to Chasing state
        else if (enemyState == EnemyState.Attacking)
            anim.SetBool("isAttacking", true); // Set the "isAttacking" parameter in the Animator to true when transitioning to Attacking state
    }
    private void OnDrawGizmosSelected()
    {
        // Draw a wire sphere in the editor to visualize the player detection range
        Gizmos.color = Color.red; // Set the Gizmos color to red
        Gizmos.DrawWireSphere(detectionPoint.position, playerDetectionRange); // Draw a wire sphere at the detection point with the specified radius for player detection
    }
}

public enum EnemyState
{
    Idle,
    Chasing,
    Attacking,
    Knockback
}