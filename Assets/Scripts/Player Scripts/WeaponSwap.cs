using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSwap : MonoBehaviour
{
    public GameObject drill;
    public GameObject handgun;

    // Update is called once per frame
    void Update()
    {
        if (!GetComponent<PlayerHealth>().dead)
        {
            if (!GetComponent<PlayerMovement>().isPaused)
            {
                // Check if the player is above y 505
                if (transform.position.y > 500f)
                {
                    // Player is above y 505, disable both drill and handgun
                    drill.SetActive(false);
                    handgun.SetActive(false);
                }
                else
                {
                    bool isRightMouseButtonDown = Input.GetMouseButton(1);
                    bool isLeftMouseButtonDown = Input.GetMouseButton(0);

                    // Check if the right mouse button is held down and the left mouse button is not held down
                    if (isRightMouseButtonDown)
                    {
                        if (!isLeftMouseButtonDown)
                            // Right mouse button is held down, set the object inactive
                            drill.SetActive(false);
                        handgun.SetActive(true);
                    }
                    else
                    {
                        // Right mouse button is not held down or left mouse button is held down, set the object active
                        drill.SetActive(true);
                        handgun.SetActive(false);
                    }
                }
            }
        }
        else
        {
            drill.SetActive(false);
            handgun.SetActive(false);
        }
    }
}