using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CaveGenerator : MonoBehaviour
{
    public List<Vector2> numberPairs = new List<Vector2>();
    public float minDistance = 5f;
    public GameObject lair;
    public int caveCount = 0;
    public GameObject village;

    void Start()
    {
        GenerateNumberPairs(5);
        LogNumberPairs();
    }

    void GenerateNumberPairs(int pairsToGenerate)
    {
        for (int i = 0; i < pairsToGenerate; i++)
        {
            Vector2 newPair = GenerateRandomPair(i);

            // Check distance with existing pairs
            while (!IsDistanceSuitable(newPair))
            {
                newPair = GenerateRandomPair(i);
            }

            numberPairs.Add(newPair);
        }
    }

    Vector2 GenerateRandomPair(int i)
    {
        float x = Random.Range(-147f, 147f);
        float y;

        if (i == 4)
        {
            y = Random.Range(-197f, -150f);
        }
        else
        {
            y = Random.Range(-197f, -20f);
        }

        return new Vector2(x, y);
    }

    bool IsDistanceSuitable(Vector2 newPair)
    {
        foreach (Vector2 existingPair in numberPairs)
        {
            float distance = Vector2.Distance(existingPair, newPair);
            if (distance < minDistance)
            {
                return false; // Too close, generate a new pair
            }
        }
        return true; // Suitable distance
    }

    void LogNumberPairs()
    {
        foreach (Vector2 pair in numberPairs)
        {
            caveCount++;
            if (caveCount <= 4)
            {
                GenerateLair((int)pair.x, (int)pair.y);
            }
            else
            {
                GenerateVillage((int)pair.x, (int)pair.y);
            }
        }
    }

    public void GenerateLair(int x, int y)
    {
        Vector2 lairPosition = new Vector2(x + 0.5f, y + 0.5f);
        GameObject newLair = Instantiate(lair, lairPosition, Quaternion.identity);
        newLair.GetComponent<LairBehaviour>().caveNumber = caveCount;
    }

    public void GenerateVillage(int x, int y)
    {
        Vector2 villagePosition = new Vector2(x + 0.5f, y + 0.5f);
        GameObject newVillage = Instantiate(village, villagePosition, Quaternion.identity);
    }
}