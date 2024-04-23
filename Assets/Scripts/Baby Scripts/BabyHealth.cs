using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class BabyHealth : MonoBehaviour
{
    // General flags
    public bool dead = false;
    public bool isOnBed = false;

    // Layer mask for bed detection
    public LayerMask bedLayer;

    // UI text for displaying stats
    public Image healthImage;
    public Image hungerImage;
    public Image energyImage;
    public Image loveImage;
    public StatsSprites statsSprites;

    // Health variables
    public int health;
    public int healthMax = 3;
    public float healthCounter = 0f;
    public bool isFlashing = false;
    public int damageCounter = 0;

    // Hunger variables
    public int hunger;
    public int hungerMax = 1;
    public float hungerTime = 24f;
    public float starvingTime = 6f;
    public float hungerCounter = 0f;
    public float starvingCounter = 0f;
    public float healthCounter_hunger = 0f;
    public int hungerHealCounter = 0;

    // Energy variables
    public int energy;
    public int energyMax = 4;
    public float energyTime = 3f;
    public float energyTime_heal = 2f;
    public float tiredTime = 6f;
    public float energyCounter = 0f;
    public float tiredCounter = 0f;
    public float healthCounter_energy = 0f;
    public float regainEnergyCounter = 0f;

    // Love variables
    public int love;
    public int loveMax = 4;
    public float loveTime_heal = 2f;
    public float loveTime = 3f;
    public float sadTime = 6f;
    public float loveCounter = 0f;
    public float sadCounter = 0f;
    public float healthCounter_love = 0f;
    public float regainLoveCounter = 0f;

    // Poison variables
    public bool isPoisoned = false;
    public float poisonDuration = 10f;
    public float poisonDamageInterval = 2f;
    public float poisonDamageTimer = 0f;
    public float poisonTimer = 0f;

    void Start()
    {
        health = healthMax;
        hunger = hungerMax;
        energy = energyMax;
        love = loveMax;
    }

    // Update is called once per frame
    void Update()
    {
        if (damageCounter >= 3)
        {
            TakeDamage(1);
            damageCounter = 0;
        }

        // Timer is set up to keep track of hunger ------------------------------------------------------------------------------------
        if (hunger != 0f)
        {
            hungerCounter += Time.deltaTime;
        }
        else
        {
            hungerCounter = 0f;
        }

        if (hungerCounter >= 60f * hungerTime && hunger != 0)
        {
            hungerCounter = 0f;
            hunger = hunger - 1;
            StartCoroutine(HungerFlash());
            healthCounter_hunger = 0f;
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
            healthCounter_hunger += Time.deltaTime;
            if (healthCounter_hunger >= 10f)
            {
                healthCounter_hunger = 0f;
                TakeDamage(1);
            }
        }

        // Timer is set up to keep track of energy ------------------------------------------------------------------------------------
        Collider2D[] touchingBedCheck = Physics2D.OverlapCircleAll(transform.position, 1f, bedLayer);

        if (touchingBedCheck.Length != 0)
        {
            isOnBed = true;
        }
        else
        {
            isOnBed = false;
        }

        if (energy != 0f && !isOnBed)
        {
            energyCounter += Time.deltaTime;
        }
        else
        {
            energyCounter = 0f;
        }

        if (energyCounter >= 60f * energyTime && energy != 0)
        {
            energyCounter = 0f;
            energy = energy - 1;
            StartCoroutine(EnergyFlash());
            healthCounter_energy = 0f;
        }

        // When energy reaches 0, a second timer starts before you start taking damage
        if (energy <= 0)
        {
            tiredCounter += Time.deltaTime;
        }
        else
        {
            tiredCounter = 0f;
        }

        // When that ends, you take 1 point of damage every 10 seconds
        if (tiredCounter >= 60f * tiredTime)
        {
            healthCounter_energy += Time.deltaTime;
            if (healthCounter_energy >= 10f)
            {
                healthCounter_energy = 0f;
                TakeDamage(1);
            }
        }

        // Timer is set up to keep track of love ------------------------------------------------------------------------------------
        if (love != 0f && !GetComponent<CarryBaby>().isSnapped)
        {
            loveCounter += Time.deltaTime;
        }
        else
        {
            loveCounter = 0f;
        }

        if (loveCounter >= 60f * loveTime && love != 0)
        {
            loveCounter = 0f;
            love = love - 1;
            StartCoroutine(LoveFlash());
            healthCounter_love = 0f;
        }

        // When love reaches 0, a second timer starts before you start taking damage
        if (love <= 0)
        {
            sadCounter += Time.deltaTime;
        }
        else
        {
            sadCounter = 0f;
        }

        // When that ends, you take 1 point of damage every 10 seconds
        if (sadCounter >= 60f * sadTime)
        {
            healthCounter_love += Time.deltaTime;
            if (healthCounter_love >= 10f)
            {
                healthCounter_love = 0f;
                TakeDamage(1);
            }
        }

        if (hunger == hungerMax)
        {
            Heal();
        }

        if (isOnBed)
        {
            Sleep();
        }
        else
        {
            regainEnergyCounter = 0f;
        }

        if (GetComponent<CarryBaby>().isSnapped)
        {
            Love();
        }
        else
        {
            regainLoveCounter = 0f;
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

                if (hungerHealCounter >= 5)
                {
                    hunger--;
                    hungerHealCounter = 0;
                }
            }
        }
    }

    public void Sleep()
    {
        if (!dead)
        {
            regainEnergyCounter += Time.deltaTime;
            if (regainEnergyCounter >= 60f * energyTime_heal && energy < energyMax)
            {
                regainEnergyCounter = 0f;
                energy = energy + 1;
            }
        }
    }

    public void Love()
    {
        if (!dead)
        {
            regainLoveCounter += Time.deltaTime;
            if (regainLoveCounter >= 60f * loveTime_heal && love < loveMax)
            {
                regainLoveCounter = 0f;
                love = love + 1;
            }
        }
    }

    public void TakeDamage(int damage)
    {
        if (health > 0 && !isFlashing)
        {
            health = health - damage;
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

    IEnumerator EnergyFlash()
    {
        energyImage.color = Color.red;

        // Wait for half a second
        yield return new WaitForSeconds(0.5f);

        // Change sprite back to normal
        energyImage.color = Color.white;
    }

    IEnumerator LoveFlash()
    {
        loveImage.color = Color.red;

        // Wait for half a second
        yield return new WaitForSeconds(0.5f);

        // Change sprite back to normal
        loveImage.color = Color.white;
    }

    public void ApplyPoison()
    {
        // Reset poisonTimer to 0 if ApplyPoison is called while poison is already active
        if (isPoisoned)
        {
            poisonTimer = 0f;
        }
        isPoisoned = true;
    }

    public void SetUI()
    {
        healthImage.sprite = statsSprites.bHealth[health];
        hungerImage.sprite = statsSprites.bHunger[hunger];
        energyImage.sprite = statsSprites.bEnergy[energy];
        loveImage.sprite = statsSprites.bLove[love];
    }
}