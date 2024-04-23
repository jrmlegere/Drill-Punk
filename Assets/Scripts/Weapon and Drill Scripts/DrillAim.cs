using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrillAim : MonoBehaviour
{
    public GameObject player; // The center of the circle
    public float radius = 1.8f;   // The radius of the circle

    // Update is called once per frame
    void Update()
    {
        if (!player.GetComponent<PlayerMovement>().isPaused)
        {
            // Calculate the position of the mouse in world space
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePos.z = 0f; // Ensure the same Z position as the player.transform

            // Calculate the direction from the player.transform to the mouse
            Vector3 dirToMouse = mousePos - player.transform.position;

            // Normalize the direction vector and multiply by the radius
            Vector3 targetPos = player.transform.position + dirToMouse.normalized * radius;

            // Set the position of the drill to the target position
            transform.position = targetPos;

            // Calculate the angle to aim at the mouse
            float angle = Mathf.Atan2(dirToMouse.y, dirToMouse.x) * Mathf.Rad2Deg;

            // Rotate the drill to aim at the mouse
            transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));

            // Flip the object vertically based on the position of the mouse
            if (mousePos.x < player.transform.position.x)
            {
                // If the mouse is on the left side, flip the object vertically
                transform.localScale = new Vector3(0.4f, -0.4f, 0.4f);
            }
            else
            {
                // If the mouse is on the right side, maintain the original object orientation
                transform.localScale = new Vector3(0.4f, 0.4f, 0.4f);
            }
        }
    }
}