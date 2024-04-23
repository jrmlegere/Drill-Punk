using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    public GameObject inventoryUI;

    public Text coalText;
    public int coalCount = 0;
    public Text mushroomText;
    public int mushroomCount = 0;
    public Text plasticText;
    public int plasticCount = 0;
    public Text steelText;
    public int steelCount = 0;
    public Text woodText;
    public int woodCount = 0;
    public Text trinititeText;
    public int trinititeCount = 0;

    public Text steakText;
    public int steakCount = 0;
    public Text dogText;
    public int dogCount = 0;
    public Text honeyhamText;
    public int honeyhamCount = 0;
    public Text rationsText;
    public int rationsCount = 0;

    public Text steelbulletText;
    public int steelbulletCount = 20;
    public Text plasticbulletText;
    public int plasticbulletCount = 0;
    public Text bouncybulletText;
    public int bouncybulletCount = 0;
    public Text radbulletText;
    public int radbulletCount = 0;

    public Text campText;
    public int campCount = 0;
    public Text saplingText;
    public int saplingCount = 0;
    public Text radSuitText;
    public bool radSuitUnlocked = false;

    private void Update()
    {
        coalText.text = coalCount.ToString();
        mushroomText.text = mushroomCount.ToString();
        plasticText.text = plasticCount.ToString();
        steelText.text = steelCount.ToString();
        woodText.text = woodCount.ToString();
        trinititeText.text = trinititeCount.ToString();

        steakText.text = steakCount.ToString();
        dogText.text = dogCount.ToString();
        honeyhamText.text = honeyhamCount.ToString();
        rationsText.text = rationsCount.ToString();

        steelbulletText.text = steelbulletCount.ToString();
        plasticbulletText.text = plasticbulletCount.ToString();
        bouncybulletText.text = bouncybulletCount.ToString();
        radbulletText.text = radbulletCount.ToString();

        if (campCount > 0)
        {
            campText.text = "(" + campCount + ") Hold C to setup";
        }
        else
        {
            campText.text = "None Crafted";
        }

        if (saplingCount > 0)
        {
            saplingText.text = "(" + saplingCount +") Hold T to plant";
        }
        else
        {
            saplingText.text = "None Crafted";
        }

        if (radSuitUnlocked)
        {
            radSuitText.text = "Press R to Equip";
        }
        else
        {
            radSuitText.text = "Not Yet Crafted";
        }
    }
}
