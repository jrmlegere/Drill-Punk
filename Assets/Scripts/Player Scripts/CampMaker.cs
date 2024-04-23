using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class CampMaker : MonoBehaviour
{
    public GameObject camp;
    private bool isMakingCamp = false;
    public Text updates;
    public AudioSource campAudio;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C) && !isMakingCamp && GetComponent<Inventory>().campCount >= 1)
        {
            if (transform.position.y < -10f)
            {
                StartCoroutine(MakeCampCoroutine());
            }
            else
            {
                ShowMessage("You can't set up camp here!");
            }
        }

        if (Input.GetKeyUp(KeyCode.C))
        {
            StopCoroutine(MakeCampCoroutine());
            isMakingCamp = false;
            ClearMessage();
        }
    }

    IEnumerator MakeCampCoroutine()
    {
        isMakingCamp = true;
        float timer = 0f;
        float timeToMakeCamp = 3f;

        while (timer < timeToMakeCamp)
        {
            // Check if C key is released
            if (!Input.GetKey(KeyCode.C))
            {
                isMakingCamp = false;
                ClearMessage();
                yield break;
            }

            timer += Time.deltaTime;

            // Calculate the remaining time
            float remainingTime = timeToMakeCamp - timer;

            // Update the message with the countdown
            ShowMessage($"Making a camp in {Mathf.CeilToInt(remainingTime)}");

            yield return null;
        }

        ShowMessage($"A new Camp has been set up!");

        // Make the camp after 3 seconds
        MakeCamp((int)transform.position.x, ((int)transform.position.y) + 0.5f);
        campAudio.PlayOneShot(campAudio.clip);
        GetComponent<Inventory>().campCount--;
        isMakingCamp = false;

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

    public void MakeCamp(float x, float y)
    {
        // Define the size of the area
        Vector2 areaSize = new Vector2(7.5f, 5.5f);

        // Center of the area, adjusted based on the camp position
        Vector2 center = new Vector2((int)transform.position.x, (int)transform.position.y);

        // Find all colliders within the specified box
        Collider2D[] colliders = Physics2D.OverlapBoxAll(center, areaSize, 0f);

        // Destroy all objects on a certain layer within the area
        foreach (Collider2D collider in colliders)
        {
            if (collider.gameObject.layer == LayerMask.NameToLayer("Ground"))
            {
                // Get the OreBehaviour component attached to the dirtBlock
                OreBehaviour oreBehaviour = collider.GetComponent<OreBehaviour>();

                // Check if the OreBehaviour component exists
                if (oreBehaviour != null)
                {
                    collider.GetComponent<OreBehaviour>().Drop();
                }
                Destroy(collider.gameObject);
            }
        }

        // Instantiate the camp
        Vector2 campPosition = new Vector2(x, y);
        Instantiate(camp, campPosition, Quaternion.identity);
    }
}