using UnityEngine;
using System.Collections;
using UnityEditor.Experimental.GraphView;

public class NPC_Patrol : MonoBehaviour
{
    public float speed = 2; // Speed at which the NPC moves
    public Vector2[] patrolPoints; // Array of patrol points for the NPC to follow

    public float pauseDuration = 1.5f;

    private bool isPaused;

    private Vector2 target;

    private int currentPatrolIndex; // Index to keep track of the current patrol point

    private Rigidbody2D rb;
    private Animator anim;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponentInChildren<Animator>();
        StartCoroutine(SetPatrolPoint()); // Start the patrol routine
    }

    // Update is called once per frame
    void Update()
    {
        if (isPaused)
        {
            rb.linearVelocity = Vector2.zero;
            return;
        }
        Vector2 direction = ((Vector3)target - transform.position).normalized; // Calculate the direction towards the target
        if(direction.x < 0 && transform.localScale.x > 0 || direction.x > 0 && transform.localScale.x < 0)
            transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z); // Flip the NPC sprite based on the direction of movement
        rb.linearVelocity = direction * speed; // Move the NPC towards the target direction
   
        if(Vector2.Distance(transform.position, target) < 0.1) // Check if the NPC is close enough to the target
        {
           StartCoroutine(SetPatrolPoint()); // Move to the next patrol point
        }
    }



    IEnumerator SetPatrolPoint()
    {
        isPaused = true; // Pause the NPC movement
        anim.Play("Idle"); // Play the idle animation while paused
        yield return new WaitForSeconds(pauseDuration); // Wait for the specified pause duration
       
        currentPatrolIndex = (currentPatrolIndex + 1) % patrolPoints.Length; // Move to the next patrol point
        target = patrolPoints[currentPatrolIndex];
        isPaused = false; // Resume the NPC movement
        anim.Play("Walk"); // Play the walk animation when resuming movement
    }
}
