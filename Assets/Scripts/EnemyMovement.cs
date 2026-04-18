using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    public float speed; // Speed at which the enemy moves
    private Rigidbody2D rb; // Reference to the Rigidbody2D component for physics-based movement
    private Transform Player; // Reference to the player's Transform component to track the player's position
    private bool isChasing; // Flag to indicate whether the enemy is currently chasing the player
    private int facingDirection = -1; // 1 for right, -1 for left 
    void Start()
    {

        rb = GetComponent<Rigidbody2D>(); // Get the Rigidbody2D component attached to the enemy
    }
    
    void Update()
    {
        if(isChasing == true) // Check if the enemy is in chasing mode
        {
            if( Player.position.x > transform.position.x && facingDirection == -1 || // Player is to the right of the enemy but the enemy is currently facing left
                Player.position.x < transform.position.x && facingDirection == 1) // Player is to the left of the enemy but the enemy is currently facing right
            {
                Flip(); // Flip the enemy's facing direction if necessary
            }
            Vector2 direction = (Player.position - transform.position).normalized; // Calculate the direction vector from the enemy to the player and normalize it
            rb.linearVelocity = direction * speed; // Set the enemy's velocity to move towards the player at the specified speed
        } 
    }
    void Flip()
    {
        facingDirection *= -1; // Invert the facing direction
        // Flip the enemy's scale to change its facing direction
        transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player") // Check if the enemy collides with an object tagged as "Player"
        {
            if (Player == null) // Check if the player reference is not already set
            {
                Player = collision.transform; // Store the player's Transform component for tracking
            }
            isChasing = true; // Set the chasing flag to true when the enemy collides with the player
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player") // Check if the enemy stops colliding with an object tagged as "Player"
        {
            rb.linearVelocity = Vector2.zero; // Stop any velocity to prevent the enemy from rotating
            isChasing = false; // Set the chasing flag to false when the enemy stops colliding with the player
        }
    }
}
