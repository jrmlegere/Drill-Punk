using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crafting : MonoBehaviour
{
    public GameObject craftingUI;

    public int coalCount;
    public int mushroomCount;
    public int plasticCount;
    public int steelCount;
    public int woodCount;
    public int trinititeCount;

    public AudioSource craftAudio;
    public AudioSource errorAudio;

    // Update is called once per frame
    void Update()
    {
        coalCount = GetComponent<Inventory>().coalCount;
        mushroomCount = GetComponent<Inventory>().mushroomCount;
        plasticCount = GetComponent<Inventory>().plasticCount;
        steelCount = GetComponent<Inventory>().steelCount;
        woodCount = GetComponent<Inventory>().woodCount;
        trinititeCount = GetComponent<Inventory>().trinititeCount;
    }

    public void CraftSteelBullet()
    {
        if (steelCount >= 5 && coalCount >= 1 && (GetComponent<Inventory>().steelbulletCount + 5) < 1000)
        {
            craftAudio.PlayOneShot(craftAudio.clip);
            GetComponent<Inventory>().steelCount = steelCount - 5;
            GetComponent<Inventory>().coalCount = coalCount - 1;
            GetComponent<Inventory>().steelbulletCount = GetComponent<Inventory>().steelbulletCount + 5;
        }
        else
        {
            errorAudio.PlayOneShot(errorAudio.clip);
        }
    }

    public void CraftPlasticBullet()
    {
        if (plasticCount >= 5 && coalCount >= 1 && (GetComponent<Inventory>().plasticbulletCount + 5) < 1000)
        {
            craftAudio.PlayOneShot(craftAudio.clip);
            GetComponent<Inventory>().plasticCount = plasticCount - 5;
            GetComponent<Inventory>().coalCount = coalCount - 1;
            GetComponent<Inventory>().plasticbulletCount = GetComponent<Inventory>().plasticbulletCount + 5;
        }
        else
        {
            errorAudio.PlayOneShot(errorAudio.clip);
        }
    }

    public void CraftBouncyBullet()
    {
        if (mushroomCount >= 5 && coalCount >= 1 && (GetComponent<Inventory>().bouncybulletCount + 5) < 1000)
        {
            craftAudio.PlayOneShot(craftAudio.clip);
            GetComponent<Inventory>().mushroomCount = mushroomCount - 5;
            GetComponent<Inventory>().coalCount = coalCount - 1;
            GetComponent<Inventory>().bouncybulletCount = GetComponent<Inventory>().bouncybulletCount + 5;
        }
        else
        {
            errorAudio.PlayOneShot(errorAudio.clip);
        }
    }

    public void CraftRadBullet()
    {
        if (trinititeCount >= 1 && coalCount >= 1 && (GetComponent<Inventory>().radbulletCount + 1) < 1000)
        {
            craftAudio.PlayOneShot(craftAudio.clip);
            GetComponent<Inventory>().trinititeCount = trinititeCount - 1;
            GetComponent<Inventory>().coalCount = coalCount - 1;
            GetComponent<Inventory>().radbulletCount++;
        }
        else
        {
            errorAudio.PlayOneShot(errorAudio.clip);
        }
    }

    public void CraftCamp()
    {
        if (plasticCount >= 50 && coalCount >= 50 && steelCount >= 50 && mushroomCount >= 50 && (GetComponent<Inventory>().campCount + 1) < 1000)
        {
            craftAudio.PlayOneShot(craftAudio.clip);
            GetComponent<Inventory>().steelCount = steelCount - 50;
            GetComponent<Inventory>().coalCount = coalCount - 50;
            GetComponent<Inventory>().plasticCount = plasticCount - 50;
            GetComponent<Inventory>().mushroomCount = mushroomCount - 50;
            GetComponent<Inventory>().campCount++;
        }
        else
        {
            errorAudio.PlayOneShot(errorAudio.clip);
        }
    }

    public void CraftRadSuit()
    {
        if (plasticCount >= 99 && coalCount >= 10 && !GetComponent<Inventory>().radSuitUnlocked)
        {
            craftAudio.PlayOneShot(craftAudio.clip);
            GetComponent<Inventory>().plasticCount = plasticCount - 99;
            GetComponent<Inventory>().coalCount = coalCount - 10;
            GetComponent<Inventory>().radSuitUnlocked = true;
        }
        else
        {
            errorAudio.PlayOneShot(errorAudio.clip);
        }
    }

    public void CraftSapling()
    {
        if (woodCount >= 1 && mushroomCount >= 10 && coalCount >= 10 && (GetComponent<Inventory>().saplingCount + 1) < 1000)
        {
            craftAudio.PlayOneShot(craftAudio.clip);
            GetComponent<Inventory>().woodCount = woodCount - 1;
            GetComponent<Inventory>().mushroomCount = mushroomCount - 10;
            GetComponent<Inventory>().coalCount = coalCount - 10;
            GetComponent<Inventory>().saplingCount++;
        }
        else
        {
            errorAudio.PlayOneShot(errorAudio.clip);
        }
    }
}
