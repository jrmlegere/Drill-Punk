using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BadDirtBehaviour : MonoBehaviour
{
    public LayerMask playerLayer;
    public LayerMask babyLayer;

    void Start()
    {
        playerLayer = LayerMask.GetMask("Player");
        babyLayer = LayerMask.GetMask("Baby");
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        if ((playerLayer.value & 1 << collision.gameObject.layer) != 0)
        {
            PlayerHealth playerHealth = collision.gameObject.GetComponent<PlayerHealth>();

            if (playerHealth != null)
            {
                playerHealth.ApplyPoison();
            }
        }

        if ((babyLayer.value & 1 << collision.gameObject.layer) != 0)
        {
            BabyHealth babyHealth = collision.gameObject.GetComponent<BabyHealth>();

            if (babyHealth != null)
            {
                babyHealth.ApplyPoison();
            }
        }

    }
}
