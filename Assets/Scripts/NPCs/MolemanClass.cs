using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MolemanClass : MonoBehaviour
{
    public GameObject player;
    public bool isFacingRight = false;

    public Sprite icon;
    public string fName;
    public string lName;
    public string job;
    public string quote;
    public int id;

    public void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    public void Update()
    {
        if (player.transform.position.x < transform.position.x)
        {
            if (isFacingRight)
            {
                Flip();
            }
        }
        else
        {
            if (!isFacingRight)
            {
                Flip();
            }
        }
    }

    private void Flip()
    {
        Vector3 localScale = transform.localScale;
        localScale.x *= -1f;
        transform.localScale = localScale;
        isFacingRight = !isFacingRight;
    }
}
