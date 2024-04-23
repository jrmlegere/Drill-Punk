using System.Collections;
using UnityEngine;

public class BabyShake : MonoBehaviour
{
    public GameObject player;
    public float shakeIntensity = 1f;
    private Vector3 originalLocalPosition;
    public AudioSource shakeAudio;
    private bool isShaking = false;

    void Update()
    {
        originalLocalPosition = transform.localPosition;

        if (Input.GetKeyDown(KeyCode.B) && !player.GetComponent<PlayerMovement>().isPaused)
        {
            StartShake();
        }

        if (Input.GetKeyUp(KeyCode.B) || player.GetComponent<PlayerMovement>().isPaused)
        {
            StopShake();
        }
    }

    void StartShake()
    {
        if (!isShaking)
        {
            isShaking = true;
            StartCoroutine(Shake());
            shakeAudio.Play();
            shakeAudio.loop = true;
        }
    }

    void StopShake()
    {
        if (isShaking)
        {
            isShaking = false;
            StopAllCoroutines();
            shakeAudio.Stop();
        }
    }

    IEnumerator Shake()
    {
        while (isShaking)
        {
            float x = Random.Range(-1f, 1f) * shakeIntensity;
            float y = Random.Range(-1f, 1f) * shakeIntensity;

            transform.localPosition = originalLocalPosition + new Vector3(x, y, 0f);

            yield return null;
        }
    }
}