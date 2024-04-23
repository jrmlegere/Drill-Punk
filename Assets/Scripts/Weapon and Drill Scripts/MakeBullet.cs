using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class MakeBullet : MonoBehaviour
{
    public GameObject player;

    public Sprite bulletSprite;
    public float bulletSpeed = 50f;
    public float bulletTimer = 1f;
    public float bulletColliderRadius = 0.3f; // Adjust the radius as needed

    public AudioSource steelSound;
    public AudioSource plasticSound;
    public AudioSource bouncySound;
    public AudioSource radSound;

    public AudioSource emptySound;

    public bool hasAmmo = true;

    void Update()
    {
        if (!player.GetComponent<PlayerMovement>().isPaused)
        {
            switch (player.GetComponent<AmmoSwap>().bulletType)
            {
                case 1:
                    if (player.GetComponent<Inventory>().steelbulletCount > 0)
                    {
                        hasAmmo = true;
                    }
                    else
                    {
                        hasAmmo = false;
                    }
                    break;
                case 2:
                    if (player.GetComponent<Inventory>().plasticbulletCount > 0)
                    {
                        hasAmmo = true;
                    }
                    else
                    {
                        hasAmmo = false;
                    }
                    break;
                case 3:
                    if (player.GetComponent<Inventory>().bouncybulletCount > 0)
                    {
                        hasAmmo = true;
                    }
                    else
                    {
                        hasAmmo = false;
                    }
                    break;
                case 4:
                    if (player.GetComponent<Inventory>().radbulletCount > 0)
                    {
                        hasAmmo = true;
                    }
                    else
                    {
                        hasAmmo = false;
                    }
                    break;
                default:
                    break;
            }

            if (Input.GetMouseButtonDown(0) && hasAmmo)
            {
                SpawnBullet(player.GetComponent<AmmoSwap>().bulletType);
            }
            else if (Input.GetMouseButtonDown(0) && !hasAmmo)
            {
                emptySound.PlayOneShot(emptySound.clip);
            }
        }
    }

    void SpawnBullet(int type)
    {
        // Create a new GameObject for the bullet
        GameObject bullet = new GameObject("Bullet");

        // Set the layer of the bullet
        bullet.layer = LayerMask.NameToLayer("Bullet");

        // Add a SpriteRenderer component to the bullet GameObject
        SpriteRenderer bulletRenderer = bullet.AddComponent<SpriteRenderer>();

        // Set the sprite for the bullet
        bulletRenderer.sprite = bulletSprite;

        // Set the sorting order of the bullet
        bulletRenderer.sortingOrder = -1;

        switch (type)
        {
            case 1:
                bulletRenderer.color = new Color(255f / 255f, 255f / 255f, 255f / 255f);
                steelSound.PlayOneShot(steelSound.clip);
                player.GetComponent<Inventory>().steelbulletCount--;
                break;
            case 2:
                bulletRenderer.color = new Color(255f / 255f, 1f / 255f, 145f / 255f);
                plasticSound.PlayOneShot(plasticSound.clip);
                player.GetComponent<Inventory>().plasticbulletCount--;
                break;
            case 3:
                bulletRenderer.color = new Color(255f / 255f, 161f / 255f, 1f / 255f);
                bouncySound.PlayOneShot(bouncySound.clip);
                player.GetComponent<Inventory>().bouncybulletCount--;
                break;
            case 4:
                bulletRenderer.color = new Color(112f / 255f, 255f / 255f, 1f / 255f);
                radSound.PlayOneShot(radSound.clip);
                player.GetComponent<Inventory>().radbulletCount--;
                break;
            default:
                break;
        }

        bullet.AddComponent<BulletBreak>();
        bullet.GetComponent<BulletBreak>().type = type;

        // Set the position of the bullet to match the position of the script's GameObject
        bullet.transform.position = player.transform.position;

        // Adjust the Bullet size
        bullet.transform.localScale = new Vector3(0.3f, 0.3f, 0.3f);

        // Add a 2D Circle Collider to the bullet GameObject
        CircleCollider2D bulletCollider = bullet.AddComponent<CircleCollider2D>();
        bulletCollider.radius = bulletColliderRadius;

        // Add a Rigidbody2D component to the bullet GameObject for physics simulation
        Rigidbody2D bulletRb = bullet.AddComponent<Rigidbody2D>();

        // Set the gravity scale to zero to disable gravity
        bulletRb.gravityScale = 0f;

        // Calculate the direction towards the mouse
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        float directionX = (mousePosition.x - player.transform.position.x);
        float directionY = (mousePosition.y - player.transform.position.y);
        Vector2 direction = new Vector2(directionX, directionY).normalized;

        // Set the velocity of the bullet based on the calculated direction and speed
        bulletRb.velocity = direction * bulletSpeed;

        // Destroy the bullet after a certain lifetime
        Destroy(bullet, bulletTimer);
    }
}