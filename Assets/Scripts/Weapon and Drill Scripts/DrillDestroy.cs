using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class DrillDestroy : MonoBehaviour
{
    public GameObject player;
    public LayerMask dirtLayer;
    public float radius = 1f;
    public float drillDuration = 0.5f; // Reduce duration for a 0.5-second timer

    public bool drillOn = false;
    public bool touchingDirt = false;

    public AudioSource drillAudio;
    public AudioSource drillOffAudio;

    // Keep track of the blocks being drilled
    public List<GameObject> blocksBeingDrilled = new List<GameObject>();

    void Update()
    {
        if (!player.GetComponent<PlayerMovement>().isPaused)
        {
            if (!player.GetComponent<PlayerMovement>().craftingShowing && !player.GetComponent<PlayerMovement>().inventoryShowing)
            {
                if (drillOn)
                {
                    if (!drillAudio.isPlaying)
                    {
                        drillAudio.Play();
                        drillAudio.loop = true;
                    }
                }
                else
                {
                    if (drillAudio.isPlaying)
                    {
                        drillAudio.Stop();

                        // Play the drill off audio once
                        if (drillOffAudio != null)
                        {
                            drillOffAudio.PlayOneShot(drillOffAudio.clip);
                        }
                    }
                }
            }
            else
            {
                if (drillAudio.isPlaying)
                {
                    drillAudio.Stop();

                    // Play the drill off audio once
                    if (drillOffAudio != null)
                    {
                        drillOffAudio.PlayOneShot(drillOffAudio.clip);
                    }
                }
            }
            
            Collider2D[] touchingDirtCheck = Physics2D.OverlapCircleAll(transform.position, radius, dirtLayer);

            if (touchingDirtCheck.Length != 0)
            {
                touchingDirt = true;
            }
            else
            {
                touchingDirt = false;
            }

            if (Input.GetMouseButtonDown(0))
            {
                StartDrilling();
            }

            if (Input.GetMouseButtonUp(0))
            {
                ClearDrillConditions();
                drillOn = false;
            }

            if (drillOn)
            {
                // Check for dirt blocks, calculate positions, and start timer
                DetectAndDestroyDirtBlocks();
            }
        }
    }

    void StartDrilling()
    {
        drillOn = true;
        blocksBeingDrilled.Clear(); // Clear the list when drilling starts
        // Other actions when drilling starts
    }

    void DetectAndDestroyDirtBlocks()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, radius, dirtLayer);

        foreach (Collider2D collider in colliders)
        {
            if (!blocksBeingDrilled.Contains(collider.gameObject))
            {
                collider.GetComponent<Renderer>().material.color = new Color(255, 0, 0);

                // Add the block to the list of blocks being drilled
                blocksBeingDrilled.Add(collider.gameObject);

                // Start timer
                StartCoroutine(DrillTimer(collider.gameObject));
            }
        }
    }

    IEnumerator DrillTimer(GameObject dirtBlock)
    {
        yield return new WaitForSeconds(drillDuration);

        Collider2D[] colliders2 = Physics2D.OverlapCircleAll(transform.position, radius, dirtLayer);

        if (dirtBlock != null && drillOn && Input.GetMouseButton(0))
        {
            // Check if the block is still in contact with the drill
            if (blocksBeingDrilled.Contains(dirtBlock) && IsDirtBlockInCollider(dirtBlock, colliders2))
            {
                // Get the OreBehaviour component attached to the dirtBlock
                OreBehaviour oreBehaviour = dirtBlock.GetComponent<OreBehaviour>();

                // Check if the OreBehaviour component exists
                if (oreBehaviour != null)
                {
                    dirtBlock.GetComponent<OreBehaviour>().Drop();
                }

                Destroy(dirtBlock);
                blocksBeingDrilled.Remove(dirtBlock); // Remove from the list after destroying
            }
            else
            {
                // Reset the color if the block was not in contact
                dirtBlock.GetComponent<Renderer>().material.color = Color.white;
                blocksBeingDrilled.Remove(dirtBlock);
            }
        }
        else if (dirtBlock != null)
        {
            // Reset the color if the mouse button was released
            dirtBlock.GetComponent<Renderer>().material.color = Color.white;
            blocksBeingDrilled.Remove(dirtBlock); // Remove from the list
        }
    }

    // Check if the drill conditions are met for all blocks being drilled
    void ClearDrillConditions()
    {
        foreach (GameObject block in blocksBeingDrilled)
        {
            if (block != null)
            {
                Renderer renderer = block.GetComponent<Renderer>();
                if (renderer != null)
                {
                    renderer.material.color = Color.white;
                }
            }
        }
        blocksBeingDrilled.Clear();
    }

    bool IsDirtBlockInCollider(GameObject dirtBlock, Collider2D[] colliders)
    {
        // Check if the dirt block's collider is present in the colliders array
        Collider2D dirtCollider = dirtBlock.GetComponent<Collider2D>();
        return dirtCollider != null && Array.Exists(colliders, collider => collider == dirtCollider);
    }
}