using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class VillageExitBehaviour : MonoBehaviour
{
    public GameObject village;
    public GameObject player;
    public bool playerNearby;
    public bool ready = false;
    public bool isMessageDisplayed = false;

    // Start is called before the first frame update
    void Start()
    {
        ready = false;
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        if ((Vector3.Distance(player.transform.position, transform.position) < 2f))
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
            player.transform.position = village.transform.position;
            ready = false;
        }
    }
}