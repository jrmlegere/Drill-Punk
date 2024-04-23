using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    public bool dead = false;

    public GameObject baby;

    public Image healthImage;
    public Image hungerImage;
    public StatsSprites statsSprites;

    public int health;
    public int healthMax = 10;
    public int hunger;
    public int hungerMax = 3;
    public float hungerTime = 8f;
    public float starvingTime = 6f;
    public float elapsedCounter = 0f;
    public float starvingCounter = 0f;
    public float healthCounter = 0f;

    public bool isFlashing = false;

    public bool isPoisoned = false;
    public float poisonDuration = 10f;
    public float poisonDamageInterval = 2f;
    public float poisonDamageTimer = 0f;
    public float poisonTimer = 0f;

    public int hungerHealCounter = 0;

    public PlayerMovement playerMovement;

    void Start()
    {
        health = healthMax;
        hunger = hungerMax;
        playerMovement = GetComponent<PlayerMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        if (baby.GetComponent<BabyHealth>().dead)
        {
            dead = true;
        }

        // Timer is set up to keep track of hunger
        if (hunger != 0f)
        {
            elapsedCounter += Time.deltaTime;
        }
        else
        {
            elapsedCounter = 0f;
        }

        if (elapsedCounter >= 60f * hungerTime && hunger != 0)
        {
            elapsedCounter = 0f;
            hunger = hunger - 1;
            StartCoroutine(HungerFlash());
            healthCounter = 0f;
        }

        // When hunger reaches 0, a second timer starts before you start taking damage
        if (hunger <= 0)
        {
            starvingCounter += Time.deltaTime;
        }
        else
        {
            starvingCounter = 0f;
        }

        // When that ends, you take 1 point of damage every 10 seconds
        if (starvingCounter >= 60f * starvingTime)
        {
            healthCounter += Time.deltaTime;
            if (healthCounter >= 10f)
            {
                healthCounter = 0f;
                TakeDamage(1);
            }
        }

        if (hunger == hungerMax)
        {
            Heal();
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
                    if (health > 1)
                    {
                        TakeDamage(1); // Adjust damage amount as needed
                    }
                }
            }
        }

        SetUI();
    }

    public void Heal()
    {
        if (!dead)
        {
            healthCounter += Time.deltaTime;
            if (healthCounter >= 10f && health < healthMax)
            {
                healthCounter = 0f;
                health = health + 1;
                hungerHealCounter++;

                if (hungerHealCounter >= 4)
                {
                    hunger--;
                    hungerHealCounter = 0;
                }
            }
        }
    }

    public void TakeDamage(int damage)
    {
        if (health > 0 && !isFlashing)
        {
            health = health - damage;
            if (baby.GetComponent<CarryBaby>().isSnapped)
            {
                baby.GetComponent<BabyHealth>().damageCounter += 1;
            }
            StartCoroutine(FlashSprite());
        }

        if (health <= 0)
        {
            dead = true;
        }
    }

    IEnumerator FlashSprite()
    {
        isFlashing = true;

        // Change sprite to red
        if (isPoisoned)
        {
            GetComponent<SpriteRenderer>().color = Color.green;
            healthImage.color = Color.green;
        }
        else
        {
            GetComponent<SpriteRenderer>().color = Color.red;
            healthImage.color = Color.red;
        }

        // Wait for half a second
        yield return new WaitForSeconds(0.5f);

        // Change sprite back to normal
        GetComponent<SpriteRenderer>().color = Color.white;
        healthImage.color = Color.white;

        isFlashing = false;
    }

    IEnumerator HungerFlash()
    {
        hungerImage.color = Color.red;

        // Wait for half a second
        yield return new WaitForSeconds(0.5f);

        // Change sprite back to normal
        hungerImage.color = Color.white;
    }

    public void ApplyPoison()
    {
        if (!playerMovement.radSuit)
        {
            // Reset poisonTimer to 0 if ApplyPoison is called while poison is already active
            if (isPoisoned)
            {
                poisonTimer = 0f;
            }
            isPoisoned = true;
        }
        if (baby.GetComponent<CarryBaby>().isSnapped)
        {
            baby.GetComponent<BabyHealth>().ApplyPoison();
        }
    }

    public void SetUI()
    {
        healthImage.sprite = statsSprites.health[health];
        hungerImage.sprite = statsSprites.hunger[hunger];
    }
}