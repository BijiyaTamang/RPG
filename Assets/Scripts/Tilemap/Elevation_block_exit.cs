using UnityEngine;

public class Elevation_block_exit : MonoBehaviour
{
    public Collider2D[] blockColliders; // Reference to the block's colliders
    public Collider2D[] Boundary;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            foreach (Collider2D block in blockColliders)
            {
                block.enabled = true;

            }
            foreach (Collider2D block in Boundary)
            {
                block.enabled = false;

            }
            collision.gameObject.GetComponent<SpriteRenderer>().sortingOrder = 1; // Set the sorting order to 1 to render the player above the block
        }
    }
}
