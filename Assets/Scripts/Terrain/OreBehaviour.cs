using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OreBehaviour : MonoBehaviour
{
    public TileClass tile;
    public float x;
    public float y;

    public void Drop()
    {
        if (tile != null && tile.dropSprite != null)
        {
            GameObject drop = Instantiate(tile.dropSprite, new Vector2(x, y), Quaternion.identity);
            drop.layer = 11;
            drop.tag = "Drop";

            // Destroy the drop object after 60 seconds
            Destroy(drop, 60f);
        }
    }
}