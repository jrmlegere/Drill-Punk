using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UILogic : MonoBehaviour
{
    public GameObject player;
    public Text coordinates;
    public Text time;
    public Text scoreText;
    public Text updates;
    public Text ammo;

    public GameObject health;
    public GameObject hunger;
    public GameObject bHealth;
    public GameObject bHunger;
    public GameObject bEnergy;
    public GameObject bHappy;

    public GameObject ammoBox;

    public float minutesPerDay = 24f;
    private float elapsedTime = 0f;
    public float day = 1f;
    public float minutes;
    public float seconds;
    private float previousScreenWidth = -1f;

    public int score;

    // Update is called once per frame
    void Update()
    {
        coordinates.text = "X: " + (int)player.transform.position.x + " / Y: " + (int)player.transform.position.y + " ";

        if (!player.GetComponent<PlayerHealth>().dead)
        {
            // Increase elapsed time based on real-time
            elapsedTime += Time.deltaTime;
        }

        if (elapsedTime >= minutesPerDay * 60f)
        {
            // Reset elapsed time to 0 to start a new "day"
            elapsedTime = 0f;
            day = day + 1f;
            score += 1000;
        }

        minutes = Mathf.Floor(elapsedTime / 60f);
        seconds = elapsedTime % 60f;

        time.text = " Day " + day + "\n " + string.Format("{0:00}:{1:00}", minutes, seconds);

        scoreText.text = "Score: " + score;

        // Check if the screen resolution has changed
        if (Screen.width != previousScreenWidth)
        {
            AdjustTextAndImageSize();
            previousScreenWidth = Screen.width;
        }
    }

    void AdjustTextAndImageSize()
    {
        float screenWidth = Screen.width;
        float screenHeight = Screen.height;
        int newSize = Mathf.RoundToInt(screenWidth / 30f);

        // Adjust font size for text elements
        coordinates.fontSize = newSize;
        time.fontSize = newSize;
        scoreText.fontSize = newSize;
        updates.fontSize = newSize;
        ammo.fontSize = newSize;

        // Adjust the size of the updates text box (RectTransform width)
        RectTransform updatesRectTransform = updates.GetComponent<RectTransform>();
        Vector2 newSizeDelta = updatesRectTransform.sizeDelta;
        newSizeDelta.x = screenWidth / 3f;  // You can adjust this multiplier as needed
        updatesRectTransform.sizeDelta = newSizeDelta;

        // Assuming health, hunger, bHealth, bHunger, bEnergy, and bHappy are Image components
        float needSize = screenHeight * 0.14f;

        health.GetComponent<RectTransform>().sizeDelta = new Vector2(needSize, needSize);
        health.GetComponent<RectTransform>().position = new Vector2(screenWidth * 0.05f - needSize * 0.5f, screenHeight * 0.17f - needSize * 0.5f);

        hunger.GetComponent<RectTransform>().sizeDelta = new Vector2(needSize, needSize);
        hunger.GetComponent<RectTransform>().position = new Vector2(screenWidth * 0.135f - needSize * 0.5f, screenHeight * 0.17f - needSize * 0.5f);

        bHealth.GetComponent<RectTransform>().sizeDelta = new Vector2(needSize, needSize);
        bHealth.GetComponent<RectTransform>().position = new Vector2(screenHeight * 0.02f, screenHeight * 0.02f);

        bHunger.GetComponent<RectTransform>().sizeDelta = new Vector2(needSize, needSize);
        bHunger.GetComponent<RectTransform>().position = new Vector2(screenHeight * 0.095f, screenHeight * 0.02f);

        bEnergy.GetComponent<RectTransform>().sizeDelta = new Vector2(needSize, needSize);
        bEnergy.GetComponent<RectTransform>().position = new Vector2(screenHeight * 0.17f, screenHeight * 0.02f);

        bHappy.GetComponent<RectTransform>().sizeDelta = new Vector2(needSize, needSize);
        bHappy.GetComponent<RectTransform>().position = new Vector2(screenHeight * 0.245f, screenHeight * 0.02f);

        ammoBox.GetComponent<RectTransform>().sizeDelta = new Vector2(screenHeight * 0.068f, screenHeight * 0.068f);
        ammoBox.GetComponent<RectTransform>().position = new Vector2(screenHeight * 1.684f, screenHeight * 0.852f);
    }
}