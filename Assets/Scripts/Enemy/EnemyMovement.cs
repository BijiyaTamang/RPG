using System.Collections;
using UnityEngine;

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
    private Vector2 startPosition; // Stores the enemy's original spawn position to return to after chasing

    void Start()
    {
        rb = GetComponent<Rigidbody2D>(); // Get the Rigidbody2D component attached to the enemy
        anim = GetComponent<Animator>(); // Get the Animator component attached to the enemy
        startPosition = transform.position; // Save the enemy's spawn position on start
        Invoke(nameof(InitState), 0.05f); // Wait a frame for Animator to initialize before setting state
    }

    void InitState()
    {
        ChangeState(EnemyState.Idle); // Set the initial state of the enemy to Idle
    }


    void FixedUpdate()
    {
        if (enemyState == EnemyState.Chasing)
            EnemyChase(); // Handle chasing movement in FixedUpdate for consistent physics
        else if (enemyState == EnemyState.Returning)
            ReturnToStart(); // Handle return movement in FixedUpdate for consistent physics
    }

    void EnemyChase()
    {
        if (Player == null) return;

        if (Player.position.x > transform.position.x && facingDirection == -1 || // Player is to the right but enemy faces left
            Player.position.x < transform.position.x && facingDirection == 1) // Player is to the left but enemy faces right
            Flip(); // Flip the enemy's facing direction if necessary

        Vector2 direction = (Player.position - transform.position).normalized; // Calculate normalized direction to player
        rb.MovePosition(rb.position + direction * speed * Time.fixedDeltaTime); // Move towards player using MovePosition for reliable physics
    }
    private float returnCooldown = 0.5f;
    private float returnCooldownTimer = 0f;

    void Update()
    {
        if (enemyState != EnemyState.Knockback)
        {
            if (attackCooldownTimer > 0)
                attackCooldownTimer -= Time.deltaTime;

            if (returnCooldownTimer > 0)
                returnCooldownTimer -= Time.deltaTime;

            if (enemyState != EnemyState.Returning)
                CheckForPlayer();

            if (enemyState == EnemyState.Attacking)
                rb.linearVelocity = Vector2.zero;
        }
    }

    private void CheckForPlayer()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(detectionPoint.position, playerDetectionRange, playerLayer);

        if (hits.Length > 0)
        {
            Player = hits[0].transform;
            float dist = Vector2.Distance(transform.position, Player.position);

            if (dist < attackRange && attackCooldownTimer <= 0)
            {
                attackCooldownTimer = attackCooldown;
                ChangeState(EnemyState.Attacking);
            }
            else if (dist > attackRange && enemyState != EnemyState.Attacking)
            {
                ChangeState(EnemyState.Chasing);
            }
        }
        else
        {
            if (returnCooldownTimer <= 0) // Only return if cooldown expired
                ChangeState(EnemyState.Returning);
        }
    }

    void ReturnToStart()
    {
        float distanceToStart = Vector2.Distance(transform.position, startPosition);

        if (distanceToStart > 0.1f)
        {
            if (startPosition.x > transform.position.x && facingDirection == -1 ||
                startPosition.x < transform.position.x && facingDirection == 1)
                Flip();

            Vector2 direction = ((Vector2)startPosition - (Vector2)transform.position).normalized;
            rb.MovePosition(rb.position + direction * speed * Time.fixedDeltaTime);
        }
        else
        {
            transform.position = startPosition;
            rb.linearVelocity = Vector2.zero;
            anim.SetBool("isReturning", false);
            anim.SetBool("isIdle", true);
            returnCooldownTimer = returnCooldown; // Set cooldown when reaching start
            enemyState = EnemyState.Returning;
            ChangeState(EnemyState.Idle);
        }
    }

    void Flip()
    {
        facingDirection *= -1; // Invert the facing direction
        transform.localScale = new Vector3(
            transform.localScale.x * -1, // Flip the enemy's scale to change its facing direction
            transform.localScale.y,
            transform.localScale.z
        );
    }

    public void ChangeState(EnemyState newState)
    {
        if (enemyState == newState) return; // Avoid resetting the same state repeatedly

        enemyState = newState; // Update the enemy's state

        // Set all animation bools based on current state in one clean block
        anim.SetBool("isIdle", newState == EnemyState.Idle);
        anim.SetBool("isChasing", newState == EnemyState.Chasing);
        anim.SetBool("isAttacking", newState == EnemyState.Attacking);
        anim.SetBool("isReturning", newState == EnemyState.Returning);
    }

    public EnemyState GetCurrentState() => enemyState; // Expose current state for debugging and other scripts

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red; // Set the Gizmos color to red
        Gizmos.DrawWireSphere(detectionPoint.position, playerDetectionRange); // Draw detection range in editor
    }
}

public enum EnemyState
{
    Idle,
    Chasing,
    Attacking,
    Knockback,
    Returning
}