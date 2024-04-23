using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaddlepusMovement : MonoBehaviour
{
    public Transform player;
    public LayerMask dirtLayer;

    public bool isFacingRight = false;
    public float directionX = -1f;
    public float directionY;

    public float jumpForce = 5f;
    public float jumpCooldown = 2f;
    private float lastJumpTime;

    public Sprite waddlepus;
    public Sprite waddlepus_jump;

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
        // Makes the Waddlepus always face the player
        if (player.position.x > transform.position.x)
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

        if (player.position.y > transform.position.y)
        {
            directionY = 1f;
        }
        else
        {
            directionY = -1f;
        }

        // Jump towards the player
        if (Time.time - lastJumpTime > jumpCooldown)
        {
            Jump();
            lastJumpTime = Time.time;
        }

        // Destroy Blocks if the jump is below halfway done
        if (Time.time - lastJumpTime < (jumpCooldown / 2f))
        {
            DestroyDirt();
            GetComponent<SpriteRenderer>().sprite = waddlepus_jump;
        }
        else
        {
            GetComponent<SpriteRenderer>().sprite = waddlepus;
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

    private void Jump()
    {
        GetComponent<Rigidbody2D>().AddForce(new Vector2(jumpForce * directionX, jumpForce * directionY), ForceMode2D.Impulse);
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