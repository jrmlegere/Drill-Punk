using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class LairExitBehaviour : MonoBehaviour
{
    public GameObject lair;
    public GameObject player;
    public bool playerNearby;
    public bool ready = false;
    public Text updates;
    public bool isMessageDisplayed = false;

    // Start is called before the first frame update
    void Start()
    {
        ready = false;
        player = GameObject.FindGameObjectWithTag("Player");
        updates = GameObject.FindGameObjectWithTag("Updates").GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!lair.GetComponent<LairBehaviour>().open && !isMessageDisplayed)
        {
            StartCoroutine(DisplayMessage("Lair cleared! Press 'W' to leave", 3f));
            isMessageDisplayed = true;
        }

        if ((Vector3.Distance(player.transform.position, transform.position) < 2f) || !lair.GetComponent<LairBehaviour>().open)
        {
            playerNearby = true;
            GetComponent<SpriteRenderer>().color = Color.yellow;
        }
        else
        {
            playerNearby = false;
            GetComponent<SpriteRenderer>().color = Color.white;
        }

        if (Input.GetKeyDown(KeyCode.W) && playerNearby && ready)
        {
            updates.text = "";
            player.transform.position = lair.transform.position;
            ready = false;
        }
    }

    IEnumerator DisplayMessage(string message, float duration)
    {
        updates.text = message;
        yield return new WaitForSeconds(duration);
        updates.text = "";
    }
}