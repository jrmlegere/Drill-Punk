using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VillageBehaviour : MonoBehaviour
{
    public GameObject player;
    public GameObject logic;
    public bool playerNearby;
    public float seed;
    public int worldSizeX = 50;
    public int worldSizeY = 25;
    private GameObject[,] worldChunks;
    public int chunkSize = 10;
    public TileClass dirt;
    public int caveNumber = 5;
    public GameObject exit;
    public GameObject villageText;
    public Material defaultSpriteMaterial;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        logic = GameObject.FindGameObjectWithTag("Logic");
        MakeBarriers();
        GenerateLairTerrain();
    }

    void Update()
    {
        if ((Vector3.Distance(player.transform.position, transform.position) < 2f))
        {
            playerNearby = true;
            villageText.SetActive(playerNearby);
            GetComponent<SpriteRenderer>().color = Color.yellow;
        }
        else
        {
            playerNearby = false;
            villageText.SetActive(playerNearby);
            GetComponent<SpriteRenderer>().color = Color.white;
        }

        if (Input.GetKeyDown(KeyCode.W) && playerNearby)
        {
            player.transform.position = exit.transform.position;
            StartCoroutine(DelayedSetReady());
        }
    }

    IEnumerator DelayedSetReady()
    {
        yield return new WaitForSeconds(1f);
        exit.GetComponent<VillageExitBehaviour>().ready = true;
    }

    public void GenerateLairTerrain()
    {
        seed = Random.Range(-10000, 10000);
        CreateChunks();
        GenerateTerrain();
        GenerateExit();
    }

    public void GenerateNoiseTexture(float frequency, float limit, Texture2D noiseTexture)
    {
        for (int x = 0; x < noiseTexture.width; x++)
        {
            for (int y = 0; y < noiseTexture.height; y++)
            {
                float v = Mathf.PerlinNoise((x + seed) * frequency, (y + seed) * frequency);
                if (v > limit)
                    noiseTexture.SetPixel(x, y, Color.white);
                else
                    noiseTexture.SetPixel(x, y, Color.black);
            }
        }

        noiseTexture.Apply();
    }

    public void CreateChunks()
    {
        int numChunksX = worldSizeX / chunkSize;
        int numChunksY = worldSizeY / chunkSize;

        worldChunks = new GameObject[numChunksX, numChunksY];

        for (int i = 0; i < numChunksX; i++)
        {
            for (int j = 0; j < numChunksY; j++)
            {
                GameObject newChunk = new GameObject();
                newChunk.name = i.ToString() + ", " + j.ToString();
                newChunk.transform.parent = this.transform;
                worldChunks[i, j] = newChunk;
            }
        }
    }

    public void GenerateTerrain()
    {
        TileClass tile;
        for (int x = 0; x < worldSizeX; x++)
        {
            for (int y = 0; y < 4; y++)
            {
                // Set a default tile in case none of the conditions are met
                tile = dirt;
                PlaceTile(tile, x, y + (100 * caveNumber));
            }
        }
        // Define the size of the area
        Vector2 areaSize = new Vector2(2.5f, 2.5f);

        // Center of the area, adjusted based on the camp position
        Vector2 center = new Vector2(6.5f, (100 * caveNumber) + 5.5f);

        // Find all colliders within the specified box
        Collider2D[] colliders = Physics2D.OverlapBoxAll(center, areaSize, 0f);

        // Destroy all objects on a certain layer within the area
        foreach (Collider2D collider in colliders)
        {
            if (collider.gameObject.layer == LayerMask.NameToLayer("Ground"))
            {
                Destroy(collider.gameObject);
            }
        }
    }

    public void PlaceTile(TileClass tile, int x, int y)
    {
        GameObject newTile = new GameObject();

        float chunkCoordX = (Mathf.Round(x));
        chunkCoordX /= chunkSize;
        float chunkCoordY = (Mathf.Round(y));
        chunkCoordY /= chunkSize;

        int chunkIndexX = Mathf.Clamp((int)chunkCoordX, 0, worldChunks.GetLength(0) - 1);
        int chunkIndexY = Mathf.Clamp((int)chunkCoordY, 0, worldChunks.GetLength(1) - 1);

        newTile.transform.parent = worldChunks[chunkIndexX, chunkIndexY].transform;

        newTile.AddComponent<SpriteRenderer>();
        newTile.GetComponent<SpriteRenderer>().sprite = tile.tileSprite;
        newTile.transform.position = new Vector2(x + 0.5f, y + 0.5f);
        newTile.AddComponent<BoxCollider2D>();
        newTile.GetComponent<BoxCollider2D>().size = new Vector2(0.8f, 0.8f);
        newTile.GetComponent<BoxCollider2D>().edgeRadius = 0.1f;
        newTile.name = tile.tileName;
        newTile.layer = 6;

        newTile.GetComponent<SpriteRenderer>().material = defaultSpriteMaterial;

        if (tile.dropSprite != null)
        {
            OreBehaviour scriptComponent = newTile.AddComponent<OreBehaviour>();
            scriptComponent.tile = tile;
            scriptComponent.x = x + 0.5f;
            scriptComponent.y = y + 0.5f;
        }
    }

    public void MakeBarriers()
    {
        GameObject barrier1 = new GameObject();
        barrier1.AddComponent<SpriteRenderer>();
        barrier1.GetComponent<SpriteRenderer>().sprite = dirt.tileSprite;
        barrier1.transform.localScale = new Vector3(1f, worldSizeY, 1f);
        barrier1.transform.position = new Vector2(-0.5f, (100f * caveNumber) + (worldSizeY / 2f));
        barrier1.AddComponent<BoxCollider2D>();
        barrier1.GetComponent<BoxCollider2D>().size = new Vector2(1f, 1f);
        barrier1.name = "Barrier L";
        barrier1.layer = 15;

        GameObject barrier2 = new GameObject();
        barrier2.AddComponent<SpriteRenderer>();
        barrier2.GetComponent<SpriteRenderer>().sprite = dirt.tileSprite;
        barrier2.transform.localScale = new Vector3(1f, worldSizeY, 1f);
        barrier2.transform.position = new Vector2(worldSizeX + 0.5f, (100f * caveNumber) + (worldSizeY / 2f));
        barrier2.AddComponent<BoxCollider2D>();
        barrier2.GetComponent<BoxCollider2D>().size = new Vector2(1f, 1f);
        barrier2.name = "Barrier R";
        barrier2.layer = 15;

        GameObject barrier3 = new GameObject();
        barrier3.AddComponent<SpriteRenderer>();
        barrier3.GetComponent<SpriteRenderer>().sprite = dirt.tileSprite;
        barrier3.transform.localScale = new Vector3(worldSizeX + 2f, 1f, 1f);
        barrier3.transform.position = new Vector2(worldSizeX / 2f, (100f * caveNumber) - 0.5f);
        barrier3.AddComponent<BoxCollider2D>();
        barrier3.GetComponent<BoxCollider2D>().size = new Vector2(1f, 1f);
        barrier3.name = "Barrier D";
        barrier3.layer = 15;

        GameObject barrier4 = new GameObject();
        barrier4.AddComponent<SpriteRenderer>();
        barrier4.GetComponent<SpriteRenderer>().sprite = dirt.tileSprite;
        barrier4.transform.localScale = new Vector3(worldSizeX + 2f, 1f, 1f);
        barrier4.transform.position = new Vector2(worldSizeX / 2f, (100f * caveNumber) + worldSizeY + 0.5f);
        barrier4.AddComponent<BoxCollider2D>();
        barrier4.GetComponent<BoxCollider2D>().size = new Vector2(1f, 1f);
        barrier4.name = "Barrier U";
        barrier4.layer = 15;
    }

    public void GenerateExit()
    {
        Vector2 center = new Vector2(5.5f, (100 * caveNumber) + 5.5f);
        exit = Instantiate(exit, center, Quaternion.identity);
        exit.GetComponent<VillageExitBehaviour>().village = this.gameObject;
    }
}
