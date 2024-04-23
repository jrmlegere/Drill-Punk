using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WomsterMovement : MonoBehaviour
{
    public EnemyManager enemyManager;
    public Transform player;
    public LayerMask dirtLayer;

    public float moveSpeed = 2f;
    public float speedBuff;
    public bool isFacingRight = false;
    public float direction = 180f;

    public Sprite w1;
    public Sprite w2;

    public bool belowHalf = false;

    void Start()
    {
        // Find the player object by its name (you can also use a tag)
        player = GameObject.Find("Player").transform;
        if (player == null)
        {
            Debug.LogError("Player not found! Make sure the player object has the name 'Player'.");
        }

        StartCoroutine(ChangeSprite());
    }

    // Update is called once per frame
    void Update()
    {
        // Makes the Womster always face the player
        if (player.position.x < transform.position.x)
        {
            if (isFacingRight)
            {
                FlipY();
            }
        }
        else
        {
            if (!isFacingRight)
            {
                FlipY();
            }
        }

        LookAtPlayer();
        MoveTowardsPlayer();
        DestroyDirt();

        // Womster moves slightly faster when within 10 blocks of the player
        if ((player.position.x - transform.position.x < 10f) && (player.position.y - transform.position.y < 10f))
        {
            speedBuff = 1.5f;
        }
        else
        {
            speedBuff = 1f;
        }

        // Womster will run away when below half health
        if ((enemyManager.health <= enemyManager.healthMax / 2f) && belowHalf == false)
        {
            moveSpeed = moveSpeed * -2f;
            direction = 0f;
            FlipY2();
            belowHalf = true;
        }
    }

    private void FlipY()
    {
        Vector3 localScale = transform.localScale;
        localScale.y *= -1f;
        transform.localScale = localScale;
        isFacingRight = !isFacingRight;
    }

    private void FlipY2()
    {
        Vector3 localScale = transform.localScale;
        localScale.y *= -1f;
        transform.localScale = localScale;
    }

    private void LookAtPlayer()
    {
        Vector3 directionToPlayer = player.position - transform.position;
        float angle = Mathf.Atan2(directionToPlayer.y, directionToPlayer.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle + direction));
    }

    void MoveTowardsPlayer()
    {
        // Calculate the direction to move towards the player
        Vector3 directionToPlayer = (player.position - transform.position).normalized;

        // Set the velocity to move towards the player at a constant speed
        GetComponent<Rigidbody2D>().velocity = new Vector2(directionToPlayer.x * moveSpeed * speedBuff, directionToPlayer.y * moveSpeed * speedBuff);
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

    IEnumerator ChangeSprite()
    {
        while (true)
        {
            GetComponent<SpriteRenderer>().sprite = (GetComponent<SpriteRenderer>().sprite == w1) ? w2 : w1;
            yield return new WaitForSeconds(0.4615f);
        }
    }
}
