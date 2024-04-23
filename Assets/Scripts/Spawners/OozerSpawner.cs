using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OozerSpawner : MonoBehaviour
{
    public Transform player;
    public GameObject oozer;

    public float lastSpawn = 0f;
    public float spawnTime = 12f;

    // Update is called once per frame
    void Update()
    {
        if (player.position.y > 0 && player.position.y < 50)
        {
            if (Time.time - lastSpawn > spawnTime)
            {
                // Generate a random direction (-1 or 1)
                float direction = Random.Range(0, 2) == 0 ? -1f : 1f;

                // Instantiate the oozer with the random direction
                GameObject newOozer = Instantiate(oozer, new Vector2(player.position.x + (25f * direction), player.position.y + 5), Quaternion.identity);
                lastSpawn = Time.time;
            }
        }
    }
}