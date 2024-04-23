using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "StatsSprites", menuName = "Stats Sprites")]
public class StatsSprites : ScriptableObject
{
    public Sprite[] health = new Sprite[11]; // Health_0 to Health_10
    public Sprite[] hunger = new Sprite[4]; // Hunger_0 to Hunger_3
    public Sprite[] bHealth = new Sprite[4]; // BHealth_0 to BHealth_3
    public Sprite[] bHunger = new Sprite[2]; // BHunger_0 to BHunger_1
    public Sprite[] bEnergy = new Sprite[5]; // BEnergy_0 to BEnergy_4
    public Sprite[] bLove = new Sprite[5]; // BLove_0 to BLove_4
}