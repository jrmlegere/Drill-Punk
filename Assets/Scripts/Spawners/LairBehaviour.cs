using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class LairBehaviour : MonoBehaviour
{
    public bool open = true;
    public float closeTime = 12f;
    public GameObject player;
    public bool playerNearby;
    public int caveNumber;
    public GameObject logic;

    public Sprite openSprite;
    public Sprite closedSprite;

    public GameObject exit;

    public GameObject sign;
    public Sprite wadSign;
    public Sprite womSign;
    public Sprite bomSign;

    public int enemyType;
    public GameObject waddlepuss;
    public GameObject womster;
    public GameObject bomblebee;
    public int enemyCount;

    public int oreType;
    public TileClass dirt;
    public TileClass coal;
    public TileClass mushroom;
    public TileClass plastic;
    public TileClass steel;

    public GameObject enemy;
    public TileClass ore;

    public float lastSpawn = 0f;
    public float spawnTime = 120f;

    public float seed;
    public Texture2D caveNoiseTexture;
    public int worldSize = 50;
    public OreClass ores;
    public float caveFreq = 0.06f;
    public float surfaceValue = 0f;
    private GameObject[,] worldChunks;
    public int chunkSize = 10;

    public GameObject lairText;

    public GameObject moleman;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        logic = GameObject.FindGameObjectWithTag("Logic");
        MakeBarriers();

        GenerateLairType();
        GenerateLairTerrain();
    }

    void Update()
    {
        if ((Vector3.Distance(player.transform.position, transform.position) < 25f) && open)
        {
            if (Time.time - lastSpawn > spawnTime)
            {
                Instantiate(enemy, transform.position, Quaternion.identity);
                lastSpawn = Time.time;
            }
        }

        if ((Vector3.Distance(player.transform.position, transform.position) < 2f) && open)
        {
            playerNearby = true;
            lairText.SetActive(playerNearby);
            GetComponent<SpriteRenderer>().color = Color.yellow;
        }
        else
        {
            playerNearby = false;
            lairText.SetActive(playerNearby);
            GetComponent<SpriteRenderer>().color = Color.white;
        }

        if (Input.GetKeyDown(KeyCode.W) && playerNearby && open)
        {
            player.transform.position = exit.transform.position;
            StartCoroutine(DelayedSetReady());
            GenerateEnemies();
        }

        if (open)
        {
            GetComponent<SpriteRenderer>().sprite = openSprite;
        }
        else
        {
            GetComponent<SpriteRenderer>().sprite = closedSprite;
        }

        if ((enemyCount <= 0) && (open == true))
        {
            logic.GetComponent<UILogic>().score += 1000;
            open = false;
            StartCoroutine(OpenLairAfterDelay(closeTime * 60f));
            MakeMoleman();
        }
    }

    IEnumerator OpenLairAfterDelay(float delayInSeconds)
    {
        yield return new WaitForSeconds(delayInSeconds);
        ClearLair();
        GenerateLairType();
        GenerateTerrain();
        open = true;
        exit.GetComponent<LairExitBehaviour>().isMessageDisplayed = false;
    }

    IEnumerator DelayedSetReady()
    {
        yield return new WaitForSeconds(1f);
        exit.GetComponent<LairExitBehaviour>().ready = true;
    }

    public void GenerateLairType()
    {
        enemyType = Random.Range(1, 4);
        switch (enemyType)
        {
            case 1:     // Waddlepuss
                sign.GetComponent<SpriteRenderer>().sprite = wadSign;
                enemy = waddlepuss;
                break;
            case 2:     // Womster
                sign.GetComponent<SpriteRenderer>().sprite = womSign;
                enemy = womster;
                break;
            case 3:     // Bomblebee
                sign.GetComponent<SpriteRenderer>().sprite = bomSign;
                enemy = bomblebee;
                break;
        }
        oreType = Random.Range(1, 5);
        switch (oreType)
        {
            case 1:     // Coal
                ore = coal;
                break;
            case 2:     // Mushroom
                ore = mushroom;
                break;
            case 3:     // Plastic
                ore = plastic;
                break;
            case 4:     // Steel
                ore = steel;
                break;
        }
        enemyCount = Random.Range(3, 6);
    }

    public void GenerateLairTerrain()
    {
        seed = Random.Range(-10000, 10000);
        DrawTextures();
        CreateChunks();
        GenerateTerrain();
        GenerateExit();
    }

    public void DrawTextures()
    {
        caveNoiseTexture = new Texture2D(worldSize, worldSize);
        ores.spreadTexture = new Texture2D(worldSize, worldSize);

        GenerateNoiseTexture(caveFreq, surfaceValue, caveNoiseTexture);
        GenerateNoiseTexture(ores.rarity, ores.size, ores.spreadTexture);
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
        int numChunksX = worldSize / chunkSize;
        int numChunksY = worldSize / chunkSize;

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
        for (int x = 0; x < worldSize; x++)
        {
            for (int y = 0; y < worldSize; y++)
            {
                // Set a default tile in case none of the conditions are met
                tile = dirt;

                // Check if the ores array has elements
                if (ores != null && ores.spreadTexture != null)
                {
                    if (ores.spreadTexture.GetPixel(x, y).r > 0.5f)
                        tile = ore;
                }

                if (caveNoiseTexture.GetPixel(x, y).r > 0.5f)
                {
                    PlaceTile(tile, x, y + (100 * caveNumber));
                }
            }
        }
        // Define the size of the area
        Vector2 areaSize = new Vector2(2.5f, 2.5f);

        // Center of the area, adjusted based on the camp position
        Vector2 center = new Vector2(25.5f, (100 * caveNumber) + 25.5f);

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
        barrier1.transform.localScale = new Vector3(1f, worldSize, 1f);
        barrier1.transform.position = new Vector2(-0.5f, (100f*caveNumber) + (worldSize/2f));
        barrier1.AddComponent<BoxCollider2D>();
        barrier1.GetComponent<BoxCollider2D>().size = new Vector2(1f, 1f);
        barrier1.name = "Barrier L";
        barrier1.layer = 15;

        GameObject barrier2 = new GameObject();
        barrier2.AddComponent<SpriteRenderer>();
        barrier2.GetComponent<SpriteRenderer>().sprite = dirt.tileSprite;
        barrier2.transform.localScale = new Vector3(1f, worldSize, 1f);
        barrier2.transform.position = new Vector2(worldSize + 0.5f, (100f * caveNumber) + (worldSize / 2f));
        barrier2.AddComponent<BoxCollider2D>();
        barrier2.GetComponent<BoxCollider2D>().size = new Vector2(1f, 1f);
        barrier2.name = "Barrier R";
        barrier2.layer = 15;

        GameObject barrier3 = new GameObject();
        barrier3.AddComponent<SpriteRenderer>();
        barrier3.GetComponent<SpriteRenderer>().sprite = dirt.tileSprite;
        barrier3.transform.localScale = new Vector3(worldSize + 2f, 1f, 1f);
        barrier3.transform.position = new Vector2(worldSize / 2f, (100f * caveNumber) - 0.5f);
        barrier3.AddComponent<BoxCollider2D>();
        barrier3.GetComponent<BoxCollider2D>().size = new Vector2(1f, 1f);
        barrier3.name = "Barrier D";
        barrier3.layer = 15;

        GameObject barrier4 = new GameObject();
        barrier4.AddComponent<SpriteRenderer>();
        barrier4.GetComponent<SpriteRenderer>().sprite = dirt.tileSprite;
        barrier4.transform.localScale = new Vector3(worldSize + 2f, 1f, 1f);
        barrier4.transform.position = new Vector2(worldSize / 2f, (100f * caveNumber) + worldSize + 0.5f) ;
        barrier4.AddComponent<BoxCollider2D>();
        barrier4.GetComponent<BoxCollider2D>().size = new Vector2(1f, 1f);
        barrier4.name = "Barrier U";
        barrier4.layer = 15;
    }

    public void GenerateExit()
    {
        Vector2 center = new Vector2(25.5f, (100 * caveNumber) + 25.5f);
        exit = Instantiate(exit, center, Quaternion.identity);
        exit.GetComponent<LairExitBehaviour>().lair = this.gameObject;
    }

    public void GenerateEnemies()
    {
        for (int i = 0; i < enemyCount; i++)
        {
            float x = Random.Range(0f, 50f);
            float y = Random.Range(100f * caveNumber, (100f * caveNumber) + 50f);
            GameObject newEnemy = Instantiate(enemy, new Vector2(x, y), Quaternion.identity);
            newEnemy.AddComponent<LairEnemy>();
            newEnemy.GetComponent<LairEnemy>().home = this.gameObject;

        }
    }

    public void ClearLair()
    {
        // Define the size of the area
        Vector2 areaSize = new Vector2(60f, 60f);

        // Center of the area, adjusted based on the camp position
        Vector2 center = new Vector2(25.5f, (100 * caveNumber) + 25.5f);

        // Find all colliders within the specified box
        Collider2D[] colliders = Physics2D.OverlapBoxAll(center, areaSize, 0f);

        // Destroy all objects on a certain layer within the area
        foreach (Collider2D collider in colliders)
        {
            if (collider.gameObject.layer == LayerMask.NameToLayer("Ground"))
            {
                Destroy(collider.gameObject);
            }
            if (collider.gameObject.layer == LayerMask.NameToLayer("Drops"))
            {
                Destroy(collider.gameObject);
            }
        }
    }

    public void MakeMoleman()
    {
        // Define the size of the area
        Vector2 areaSize = new Vector2(7.5f, 5.5f);

        // Center of the area, adjusted based on the camp position
        Vector2 center = new Vector2(transform.position.x, transform.position.y + 1.5f);

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

        GameObject theMoleman = Instantiate(moleman, center, Quaternion.identity);
    }
}
