using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MolemanSpawner : MonoBehaviour
{
    public GameObject[] molemen;
    public List<int> currentMoles = new List<int>();

    public GameObject quest;
    public GameObject[] quests;
    public string[] questTags = { "Quest 0", "Quest 1", "Quest 2", "Quest 3", "Quest 4" }; // Tags for your Canvas objects

    // Start is called before the first frame update
    void Start()
    {
        quests = new GameObject[5];
        GenerateVillagers(5);
        SetUpQuests(5);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GenerateVillagers(int molesToGenerate)
    {
        for (int i = 0; i < molesToGenerate; i++)
        {
            int newMole = Random.Range(0, molemen.Length);
            while (currentMoles.Contains(newMole))
            {
                newMole = Random.Range(0, molemen.Length);
            }
            currentMoles.Add(newMole);
            Instantiate(molemen[newMole], new Vector2(13f + (8f*i), 507f), Quaternion.identity);
        }
    }

    public void SetUpQuests(int questNum)
    {
        for (int i = 0; i < questNum; i++)
        {
            quests[i] = Instantiate(quest, new Vector2(13f + (8f * i), 506.5f), Quaternion.identity);

            // Find the Canvas object dynamically based on tag
            GameObject questScreen = GameObject.FindGameObjectWithTag(questTags[i]);

            quests[i].GetComponent<Quest>().moleman = molemen[currentMoles[i]];
            quests[i].GetComponent<Quest>().questNum = i;
            quests[i].GetComponent<Quest>().questScreen = questScreen;
            questScreen.SetActive(false);
        }
    }
}
