using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class SaplingMaker : MonoBehaviour
{
    public GameObject sapling;
    private bool isMakingSapling = false;
    public Text updates;
    public AudioSource saplingAudio;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T) && !isMakingSapling && GetComponent<Inventory>().saplingCount >= 1)
        {
            if ((transform.position.y >= -5f) && (transform.position.y < 30f))
            {
                StartCoroutine(MakeSaplingCoroutine());
            }
            else
            {
                ShowMessage("You can't plant a Sapling here!");
            }
        }

        if (Input.GetKeyUp(KeyCode.T))
        {
            StopCoroutine(MakeSaplingCoroutine());
            isMakingSapling = false;
            ClearMessage();
        }
    }

    IEnumerator MakeSaplingCoroutine()
    {
        isMakingSapling = true;
        float timer = 0f;
        float timeToMakeSapling = 3f;

        while (timer < timeToMakeSapling)
        {
            // Check if C key is released
            if (!Input.GetKey(KeyCode.T))
            {
                isMakingSapling = false;
                ClearMessage();
                yield break;
            }

            timer += Time.deltaTime;

            // Calculate the remaining time
            float remainingTime = timeToMakeSapling - timer;

            // Update the message with the countdown
            ShowMessage($"Planting a Sapling in {Mathf.CeilToInt(remainingTime)}");

            yield return null;
        }

        ShowMessage($"A new Sapling has been planted!");

        // Make the camp after 3 seconds
        MakeSapling((int)transform.position.x + 0.5f, ((int)transform.position.y) - 1.5f);
        saplingAudio.PlayOneShot(saplingAudio.clip);
        GetComponent<Inventory>().saplingCount--;
        isMakingSapling = false;

        // Clear the message after 3 seconds
        yield return new WaitForSeconds(3f);
        ClearMessage();
    }

    void ShowMessage(string message)
    {
        updates.text = message;
    }

    void ClearMessage()
    {
        updates.text = "";
    }

    public void MakeSapling(float x, float y)
    {
        Vector2 saplingPosition = new Vector2(x, y);
        GameObject newSapling = Instantiate(sapling, saplingPosition, Quaternion.identity);
        newSapling.GetComponent<OreBehaviour>().x = x;
        newSapling.GetComponent<OreBehaviour>().y = y;
        newSapling.GetComponent<SaplingBehaviour>().x = (int) x;
        newSapling.GetComponent<SaplingBehaviour>().y = (int) y;
    }
}