using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CampText : MonoBehaviour
{
    public GameObject player;
    public GameObject text;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        text.SetActive(player.GetComponent<PlayerMovement>().isAtCamp);
    }
}
