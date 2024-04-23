using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BomblebeeMovement : MonoBehaviour
{
    public EnemyManager enemyManager;
    public Transform player;
    public LayerMask dirtLayer;
    public GameObject bomb;

    public float moveSpeed = 2f;
    public bool isFacingRight = false;

    public float throwCooldown = 4f;
    private float lastThrowTime;

    void Start()
    {
        // Find the player object by its name (you can also use a tag)
        player = GameObject.Find("Player").transform;

        if (player == null)
        {
            Debug.LogError("Player not found! Make sure the player object has the name 'Player'.");
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Makes the Womster always face the player
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

        MoveTowardsPlayer();

        // Throw Bomb towards the player
        if (Time.time - lastThrowTime > throwCooldown)
        {
            GameObject Bomblebomb = Instantiate(bomb, new Vector2(transform.position.x, transform.position.y), Quaternion.identity);
            lastThrowTime = Time.time;
        }
    }

    private void Flip()
    {
        Vector3 localScale = transform.localScale;
        localScale.x *= -1f;
        transform.localScale = localScale;
        isFacingRight = !isFacingRight;
    }

    void MoveTowardsPlayer()
    {
        // Calculate the direction to move towards the player
        Vector3 directionToPlayer = (player.position - transform.position).normalized;

        // Set the velocity to move towards the player at a constant speed
        GetComponent<Rigidbody2D>().velocity = new Vector2(directionToPlayer.x * moveSpeed, directionToPlayer.y * moveSpeed);
    }

    private void DestroyDirt()
    {
        // Define the bounds of the box (adjust as needed)
        Bounds bounds = GetComponent<BoxCollider2D>().bounds;

        // Check for colliders within the bounds on the dirt layer
        Collider2D[] colliders = Physics2D.OverlapBoxAll(bounds.center, bounds.size, 0f, dirtLayer);

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
}
