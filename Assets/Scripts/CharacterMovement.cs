using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    public float moveSpeed = 5; // Speed at which the character moves
    public int facingDirection = 1; // 1 for right, -1 for left

    public Rigidbody2D rb; // Reference to the Rigidbody component
    public Animator anim; // Reference to the Animator component
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    // Update is called 50x frame
    void Update()
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

        rb.linearVelocity = new Vector2(horizontalInput, verticalInput) * moveSpeed; // Set the velocity of the Rigidbody based on input and speed
    }
    void Flip()
    {
        facingDirection *= -1; // Invert the facing direction
        // Flip the character's scale to change its facing direction
        transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
    }
}
