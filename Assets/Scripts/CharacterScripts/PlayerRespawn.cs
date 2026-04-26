using UnityEngine;
public class PlayerRespawn : MonoBehaviour
{
    public static PlayerRespawn Instance;
    private Vector3 startPosition;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        startPosition = transform.position; // save starting position on game load
    }

    public void Respawn()
    {
        StatsManager.Instance.currentHealth = StatsManager.Instance.maxHealth;
        StatsManager.Instance.UpdateHealth(0); // refresh health UI
        transform.position = startPosition;
        gameObject.SetActive(true);
    }
}