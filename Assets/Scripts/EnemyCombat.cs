using System.Security.Cryptography.X509Certificates;
using UnityEngine;

public class EnemyCombat : MonoBehaviour
{
    public int damageAmount = 1; // Amount of damage the enemy will inflict
    public Transform attackPoint; // Reference to the attack point from which the enemy will inflict damage
    public float weaponRange; // Range within which the enemy can inflict damage
    public LayerMask playerLayer; // Layer mask to identify the player layer for collision detection


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player") // Check if the collided object is tagged as "Player"
        {

            // Inflict damage to the character on collision
            collision.gameObject.GetComponent<CharacterHealth>().healthChange(-damageAmount);
        }
    }
    public void EnemyAttack()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(attackPoint.position, weaponRange, playerLayer); // Check for collisions with the player within the attack range

        if (hits.Length > 0) // If there are any hits detected
        {
            hits[0].gameObject.GetComponent<CharacterHealth>().healthChange(-damageAmount); // Inflict damage to the first hit player object
        }
    }

}