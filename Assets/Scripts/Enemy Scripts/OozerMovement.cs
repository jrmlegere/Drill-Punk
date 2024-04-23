using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class OozerMovement : MonoBehaviour
{
    public Transform player;

    public bool isFacingRight = false;
    public float directionX = -1f;
    public float speed = 4f;
    public float jumpingPower = 9f;

    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;

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
        // Makes the Zombie always face the player
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

        if (IsGrounded())
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpingPower);
        }

        if (rb.velocity.y > 0f)
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.5f);
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
        Vector2 boxSize = new Vector2(1.1f, 0.1f); // Adjust the size of the box as needed
        Vector2 boxCenter = new Vector2(groundCheck.position.x, groundCheck.position.y - boxSize.y / 2f);

        return Physics2D.OverlapBox(boxCenter, boxSize, 0f, groundLayer);
    }
}
