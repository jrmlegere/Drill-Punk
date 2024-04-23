using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBehaviour : MonoBehaviour
{
    public GameObject player;

    void Start()
    {
        // Set initial position
        transform.position = new Vector3(player.transform.position.x, player.transform.position.y, -10f);
    }

    void Update()
    {
        if (player.transform.position.y > 500f)
        {
            if (player.transform.position.y > 543f)
            {
                if (player.transform.position.x < 12.5f)
                {
                    transform.position = new Vector3(12.5f, 543f, -10f);
                }
                else if (player.transform.position.x > 37.5f)
                {
                    transform.position = new Vector3(37.5f, 543f, -10f);
                }
                else
                {
                    transform.position = new Vector3(player.transform.position.x, 543f, -10f);
                }
            }
            else if (player.transform.position.y < 509f)
            {
                if (player.transform.position.x < 12.5f)
                {
                    transform.position = new Vector3(12.5f, 509f, -10f);
                }
                else if (player.transform.position.x > 37.5f)
                {
                    transform.position = new Vector3(37.5f, 509f, -10f);
                }
                else
                {
                    transform.position = new Vector3(player.transform.position.x, 509f, -10f);
                }
            }
            else if ((player.transform.position.x < 12.5f) && (player.transform.position.y <= 543f) && (player.transform.position.y >= 509f))
            {
                transform.position = new Vector3(12.5f, player.transform.position.y, -10f);
            }
            else if ((player.transform.position.x > 37.5f) && (player.transform.position.y <= 543f) && (player.transform.position.y >= 509f))
            {
                transform.position = new Vector3(37.5f, player.transform.position.y, -10f);
            }
            else
            {
                transform.position = new Vector3(player.transform.position.x, player.transform.position.y, -10f);
            }
        }

        else if (player.transform.position.y > 400f)
        {
            if (player.transform.position.y > 443f)
            {
                if (player.transform.position.x < 12.5f)
                {
                    transform.position = new Vector3(12.5f, 443f, -10f);
                }
                else if (player.transform.position.x > 37.5f)
                {
                    transform.position = new Vector3(37.5f, 443f, -10f);
                }
                else
                {
                    transform.position = new Vector3(player.transform.position.x, 443f, -10f);
                }
            }
            else if (player.transform.position.y < 407f)
            {
                if (player.transform.position.x < 12.5f)
                {
                    transform.position = new Vector3(12.5f, 407f, -10f);
                }
                else if (player.transform.position.x > 37.5f)
                {
                    transform.position = new Vector3(37.5f, 407f, -10f);
                }
                else
                {
                    transform.position = new Vector3(player.transform.position.x, 407f, -10f);
                }
            }
            else if ((player.transform.position.x < 12.5f) && (player.transform.position.y <= 443f) && (player.transform.position.y >= 407f))
            {
                transform.position = new Vector3(12.5f, player.transform.position.y, -10f);
            }
            else if ((player.transform.position.x > 37.5f) && (player.transform.position.y <= 443f) && (player.transform.position.y >= 407f))
            {
                transform.position = new Vector3(37.5f, player.transform.position.y, -10f);
            }
            else
            {
                transform.position = new Vector3(player.transform.position.x, player.transform.position.y, -10f);
            }
        }

        else if (player.transform.position.y > 300f)
        {
            if (player.transform.position.y > 343f)
            {
                if (player.transform.position.x < 12.5f)
                {
                    transform.position = new Vector3(12.5f, 343f, -10f);
                }
                else if (player.transform.position.x > 37.5f)
                {
                    transform.position = new Vector3(37.5f, 343f, -10f);
                }
                else
                {
                    transform.position = new Vector3(player.transform.position.x, 343f, -10f);
                }
            }
            else if (player.transform.position.y < 307f)
            {
                if (player.transform.position.x < 12.5f)
                {
                    transform.position = new Vector3(12.5f, 307f, -10f);
                }
                else if (player.transform.position.x > 37.5f)
                {
                    transform.position = new Vector3(37.5f, 307f, -10f);
                }
                else
                {
                    transform.position = new Vector3(player.transform.position.x, 307f, -10f);
                }
            }
            else if ((player.transform.position.x < 12.5f) && (player.transform.position.y <= 343f) && (player.transform.position.y >= 307f))
            {
                transform.position = new Vector3(12.5f, player.transform.position.y, -10f);
            }
            else if ((player.transform.position.x > 37.5f) && (player.transform.position.y <= 343f) && (player.transform.position.y >= 307f))
            {
                transform.position = new Vector3(37.5f, player.transform.position.y, -10f);
            }
            else
            {
                transform.position = new Vector3(player.transform.position.x, player.transform.position.y, -10f);
            }
        }

        else if (player.transform.position.y > 200f)
        {
            if (player.transform.position.y > 243f)
            {
                if (player.transform.position.x < 12.5f)
                {
                    transform.position = new Vector3(12.5f, 243f, -10f);
                }
                else if (player.transform.position.x > 37.5f)
                {
                    transform.position = new Vector3(37.5f, 243f, -10f);
                }
                else
                {
                    transform.position = new Vector3(player.transform.position.x, 243f, -10f);
                }
            }
            else if (player.transform.position.y < 207f)
            {
                if (player.transform.position.x < 12.5f)
                {
                    transform.position = new Vector3(12.5f, 207f, -10f);
                }
                else if (player.transform.position.x > 37.5f)
                {
                    transform.position = new Vector3(37.5f, 207f, -10f);
                }
                else
                {
                    transform.position = new Vector3(player.transform.position.x, 207f, -10f);
                }
            }
            else if ((player.transform.position.x < 12.5f) && (player.transform.position.y <= 243f) && (player.transform.position.y >= 207f))
            {
                transform.position = new Vector3(12.5f, player.transform.position.y, -10f);
            }
            else if ((player.transform.position.x > 37.5f) && (player.transform.position.y <= 243f) && (player.transform.position.y >= 207f))
            {
                transform.position = new Vector3(37.5f, player.transform.position.y, -10f);
            }
            else
            {
                transform.position = new Vector3(player.transform.position.x, player.transform.position.y, -10f);
            }
        }

        else if (player.transform.position.y > 100f)
        {
            if (player.transform.position.y > 143f)
            {
                if (player.transform.position.x < 12.5f)
                {
                    transform.position = new Vector3(12.5f, 143f, -10f);
                }
                else if (player.transform.position.x > 37.5f)
                {
                    transform.position = new Vector3(37.5f, 143f, -10f);
                }
                else
                {
                    transform.position = new Vector3(player.transform.position.x, 143f, -10f);
                }
            }
            else if (player.transform.position.y < 107f)
            {
                if (player.transform.position.x < 12.5f)
                {
                    transform.position = new Vector3(12.5f, 107f, -10f);
                }
                else if (player.transform.position.x > 37.5f)
                {
                    transform.position = new Vector3(37.5f, 107f, -10f);
                }
                else
                {
                    transform.position = new Vector3(player.transform.position.x, 107f, -10f);
                }
            }
            else if ((player.transform.position.x < 12.5f) && (player.transform.position.y <= 143f) && (player.transform.position.y >= 107f))
            {
                transform.position = new Vector3(12.5f, player.transform.position.y, -10f);
            }
            else if ((player.transform.position.x > 37.5f) && (player.transform.position.y <= 143f) && (player.transform.position.y >= 107f))
            {
                transform.position = new Vector3(37.5f, player.transform.position.y, -10f);
            }
            else
            {
                transform.position = new Vector3(player.transform.position.x, player.transform.position.y, -10f);
            }
        }

        else
        {
            if ((player.transform.position.x < -137.5f || player.transform.position.x > 137.5f) && (player.transform.position.y < -193f))
            {
                transform.position = new Vector3(137.5f * (player.transform.position.x / Mathf.Abs(player.transform.position.x)), -193f, -10f);
            }
            else if (player.transform.position.x < -137.5f || player.transform.position.x > 137.5f)
            {
                transform.position = new Vector3(137.5f * (player.transform.position.x / Mathf.Abs(player.transform.position.x)), player.transform.position.y, -10f);
            }
            else if (player.transform.position.y < -193f)
            {
                transform.position = new Vector3(player.transform.position.x, -193f, -10f);
            }
            else
            {
                transform.position = new Vector3(player.transform.position.x, player.transform.position.y, -10f);
            }
        }
    }
}