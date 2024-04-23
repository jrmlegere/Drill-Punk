using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainGeneration : MonoBehaviour
{
    [Header("Tile Atals")]
    public float seed;
    public TileAtlas tileAtlas;

    public BiomeClass[] biomes;

    [Header("Biomes")]
    public float biomeFrequency;
    public Gradient biomeGradient;
    public Texture2D biomeMap;

    [Header("Generation Settings")]
    public int chunkSize = 16;
    public int worldSize = 100;
    public int heightAddition = 25;
    public bool generateCaves = true;
    public int dirtLayerHeight = 5;
    public float surfaceValue = 0.25f;
    public float heightMultiplier = 4f;
    public int SurfaceGeneration = 100;

    [Header("Noise Settings")]
    public float terrainFreq = 0.05f;
    public float caveFreq = 0.05f;
    public Texture2D caveNoiseTexture;

    [Header("Ore Settings")]
    public OreClass[] ores;

    private GameObject[,] worldChunks;
    private List<Vector2> worldTiles = new List<Vector2>();
    public BiomeClass curBiome;
    public GameObject player;
    public GameObject baby;
    public float chunkLoadDistance = 3f;
    private Vector2 previousPlayerChunk = Vector2.zero;

    public Material defaultSpriteMaterial;
    public Material specialSpriteMaterial;

    private void OnValidate()
    {
        DrawTextures();
    }

    private void Start()
    {
        seed = Random.Range(-10000, 10000);
        DrawTextures();
        CreateChunks();
        GenerateTerrain();
        player.GetComponent<CampMaker>().MakeCamp(0f, -24.5f);
        MakeBarriers();
    }

    public void DrawTextures()
    {
        biomeMap = new Texture2D(worldSize, worldSize);
        DrawBiomeTexture();

        caveNoiseTexture = new Texture2D(worldSize, worldSize);
        for (int o = 0; o < ores.Length; o++)
        {
            ores[o].spreadTexture = new Texture2D(worldSize, worldSize);
        }

        GenerateNoiseTexture(caveFreq, surfaceValue, caveNoiseTexture);
        for (int o = 0; o < ores.Length; o++)
        {
            GenerateNoiseTexture(ores[o].rarity, ores[o].size, ores[o].spreadTexture);
        }
    }

    public void DrawBiomeTexture()
    {
        for (int x = 0; x < biomeMap.width; x++)
        {
            for (int y = 0; y < biomeMap.height; y++)
            {
                float v = Mathf.PerlinNoise((x + seed) * biomeFrequency, (y + seed) * biomeFrequency);
                Color col = biomeGradient.Evaluate(v);
                biomeMap.SetPixel(x, y, col);
            }
        }
        biomeMap.Apply();
    }

    public void CreateChunks()
    {
        int numChunksX = worldSize / chunkSize;
        int numChunksY = heightAddition / chunkSize;

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

    public BiomeClass GetCurrentBiome(int x, int y)
    {
        for (int i = 0; i < biomes.Length; i++)
        {
            if (biomes[i].biomeCol == biomeMap.GetPixel(x, y))
            {
                return biomes[i];
            }
        }
        return curBiome;
    }

    public void GenerateTerrain()
    {
        TileClass tile;
        for (int x = 0; x < worldSize; x++)
        {
            float height = Mathf.PerlinNoise((x + seed) * terrainFreq, seed * terrainFreq) * heightMultiplier + heightAddition;

            for (int y = 0; y < height; y++)
            {
                if (y < height - dirtLayerHeight)
                {
                    if (y > height - 2 * dirtLayerHeight && height - dirtLayerHeight > y)
                    {
                        float randomValue = Random.Range(0f, 1f);

                        if (randomValue < 0.5f)
                        {
                            tile = GetCurrentBiome(x, y).tileAtlas.dirt;
                        }
                        else
                        {
                            int t = Random.Range(1, 101); // Generate a random number between 1 and 100
                            if (t == 1)
                            {
                                tile = tileAtlas.trinitite;
                            }
                            else
                            {
                                tile = curBiome.tileAtlas.badDirt;
                            }
                        }
                    }
                    else
                    {
                        tile = GetCurrentBiome(x, y).tileAtlas.dirt;

                        if (ores[0].spreadTexture.GetPixel(x, y).r > 0.5f && height - y > ores[0].maxSpawnHeight)
                            tile = tileAtlas.coal;
                        if (ores[1].spreadTexture.GetPixel(x, y).r > 0.5f && height - y > ores[1].maxSpawnHeight)
                            tile = tileAtlas.plastic;
                        if (ores[2].spreadTexture.GetPixel(x, y).r > 0.5f && height - y > ores[2].maxSpawnHeight)
                            tile = tileAtlas.mushroom;
                        if (ores[3].spreadTexture.GetPixel(x, y).r > 0.5f && height - y > ores[3].maxSpawnHeight)
                            tile = tileAtlas.steel;
                    }
                }
                else
                {
                    int t = Random.Range(1, 101); // Generate a random number between 1 and 100
                    if (t == 1)
                    {
                        tile = tileAtlas.trinitite;
                    }
                    else
                    {
                        tile = curBiome.tileAtlas.badDirt;
                    }
                }

                if (generateCaves)
                {
                    if (caveNoiseTexture.GetPixel(x, y).r > 0.5f)
                    {
                        PlaceTile(tile, x - (int)(worldSize / 2), y - heightAddition, false);
                    }
                }
                else
                {
                    PlaceTile(tile, x - (int)(worldSize / 2), y - heightAddition, false);
                }

                if (y >= height - 1)
                {
                    int s = Random.Range(1, 101); // Generate a random number between 1 and 100

                    // 8% chance of a tree spawn
                    if (s <= 8)
                    {
                        if (worldTiles.Contains(new Vector2(x - (int)(worldSize / 2), y - heightAddition)))
                        {
                            GenerateTree(x - (int)(worldSize / 2), y - heightAddition);
                        }
                    }
                    // 4% chance of a sign spawn
                    else if (s <= 12)
                    {
                        if (worldTiles.Contains(new Vector2(x - (int)(worldSize / 2), y - heightAddition)))
                        {
                            GenerateSign(x - (int)(worldSize / 2), y - heightAddition);
                        }
                    }
                    // 2% chance of a car spawn
                    else if (s <= 14)
                    {
                        if (worldTiles.Contains(new Vector2(x - (int)(worldSize / 2), y - heightAddition)))
                        {
                            GenerateCar(x - (int)(worldSize / 2), y - heightAddition);
                        }
                    }
                }
            }
        }
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

    public void PlaceTile(TileClass tile, int x, int y, bool rad)
    {
        if (!worldTiles.Contains(new Vector2Int(x, y)))
        {
            GameObject newTile = new GameObject();

            float chunkCoordX = (Mathf.Round((x + (int)(worldSize / 2)) / chunkSize) * chunkSize);
            chunkCoordX /= chunkSize;
            float chunkCoordY = (Mathf.Round((y + heightAddition) / chunkSize) * chunkSize);
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

            if (tile.tileName == "Bad Dirt")
            {
                BadDirtBehaviour scriptComponent = newTile.AddComponent<BadDirtBehaviour>();
                newTile.GetComponent<SpriteRenderer>().material = defaultSpriteMaterial;
            }

            if (rad)
            {
                newTile.GetComponent<SpriteRenderer>().material = defaultSpriteMaterial;
            }

            worldTiles.Add(newTile.transform.position - (Vector3.one * 0.5f));
        }
    }

    public void GenerateTree(int x, int y)
    {
        int treeHeight = 5; // Adjust this value based on the desired tree height

        for (int i = 0; i <= treeHeight; i++)
        {
            PlaceTile(tileAtlas.wood, x, y + i, true);
            if (i == treeHeight)
            {
                PlaceTile(tileAtlas.leaves, x, y + i + 1, true);
            }
        }
    }

    public void GenerateSign(int x, int y)
    {
        int signHeight = 2; // Adjust this value based on the desired tree height

        for (int i = 0; i <= signHeight; i++)
        {
            PlaceTile(tileAtlas.pole, x, y + i, true);
            if (i == signHeight)
            {
                PlaceTile(tileAtlas.sign, x, y + i + 1, true);
            }
        }
    }

    public void GenerateCar(int x, int y)
    {
        PlaceTile(tileAtlas.car_bottom, x - 1, y + 1, true);
        PlaceTile(tileAtlas.car_tire, x, y + 1, true);
        PlaceTile(tileAtlas.car_bottom, x + 1, y + 1, true);
        PlaceTile(tileAtlas.car_bottom, x + 2, y + 1, true);
        PlaceTile(tileAtlas.car_tire, x + 3, y + 1, true);
        PlaceTile(tileAtlas.car_bottom, x + 4, y + 1, true);

        PlaceTile(tileAtlas.car_front_flip, x - 1, y + 2, true);
        PlaceTile(tileAtlas.car_side, x, y + 2, true);
        PlaceTile(tileAtlas.car_side, x + 1, y + 2, true);
        PlaceTile(tileAtlas.car_side, x + 2, y + 2, true);
        PlaceTile(tileAtlas.car_side, x + 3, y + 2, true);
        PlaceTile(tileAtlas.car_front, x + 4, y + 2, true);

        PlaceTile(tileAtlas.car_top_flip, x, y + 3, true);
        PlaceTile(tileAtlas.car_top, x + 1, y + 3, true);
        PlaceTile(tileAtlas.car_top_flip, x + 2, y + 3, true);
        PlaceTile(tileAtlas.car_top, x + 3, y + 3, true);
    }

    public void MakeBarriers()
    {
        GameObject barrier1 = new GameObject();
        barrier1.AddComponent<SpriteRenderer>();
        barrier1.GetComponent<SpriteRenderer>().sprite = tileAtlas.rock.tileSprite;
        barrier1.transform.localScale = new Vector3(1f, heightAddition + 30f, 1f);
        barrier1.transform.position = new Vector2(-1f * ((worldSize / 2f) + 0.5f), (-1f * heightAddition) + ((heightAddition + 30f) / 2f));
        barrier1.AddComponent<BoxCollider2D>();
        barrier1.GetComponent<BoxCollider2D>().size = new Vector2(1f, 1f);
        barrier1.name = "Barrier L";
        barrier1.layer = 15;

        GameObject barrier2 = new GameObject();
        barrier2.AddComponent<SpriteRenderer>();
        barrier2.GetComponent<SpriteRenderer>().sprite = tileAtlas.rock.tileSprite;
        barrier2.transform.localScale = new Vector3(1f, heightAddition + 30f, 1f);
        barrier2.transform.position = new Vector2(((worldSize / 2f) + 0.5f), (-1f * heightAddition) + ((heightAddition + 30f) / 2f));
        barrier2.AddComponent<BoxCollider2D>();
        barrier2.GetComponent<BoxCollider2D>().size = new Vector2(1f, 1f);
        barrier2.name = "Barrier R";
        barrier2.layer = 15;

        GameObject barrier3 = new GameObject();
        barrier3.AddComponent<SpriteRenderer>();
        barrier3.GetComponent<SpriteRenderer>().sprite = tileAtlas.rock.tileSprite;
        barrier3.transform.localScale = new Vector3(worldSize + 2f, 1f, 1f);
        barrier3.transform.position = new Vector2(0f, (-1f * heightAddition) - 0.5f);
        barrier3.AddComponent<BoxCollider2D>();
        barrier3.GetComponent<BoxCollider2D>().size = new Vector2(1f, 1f);
        barrier3.name = "Barrier D";
        barrier3.layer = 15;

        GameObject barrier4 = new GameObject();
        barrier4.AddComponent<SpriteRenderer>();
        barrier4.GetComponent<SpriteRenderer>().sprite = tileAtlas.rock.tileSprite;
        barrier4.transform.localScale = new Vector3(worldSize + 2f, 1f, 1f);
        barrier4.transform.position = new Vector2(0f, (2f * heightMultiplier) + 0.5f);
        barrier4.AddComponent<BoxCollider2D>();
        barrier4.GetComponent<BoxCollider2D>().size = new Vector2(1f, 1f);
        barrier4.name = "Barrier U";
        barrier4.layer = 15;
    }
}