using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DefaultBehaviour : MonoBehaviour
{
    public GameObject text1;
    public GameObject text2;
    public GameObject village;

    // Start is called before the first frame update
    void Start()
    {
        village = GameObject.FindGameObjectWithTag("Village");

        text1.GetComponent<TextMeshPro>().text = "Thank you for clearing the Lair! Meet me at the Village at (" + (int)village.transform.position.x + ", " + (int)village.transform.position.y + ")";
        text2.GetComponent<TextMeshPro>().text = "Thank you for clearing the Lair! Meet me at the Village at (" + (int)village.transform.position.x + ", " + (int)village.transform.position.y + ")";

        // Destroy the game object after 1 minute
        Destroy(gameObject, 60f);
    }
}