using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombBehaviour : MonoBehaviour
{
    public Transform player;
    public LayerMask dirtLayer;
    public LayerMask playerLayer;

    public float throwForce = 10f;
    public float explodeTime = 3f;
    public float spawnTime;
    public float explodeRadius = 6.0f;
    public int explodeDamage = 2;

    public AudioSource hissSound;
    public AudioSource boomSound;

    // Start is called before the first frame update
    void Start()
    {
        // Find the player object by its name (make sure the player object has the correct name)
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
        Jump();
        spawnTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time - spawnTime < (explodeTime / 3f))
        {
            DestroyDirt();
        }

        if (Time.time - spawnTime > explodeTime)
        {
            Explode();
        }
    }

    private void Jump()
    {
        Vector2 throwDirection = (player.position - transform.position).normalized;
        GetComponent<Rigidbody2D>().AddForce(throwDirection * throwForce, ForceMode2D.Impulse);
    }

    private void DestroyDirt()
    {
        // Define the bounds of the box (adjust as needed)
        Bounds bounds = GetComponent<CircleCollider2D>().bounds;

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

    private void Explode()
    {
        // Create a new CircleCollider2D
        CircleCollider2D explosionCollider = gameObject.AddComponent<CircleCollider2D>();
        explosionCollider.radius = explodeRadius;

        // Check for colliders within the bounds of the new circle collider on the dirt layer
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, explodeRadius, dirtLayer);

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

        colliders = Physics2D.OverlapCircleAll(transform.position, explodeRadius, playerLayer);

        foreach (Collider2D collider in colliders)
        {
            PlayerHealth playerHealth = collider.gameObject.GetComponent<PlayerHealth>();

            if (playerHealth != null)
            {
                playerHealth.TakeDamage(explodeDamage);
            }
        }

        // Remove the temporary circle collider
        Destroy(explosionCollider);
        boomSound.PlayOneShot(boomSound.clip);

        // Destroy the bomb object
        Destroy(gameObject);
    }
}
