using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpscreamSpawner : MonoBehaviour
{
    public Transform player;
    public GameObject jumpscream;

    public float lastSpawn = 0f;
    public float spawnTime = 120f;

    // Update is called once per frame
    void Update()
    {
        if (player.position.y < 0)
        {
            if (Time.time - lastSpawn > spawnTime)
            {
                // 20 percent chance of a variable being true
                if (Random.value <= 0.1f)
                {
                    // Generate random directions for X and Y
                    float directionX = Random.Range(-1f, 1f);
                    float directionY = Random.Range(-1f, 1f);

                    // Instantiate the jumpscream with the random direction
                    GameObject newJumpscream = Instantiate(jumpscream, new Vector2(player.position.x + (25f * directionX), player.position.y + (25f * directionY)), Quaternion.identity);
                    lastSpawn = Time.time;
                }
                else
                {
                    lastSpawn = Time.time;
                }
            }
        }
    }
}