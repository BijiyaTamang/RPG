using UnityEngine;

public class CharacterBow : MonoBehaviour
{
    public Transform launchPoint;
    public GameObject arrowPrefab;
    public Vector2 shootDirection= Vector2.right;
    public Animator anim;
    public CharacterMovement characterMovement; // Reference to the CharacterMovement script to control movement while shooting
    public float shootCooldown = .5f; // Time in seconds between shots
    private float shootTimer; // Time when the last shot was fired

    // Update is called once per frame
    void Update()
    {
        shootTimer -= Time.deltaTime;
        HandleAiming();
        if (Input.GetButtonDown("Shoot") && shootTimer <= 0)
        {
            characterMovement.isShooting = true; // Set the isShooting flag in the CharacterMovement script to true to prevent movement while shooting
            anim.SetBool("isShooting", true);
        }
    }
    private void OnEnable()
    {
        anim.SetLayerWeight(0, 0); // Set the weight of the base layer to 0 to disable base animations when the bow is equipped
        anim.SetLayerWeight(1, 1); // Set the weight of the aiming layer to 1 to enable aiming animations when the bow is equipped
    }

    private void OnDisable()
    {
        anim.SetLayerWeight(0, 1); // Set the weight of the base layer to 1 to enable base animations when the bow is unequipped
        anim.SetLayerWeight(1, 0); // Set the weight of the aiming layer to 0 to disable aiming animations when the bow is unequipped
    }
    private void HandleAiming()
    {
        float horizontal = Input.GetAxisRaw("Horizontal"); // Get horizontal input from the player (e.g., A/D keys or left/right arrow keys)
        float vertical = Input.GetAxisRaw("Vertical"); // Get vertical input from the player (e.g., W/S keys or up/down arrow keys)

        if(horizontal != 0 || vertical !=0)
        {
            // Calculate the shoot direction based on the input and normalize it to ensure consistent speed in all directions
            shootDirection = new Vector2(horizontal, vertical).normalized;
            anim.SetFloat("aimX", shootDirection.x); // Set the "aimX" parameter in the Animator to control the horizontal aiming animation
            anim.SetFloat("aimY", shootDirection.y); // Set the "aimY" parameter in the Animator to control the vertical aiming animation
        }
    }
    public void Shoot()
    {
        if (shootTimer <= 0)
        {
            Arrow arrow = Instantiate(arrowPrefab, launchPoint.position, Quaternion.identity).GetComponent<Arrow>();
            arrow.direction = shootDirection;
            arrow.damage = StatsManager.Instance.arrowDamage;       // read from StatsManager
            arrow.speed = StatsManager.Instance.arrowSpeed;         // read from StatsManager
            arrow.stunTime = StatsManager.Instance.arrowStunTime;   // read from StatsManager
            shootTimer = shootCooldown;
        }
        anim.SetBool("isShooting", false);
        characterMovement.isShooting = false;
    }
}
