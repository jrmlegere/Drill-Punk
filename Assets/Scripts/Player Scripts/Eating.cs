using UnityEngine;

public class Eating : MonoBehaviour
{
    public GameObject player;
    public GameObject baby;

    public int playerHunger;
    public int babyHunger;

    public int steakCount;
    public int dogCount;
    public int honeyhamCount;
    public int rationsCount;

    public AudioSource eatAudio;
    public AudioSource errorAudio;

    private void Update()
    {
        playerHunger = player.GetComponent<PlayerHealth>().hunger;
        babyHunger = baby.GetComponent<BabyHealth>().hunger;

        steakCount = player.GetComponent<Inventory>().steakCount;
        dogCount = player.GetComponent<Inventory>().dogCount;
        honeyhamCount = player.GetComponent<Inventory>().honeyhamCount;
        rationsCount = player.GetComponent<Inventory>().rationsCount;
    }

    public void OnButtonPress()
    {
        if (steakCount + dogCount + honeyhamCount + rationsCount > 0)
        {
            bool feedBaby = false;

            if (playerHunger < 3 || ((babyHunger < 1) && (baby.GetComponent<CarryBaby>().isSnapped)))
            {
                eatAudio.PlayOneShot(eatAudio.clip);
                if (babyHunger < 1 && baby.GetComponent<CarryBaby>().isSnapped)
                {
                    feedBaby = true;
                }

                // Create a list of available food types
                int[] availableFoodTypes = new int[] { steakCount, dogCount, honeyhamCount, rationsCount };

                // Randomly choose a food type from the available list
                int randomNumber;
                do
                {
                    randomNumber = Random.Range(0, availableFoodTypes.Length);
                } while (availableFoodTypes[randomNumber] == 0);

                // Decrement the chosen food count
                DecrementFoodCount(randomNumber);

                // Increment hunger based on who is being fed
                if (feedBaby)
                {
                    baby.GetComponent<BabyHealth>().hunger++;
                }
                else
                {
                    player.GetComponent<PlayerHealth>().hunger++;
                }
            }
            else
            {
                errorAudio.PlayOneShot(errorAudio.clip);
            }
        }
        else
        {
            errorAudio.PlayOneShot(errorAudio.clip);
        }
    }

    private void DecrementFoodCount(int foodType)
    {
        switch (foodType)
        {
            case 0:
                player.GetComponent<Inventory>().steakCount--;
                break;
            case 1:
                player.GetComponent<Inventory>().dogCount--;
                break;
            case 2:
                player.GetComponent<Inventory>().honeyhamCount--;
                break;
            case 3:
                player.GetComponent<Inventory>().rationsCount--;
                break;
        }
    }
}