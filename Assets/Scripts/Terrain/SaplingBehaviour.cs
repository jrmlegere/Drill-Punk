using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaplingBehaviour : MonoBehaviour
{
    public Material defaultSpriteMaterial;
    public TileAtlas tileAtlas;

    public int x;
    public int y;

    public float plantTime;
    public float growTime;

    // Start is called before the first frame update
    void Start()
    {
        plantTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time - plantTime >= growTime)
        {
            GenerateTree(x, y - 1);
            Destroy(gameObject);
        }
    }

    public void PlaceTile(TileClass tile, int x, int y, bool rad)
    {
        GameObject newTile = new GameObject();

        newTile.AddComponent<SpriteRenderer>();
        newTile.GetComponent<SpriteRenderer>().sprite = tile.tileSprite;
        newTile.transform.position = new Vector2(x + 0.5f, y + 0.5f);
        newTile.AddComponent<BoxCollider2D>();
        newTile.GetComponent<BoxCollider2D>().size = new Vector2(0.8f, 0.8f);
        newTile.GetComponent<BoxCollider2D>().edgeRadius = 0.1f;
        newTile.name = tile.tileName;
        newTile.layer = 6;

        if (tile.dropSprite != null)
        {
            OreBehaviour scriptComponent = newTile.AddComponent<OreBehaviour>();
            scriptComponent.tile = tile;
            scriptComponent.x = x + 0.5f;
            scriptComponent.y = y + 0.5f;
        }

        if (rad)
        {
            newTile.GetComponent<SpriteRenderer>().material = defaultSpriteMaterial;
        }
    }

    public void GenerateTree(int x, int y)
    {
        int treeHeight = 5; // Adjust this value based on the desired tree height

        for (int i = 0; i <= treeHeight; i++)
        {
            PlaceTile(tileAtlas.wood, x, y + i - 1, true);
            if (i == treeHeight)
            {
                PlaceTile(tileAtlas.leaves, x, y + i, true);
            }
        }
    }
}
