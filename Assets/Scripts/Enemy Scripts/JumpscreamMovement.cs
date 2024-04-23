using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpscreamMovement : MonoBehaviour
{
    public Transform player;

    public bool isFacingRight = false;
    public float directionX = -1f;
    public float speed;
    public float jumpingPower = 9f;

    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;

    public float explodeRadius = 4.0f;

    public bool isHidden = true;

    public AudioSource boomSound;
    public AudioSource digSound;

    void Start()
    {
        // Find the player object by its name (you can also use a tag)
        player = GameObject.Find("Player").transform;
        Transform soundsTransform = player.transform.Find("Sounds");
        if (soundsTransform != null)
        {
            Transform bombTransform = soundsTransform.Find("Bomb");
            if (bombTransform != null)
            {
                boomSound = bombTransform.GetComponent<AudioSource>();
            }
        }
        if (player == null)
        {
            Debug.LogError("Player not found! Make sure the player object has the name 'Player'.");
        }

        GetComponent<SpriteRenderer>().enabled = false;
        GetComponent<BoxCollider2D>().size = new Vector2(0.1f, 0.1f);
        speed = 4f;
        GetComponent<EnemyManager>().silent = true;
        digSound.Play();
        digSound.loop = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (isHidden && Vector2.Distance(transform.position, player.position) < 3f)
        {
            isHidden = false;
            GetComponent<SpriteRenderer>().enabled = true;
            GetComponent<BoxCollider2D>().size = new Vector2(3.0f, 4.75f);
            speed = 6f;
            GetComponent<EnemyManager>().silent = false;
            Explode();
            digSound.Stop();
        }

        if (isHidden)
        {
            MoveTowardsPlayer();
            DestroyDirt();
        }
        else
        {
            // Makes the JumpScream always face the player
            if (player.position.x < transform.position.x)
            {
                if (isFacingRight)
                {
                    Flip();
                }
            }
            else
            {
                if (!isFacingRight)
                {
                    Flip();
                }
            }

            rb.velocity = new Vector2(directionX * speed, rb.velocity.y);
        }
    }

    void MoveTowardsPlayer()
    {
        // Calculate the direction to move towards the player
        Vector3 directionToPlayer = (player.position - transform.position).normalized;

        // Set the velocity to move towards the player at a constant speed
        GetComponent<Rigidbody2D>().velocity = new Vector2(directionToPlayer.x * speed, directionToPlayer.y * speed);
    }

    private void DestroyDirt()
    {
        // Define the bounds of the box (adjust as needed)
        Bounds bounds = GetComponent<BoxCollider2D>().bounds;

        // Check for colliders within the bounds on the dirt layer
        Collider2D[] colliders = Physics2D.OverlapBoxAll(bounds.center, bounds.size, 0f, groundLayer);

        // Iterate through colliders and destroy objects on the dirt layer
        foreach (Collider2D collider in colliders)
        {
            OreBehaviour oreBehaviour = collider.GetComponent<OreBehaviour>();
            if (oreBehaviour != null)
            {
                collider.GetComponent<OreBehaviour>().Drop();
            }
            Destroy(collider.gameObject);
        }
    }

    private void Flip()
    {
        Vector3 localScale = transform.localScale;
        localScale.x *= -1f;
        transform.localScale = localScale;
        isFacingRight = !isFacingRight;
        directionX *= -1f;
    }

    private bool IsGrounded()
    {
        Vector2 boxSize = new Vector2(1.4f, 0.05f); // Adjust the size of the box as needed
        Vector2 boxCenter = new Vector2(groundCheck.position.x, groundCheck.position.y - boxSize.y / 2f);

        return Physics2D.OverlapBox(boxCenter, boxSize, 0f, groundLayer);
    }

    private void Explode()
    {
        // Create a new CircleCollider2D
        CircleCollider2D explosionCollider = gameObject.AddComponent<CircleCollider2D>();
        explosionCollider.radius = explodeRadius;

        // Check for colliders within the bounds of the new circle collider on the dirt layer
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, explodeRadius, groundLayer);

        // Iterate through colliders and destroy objects on the dirt layer
        foreach (Collider2D collider in colliders)
        {
            OreBehaviour oreBehaviour = collider.GetComponent<OreBehaviour>();
            if (oreBehaviour != null)
            {
                collider.GetComponent<OreBehaviour>().Drop();
            }
            Destroy(collider.gameObject);
        }
        boomSound.PlayOneShot(boomSound.clip);
    }
}