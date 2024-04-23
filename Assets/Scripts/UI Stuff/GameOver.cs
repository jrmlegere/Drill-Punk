using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOver : MonoBehaviour
{
    public GameObject player;
    public GameObject baby;
    public GameObject screen;
    public Text text1;
    public Text text2;

    public bool hasPlayedLoserSound = false;
    public AudioSource loserSound;

    // Update is called once per frame
    void Update()
    {
        if (baby.GetComponent<BabyHealth>().dead)
        {
            text1.text = "BABY DIED.";
            text2.text = "BABY DIED.";
        }
        else
        {
            text1.text = "YOU DIED.";
            text2.text = "YOU DIED.";
        }

        if (player.GetComponent<PlayerHealth>().dead)
        {
            screen.SetActive(true);
        }
        else
        {
            screen.SetActive(false);
        }

        if (player.GetComponent<PlayerHealth>().dead && !hasPlayedLoserSound)
        {
            loserSound.PlayOneShot(loserSound.clip);
            hasPlayedLoserSound = true;
        }
    }

    public void ReloadScene()
    {
        // Get the current scene index
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;

        // Reload the current scene
        SceneManager.LoadScene(currentSceneIndex);
    }
}
