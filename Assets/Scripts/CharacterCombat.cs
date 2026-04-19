using UnityEngine;

public class CharacterCombat : MonoBehaviour
{
    public Animator anim; // Reference to the Animator component

    public float cooldown = 2;
    public Transform attackPoint; // Reference to the attack point transform
    public float weaponRange; // Range of the weapon attack
    public LayerMask enemyLayer; // Layer mask to specify which layers are considered enemies
    public int damage = 1;
    public float knockbackForce = 15; // Force applied to enemies when hit
    public float knockbackDuration = .15f; // Duration of the knockback effect in seconds
    public float stunTime= .3f; // Time in seconds that enemies are stunned after being hit
    private float timer;    
    private void Update()
    {
        if(timer > 0)
        {   timer -= Time.deltaTime; // Decrease the timer by the time elapsed since the last frame
        }
    }
    public void Attack()
    {
        if ((timer<=0))
        {
            anim.SetBool("isAttacking", true); // Set the "isAttacking" parameter in the Animator to trigger the attack animation

            timer = cooldown; // Reset the timer to the cooldown value to prevent immediate re-attacks
        }
    }
    public void DealDamage()
    {

        Collider2D[] enemies = Physics2D.OverlapCircleAll(attackPoint.position, weaponRange, enemyLayer); // Get all colliders within a radius of 1 unit around the character's position

        if (enemies.Length > 0)
        {
            enemies[0].GetComponent<EnemyHealth>().ChangeHealth(-damage); // If there are any colliders found, get the EnemyHealth component of the first collider and call the ChangeHealth method to reduce health by the damage amount
            enemies[0].GetComponent<EnemyKnockback>().Knockback(transform, knockbackForce,knockbackDuration, stunTime);
        }
    }
    public void FinishAttacking()
    {
        anim.SetBool("isAttacking", false); // Reset the "isAttacking" parameter in the Animator to end the attack animation
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red; // Set the Gizmos color to red
        Gizmos.DrawWireSphere(attackPoint.position, weaponRange); // Draw a wire sphere in the editor to visualize the weapon's attack range
    }

}
