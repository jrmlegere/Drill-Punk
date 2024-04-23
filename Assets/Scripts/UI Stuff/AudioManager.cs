using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public GameObject player;
    public GameObject logic;

    public AudioSource caveAudio;
    public AudioSource lairAudio;
    public AudioSource dayAudio;
    public AudioSource nightAudio;
    public AudioSource villageAudio;

    public int dayTime = 6;
    public int nightTime = 18;

    void Update()
    {
        if (player.transform.position.y < -5f)
        {
            if (!caveAudio.isPlaying)
            {
                // Play the audio on loop
                caveAudio.Play();
                caveAudio.loop = true;
            }
        }
        else
        {
            // Stop the audio when player is above the threshold
            if (caveAudio.isPlaying)
            {
                caveAudio.Stop();
            }
        }

        if (player.transform.position.y >= 100f && player.transform.position.y < 500f)
        {
            if (!lairAudio.isPlaying)
            {
                // Play the audio on loop
                lairAudio.Play();
                lairAudio.loop = true;
            }
        }
        else
        {
            // Stop the audio when player is above the threshold
            if (lairAudio.isPlaying)
            {
                lairAudio.Stop();
            }
        }

        if (player.transform.position.y >= 500f)
        {
            if (!villageAudio.isPlaying)
            {
                // Play the audio on loop
                villageAudio.Play();
                villageAudio.loop = true;
            }
        }
        else
        {
            // Stop the audio when player is above the threshold
            if (villageAudio.isPlaying)
            {
                villageAudio.Stop();
            }
        }

        if (player.transform.position.y >= -5f && player.transform.position.y < 100f && (logic.GetComponent<UILogic>().minutes < dayTime || logic.GetComponent<UILogic>().minutes >= nightTime))
        {
            if (!nightAudio.isPlaying)
            {
                // Play the audio on loop
                nightAudio.Play();
                nightAudio.loop = true;
            }
        }
        else
        {
            // Stop the audio when player is above the threshold
            if (nightAudio.isPlaying)
            {
                nightAudio.Stop();
            }
        }

        if (player.transform.position.y >= -5f && player.transform.position.y < 100f && (logic.GetComponent<UILogic>().minutes >= dayTime && logic.GetComponent<UILogic>().minutes < nightTime))
        {
            if (!dayAudio.isPlaying)
            {
                // Play the audio on loop
                dayAudio.Play();
                dayAudio.loop = true;
            }
        }
        else
        {
            // Stop the audio when player is above the threshold
            if (dayAudio.isPlaying)
            {
                dayAudio.Stop();
            }
        }
    }
}