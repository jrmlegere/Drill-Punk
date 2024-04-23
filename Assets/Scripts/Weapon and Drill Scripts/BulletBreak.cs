using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBreak : MonoBehaviour
{
    public int type;
    public float damage;

    public void Start()
    {
        switch (type)
        {
            case 1:
                damage = 1f;
                break;
            case 2:
                damage = 0.75f;
                break;
            case 3:
                damage = 1f;
                break;
            case 4:
                damage = 1f;
                break;
            default:
                break;
        }
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        if (type != 3)
        {
            Destroy(gameObject);
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (type == 3)
        {
            // Get the normal vector of the collision point
            Vector2 collisionNormal = collision.contacts[0].normal;

            // Reflect the velocity of the object based on the collision normal
            Vector2 reflectedVelocity = Vector2.Reflect(GetComponent<Rigidbody2D>().velocity, collisionNormal);

            // Apply the reflected velocity with additional force for bouncing effect
            GetComponent<Rigidbody2D>().velocity = reflectedVelocity.normalized * 5f;
        }
    }
}