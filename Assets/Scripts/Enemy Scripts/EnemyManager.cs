using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public GameObject logic;
    public GameObject player;
    public LayerMask playerLayer;
    public LayerMask bulletLayer;
    public float health;
    public float healthMax;
    public int damage;
    public float immunity;
    private bool isFlashing = false;
    public GameObject dropSprite;
    public float despawnDistance = 71f;

    public bool isPoisonous = false;

    public bool isPoisoned = false;
    public float poisonDuration = 6f;
    public float poisonDamageInterval = 2f;
    public float poisonDamageTimer = 0f;
    public float poisonTimer = 0f;

    public bool silent = false;
    public AudioSource sound1;
    public AudioSource sound2;
    public AudioSource sound3;
    public float soundCooldown;
    public float lastSoundTime;


    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        logic = GameObject.FindGameObjectWithTag("Logic");

        health = healthMax;
        soundCooldown = Random.Range(3, 6);
    }

    void Update()
    {
        if ((Time.time - lastSoundTime > soundCooldown) && !silent)
        {
            soundCooldown = Random.Range(3, 6);
            int r = Random.Range(1, 4);
            if (r == 1)
            {
                sound1.PlayOneShot(sound1.clip);
            }
            else if (r == 2)
            {
                sound2.PlayOneShot(sound2.clip);
            }
            else if (r == 3)
            {
                sound3.PlayOneShot(sound3.clip);
            }
            lastSoundTime = Time.time;
        }

        float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);

        if (distanceToPlayer > despawnDistance)
        {
            Destroy(gameObject);
        }

        // Poison Mechanic
        if (isPoisoned)
        {
            poisonTimer += Time.deltaTime;
            poisonDamageTimer += Time.deltaTime;

            if (poisonTimer >= poisonDuration)
            {
                // Poison duration has ended
                isPoisoned = false;
                poisonTimer = 0f;
            }
            else
            {
                // Player takes poison damage every 2 seconds
                if (poisonDamageTimer >= poisonDamageInterval)
                {
                    poisonDamageTimer = 0f;
                    if (health > 0)
                    {
                        health = health - 1;
                        StartCoroutine(FlashSprite());
                    }
                    if (health <= 0)
                    {
                        Drop();
                        Destroy(gameObject);
                    }
                }
            }
        }
    }

    public void ApplyPoison()
    {
        if (!isPoisonous)
        {
            // Reset poisonTimer to 0 if ApplyPoison is called while poison is already active
            if (isPoisoned)
            {
                poisonTimer = 0f;
            }
            isPoisoned = true;
        }
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        // When Enemies collide with the player, the player takes damage
        if ((playerLayer.value & 1 << collision.gameObject.layer) != 0)
        {
            PlayerHealth playerHealth = collision.gameObject.GetComponent<PlayerHealth>();

            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damage);
            }
            if (isPoisonous)
            {
                playerHealth.ApplyPoison();
            }
        }

        // When Enemies collide with bullets, the enemy takes damage
        if ((bulletLayer.value & 1 << collision.gameObject.layer) != 0)
        {
            if (health > 0 && !isFlashing)
            {
                health = health - collision.collider.GetComponent<BulletBreak>().damage;
                StartCoroutine(FlashSprite());
            }
            if (health <= 0)
            {
                Drop();
                Destroy(gameObject);
            }
            if (collision.collider.GetComponent<BulletBreak>().type == 4)
            {
                ApplyPoison();
            }
            Destroy(collision.gameObject);
        }
    }


        IEnumerator FlashSprite()
        {
            isFlashing = true;

            // Change sprite to red
            if (isPoisoned)
            {
                GetComponent<SpriteRenderer>().color = Color.green;
            }
            else
            {
                GetComponent<SpriteRenderer>().color = Color.red;
            }

            // Wait for half a second
            yield return new WaitForSeconds(0.5f);

            // Change sprite back to normal
            GetComponent<SpriteRenderer>().color = Color.white;

            isFlashing = false;
        }

        void Drop()
        {
            if (dropSprite != null)
            {
                GameObject drop = Instantiate(dropSprite, new Vector2(transform.position.x, transform.position.y), Quaternion.identity);
                drop.layer = 11;
                drop.tag = "Drop";

                // Destroy the drop object after 10 seconds
                Destroy(drop, 60f);
            }
            logic.GetComponent<UILogic>().score += 500;
        }
}