using System.Collections;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    public int facingDirection = 1; // 1 for right, -1 for left

    public Rigidbody2D rb; // Reference to the Rigidbody component
    public Animator anim; // Reference to the Animator component
    private bool isKnockbacked; // Flag to indicate if the enemy is currently being knocked back
    public CharacterCombat characterCombat; // Reference to the CharacterCombat script to access attack functionality
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    // Update is called 50x frame
    private void Update()
    {
        if (Input.GetButtonDown("Slash")) // Check if the space key is pressed to trigger an attack
        {
            characterCombat.Attack(); // Call the Attack method from the CharacterCombat script to trigger the attack animation
        }
    }
    void FixedUpdate()
    {
        if (isKnockbacked == false) // Check if the enemy is currently being knocked back
        {
            float horizontalInput = Input.GetAxis("Horizontal"); // Get horizontal input (A/D or Left/Right)
            float verticalInput = Input.GetAxis("Vertical"); // Get vertical input (W/S or Up/Down)

            if (horizontalInput > 0 && transform.localScale.x < 0 || // Character is moving right but currently facing left
                horizontalInput < 0 && transform.localScale.x > 0)// Character is moving left but currently facing right
            {
                Flip(); // Flip the character's facing direction if necessary
            }

            anim.SetFloat("Horizontal", Mathf.Abs(horizontalInput)); // Set the "Horizontal" parameter in the Animator to control animations
            anim.SetFloat("Vertical", Mathf.Abs(verticalInput)); // Set the "Vertical" parameter in the Animator to control animations

            rb.linearVelocity = new Vector2(horizontalInput, verticalInput) * StatsManager.Instance.moveSpeed; // Set the velocity of the Rigidbody based on input and speed
        }
    }
    void Flip()
    {
        facingDirection *= -1; // Invert the facing direction
        // Flip the character's scale to change its facing direction
        transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
    }
    public void Knockback(Transform enemy, float force, float stunTime)
    {
        isKnockbacked = true; // Set the knockback flag to true to indicate that the enemy is currently being knocked back
        Vector2 direction = (transform.position - enemy.position).normalized;
        rb.linearVelocity = direction * force;
        StartCoroutine(KnockbackCounter(stunTime));
    }

    IEnumerator KnockbackCounter(float stunTime)
    {
        yield return new WaitForSeconds(stunTime); // Wait for the specified stun time before allowing the enemy to move again
        rb.linearVelocity = Vector2.zero; // Stop any velocity to prevent the enemy from moving while stunned    
        isKnockbacked = false; // Reset the knockback flag to allow the enemy to move again
    }
}
