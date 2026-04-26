using System.Collections;
using UnityEngine;

public class EnemyRespawn : MonoBehaviour
{
    public float respawnTime = 10f;
    private Vector2 spawnPosition;
    private EnemyMovement enemyMovement;
    private EnemyHealth enemyHealth;

    void Start()
    {
        spawnPosition = transform.position;
        enemyMovement = GetComponent<EnemyMovement>();
        enemyHealth = GetComponent<EnemyHealth>();
    }

    public void Die()
    {
        StartCoroutine(RespawnCoroutine());
    }

    IEnumerator RespawnCoroutine()
    {
        // Disable everything
        enemyMovement.enabled = false;
        enemyHealth.enabled = false;
        GetComponent<Collider2D>().enabled = false;
        GetComponent<SpriteRenderer>().enabled = false;

        yield return new WaitForSeconds(respawnTime);

        // Reset position
        transform.position = spawnPosition;

        // Re-enable everything
        enemyMovement.enabled = true;
        enemyHealth.enabled = true;
        GetComponent<Collider2D>().enabled = true;
        GetComponent<SpriteRenderer>().enabled = true;

        // Reset health and state
        enemyHealth.ResetHealth();
        enemyMovement.ChangeState(EnemyState.Idle);
        GetComponent<Animator>().SetBool("isReturning", false);
        GetComponent<Animator>().SetBool("isChasing", false);
        GetComponent<Animator>().SetBool("isIdle", true);
    }
}