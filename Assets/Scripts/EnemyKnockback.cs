using System.Collections;
using UnityEngine;

public class EnemyKnockback : MonoBehaviour
{
    private Rigidbody2D rb; // Reference to the Rigidbody2D component
    private EnemyMovement enemyMovement; // Reference to the EnemyMovement script to change the enemy's state during knockback

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>(); // Get the Rigidbody2D component attached to the enemy
        enemyMovement = GetComponent<EnemyMovement>(); // Get the EnemyMovement component attached to the enemy
    }

    public void Knockback(Transform characterTransform, float knockbackForce, float knockbackDuration, float stunTime)
    {
        enemyMovement.ChangeState(EnemyState.Knockback); // Change the enemy's state to Knockback to prevent it from moving or attacking during knockback
        StartCoroutine(StunTimer(knockbackDuration, stunTime)); // Start the stun timer coroutine to reset the enemy's state after the stun time is over
        Vector2 direction = (transform.position - characterTransform.position).normalized; // Calculate the direction from the character to the enemy and normalize it
        rb.linearVelocity = direction * knockbackForce; // Apply the knockback force to the enemy's Rigidbody2D
        
    }
    IEnumerator StunTimer(float knockbackDuration, float stunTime)
    {
        yield return new WaitForSeconds(knockbackDuration); // Wait for the specified stun time
        rb.linearVelocity = Vector2.zero;
        yield return new WaitForSeconds(stunTime); // Wait for the specified stun time

        enemyMovement.ChangeState(EnemyState.Idle); // Change the enemy's state back to Idle after the stun time is over
    }

}
