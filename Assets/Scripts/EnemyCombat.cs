using UnityEngine;

public class EnemyCombat : MonoBehaviour
{
    public int damageAmount; // Amount of damage the enemy will inflict

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player") // Check if the collided object is not tagged as "Player"
        {

            // Inflict damage to the character on collision
            collision.gameObject.GetComponent<CharacterHealth>().healthChange(-damageAmount);
        }
    }

}
