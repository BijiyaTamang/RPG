using UnityEngine;

public class LocationVisitedTrigger : MonoBehaviour
{
    [SerializeField] private LocationSO locationsVisited;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            GameManager.Instance.LocationHistoryTracker.RecordLocation(locationsVisited);
            Destroy(gameObject);
               
        }
    }
}
