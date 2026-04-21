using UnityEngine;

public class CharacterCombat : MonoBehaviour
{
    public Animator anim; // Reference to the Animator component

    public float cooldown = 2;
    public Transform attackPoint; // Reference to the attack point transform
    public LayerMask enemyLayer; // Layer mask to specify which layers are considered enemies
    private float timer;
    public StatsUI statsUI; // Reference to the StatsUI component for updating the stats display
    private void Update()
    {
        if(timer > 0)
        {   timer -= Time.deltaTime; // Decrease the timer by the time elapsed since the last frame
        }
    }
    public void Attack()
    {
        if (!enabled) return; // block calls if component is disabled

        if (timer <= 0)
        {
            anim.SetBool("isAttacking", true);
            timer = cooldown;
        }
    }
    public void DealDamage()
    {
        //StatsManager.Instance.damage += 1; // Increase the damage stat in the StatsManager by 1 
        // statsUI.UpdateDamage(); // Update the damage display in the StatsUI to reflect the new damage value
        Collider2D[] enemies = Physics2D.OverlapCircleAll(attackPoint.position, StatsManager.Instance.weaponRange, enemyLayer); // Get all colliders within a radius of 1 unit around the character's position

        if (enemies.Length > 0)
        {
            enemies[0].GetComponent<EnemyHealth>().ChangeHealth(-StatsManager.Instance.damage); // If there are any colliders found, get the EnemyHealth component of the first collider and call the ChangeHealth method to reduce health by the damage amount
            enemies[0].GetComponent<EnemyKnockback>().Knockback(transform, StatsManager.Instance.knockbackForce, StatsManager.Instance.knockbackDuration, StatsManager.Instance.stunTime);
        }
    }
    public void FinishAttacking()
    {
        anim.SetBool("isAttacking", false); // Reset the "isAttacking" parameter in the Animator to end the attack animation
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red; // Set the Gizmos color to red
        Gizmos.DrawWireSphere(attackPoint.position, StatsManager.Instance.weaponRange); // Draw a wire sphere in the editor to visualize the weapon's attack range
    }

}
