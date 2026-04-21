using UnityEngine;

public class Arrow : MonoBehaviour
{
    public Rigidbody2D rb; // Reference to the Rigidbody2D component of the arrow
    public LayerMask enemyLayer;
    public LayerMask obstacleLayer;
    public SpriteRenderer sr;
    public Sprite buriedSprite;
    public Vector2 direction = Vector2.right; // Direction in which the arrow will be fired
    public float speed; // Speed at which the arrow will move
    public int damage; // Damage that the arrow will deal to enemies
    public float knockbackForce; // Force of the knockback applied to enemies hit by the arrow
    public float knockbackTime; // Duration of the knockback effect applied to enemies hit by the arrow
    public float stunTime; // Duration of the stun effect applied to enemies hit by the arrow
    public float lifeSpan = 2; // Time in seconds before the arrow is destroyed
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb.linearVelocity = direction * speed;
        RotateArrow();
        Destroy(gameObject, lifeSpan); // Schedule the destruction of the arrow after the specified lifespan
    }
    private void RotateArrow()
    {
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg; // Calculate the angle of the arrow based on its direction
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle)); // Rotate the arrow to face the direction it is moving
    }
    public void OnCollisionEnter2D(Collision2D collision)
    {
        if ((enemyLayer.value & (1 << collision.gameObject.layer)) > 0)
        {
            collision.gameObject.GetComponent<EnemyHealth>().ChangeHealth(-damage); // If the arrow collides with an enemy, get the EnemyHealth component of the collided object and call the ChangeHealth method to reduce health by the damage amount
            collision.gameObject.GetComponent<EnemyKnockback>().Knockback(transform, knockbackForce, knockbackTime, stunTime);
            AttachToTarget(collision.gameObject.transform); // If the arrow collides with an obstacle, call the AttachToTarget method to attach the arrow to the obstacle
        }
        else if((obstacleLayer.value & (1 << collision.gameObject.layer)) > 0)
        {
            AttachToTarget(collision.gameObject.transform); // If the arrow collides with an obstacle, call the AttachToTarget method to attach the arrow to the obstacle

        }
    }
    private void AttachToTarget(Transform target)
    {
        sr.sprite = buriedSprite; // Change the sprite of the arrow to the buried sprite to visually indicate that it is stuck in the target
        rb.linearVelocity = Vector2.zero; // Stop the arrow's movement by setting its velocity to zero
        //rb.isKinematic = true; // Set the Rigidbody2D to kinematic to prevent it from being affected by physics forces
        rb.bodyType = RigidbodyType2D.Kinematic;
        transform.SetParent(target); // Set the parent of the arrow to the target it collided with, so it will move with the target if it moves

    }
}
