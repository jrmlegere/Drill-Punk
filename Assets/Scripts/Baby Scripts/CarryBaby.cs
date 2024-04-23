using UnityEngine;

public class CarryBaby : MonoBehaviour
{
    public GameObject player;
    public float snapDistance = 2f;
    public float snapSpeed = 5f;
    public Vector3 offset = new Vector3(0f, 0.4f, 0.1f); // Adjust the offset as needed
    private Quaternion originalRotation;
    public PlayerMovement playerMovement;

    public AudioSource pickupAudio;

    public bool isSnapped = false;

    void Start()
    {
        transform.position = new Vector2(0f, -25f);
    }

    void Update()
    {
        if (player.GetComponent<PlayerHealth>().dead)
        {
            isSnapped = false;
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.LeftShift) && !player.GetComponent<PlayerMovement>().isPaused)
            {
                if (isSnapped)
                {
                    // Release the object
                    isSnapped = false;
                    GetComponent<Rigidbody2D>().velocity = Vector2.zero;
                }
                else
                {
                    // Check if the player is nearby
                    float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);
                    if (distanceToPlayer < snapDistance)
                    {
                        // Snap the object to the player
                        isSnapped = true;
                        pickupAudio.PlayOneShot(pickupAudio.clip);
                    }
                }
            }
        }

        // Handle snapping and releasing
        if (isSnapped)
        {
            if (playerMovement.isFacingRight)
            {
                offset.x = -0.6f;
            }
            else
            {
                offset.x = 0.6f;
            }
            // Set the position of the baby to the target position
            transform.position = player.transform.position + offset;
            transform.rotation = originalRotation;
        }
    }
}