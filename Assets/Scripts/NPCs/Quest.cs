using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Quest : MonoBehaviour
{
    public GameObject player;
    public GameObject logic;

    public int questNum;
    public GameObject moleman;
    public MolemanClass molemanInfo;

    public GameObject questScreen;
    public Text title;
    public Text quote;
    public Text request;
    public Image icon;
    public Image image;

    public int itemType;
    public string itemTitle;
    public int itemQuantity;
    public Sprite itemSprite;

    public Sprite coal;
    public Sprite mushroom;
    public Sprite plastic;
    public Sprite steel;
    public Sprite wood;
    public Sprite trinitite;
    public Sprite steak;
    public Sprite dog;
    public Sprite honeyham;
    public Sprite rations;

    public Button button;

    public AudioSource tradeAudio;
    public AudioSource errorAudio;

    public GameObject text1;

    public 

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        logic = GameObject.FindGameObjectWithTag("Logic");

        Transform panelTransform = questScreen.transform.Find("Panel");
        if (panelTransform != null)
        {
            Transform titleTransform = panelTransform.Find("Name");
            if (titleTransform != null)
            {
                title = titleTransform.GetComponent<Text>();
                if (title == null)
                {
                    Debug.LogError("No Text component found on the 'title' GameObject.");
                }
            }

            Transform quoteTransform = panelTransform.Find("Quote");
            if (quoteTransform != null)
            {
                quote = quoteTransform.GetComponent<Text>();
            }

            Transform iconTransform = panelTransform.Find("Icon");
            if (iconTransform != null)
            {
                icon = iconTransform.GetComponent<Image>();
            }

            Transform questBoxTransform = panelTransform.Find("Quest Box");
            if (questBoxTransform != null)
            {
                Transform requestTransform = questBoxTransform.Find("Request");
                if (requestTransform != null)
                {
                    request = requestTransform.GetComponent<Text>();
                }

                Transform itemTransform = questBoxTransform.Find("Item");
                if (itemTransform != null)
                {
                    Transform imageTransform = itemTransform.Find("Image");
                    if (imageTransform != null)
                    {
                        image = imageTransform.GetComponent<Image>();
                    }
                }

                Transform giveTransform = questBoxTransform.Find("Give Button");
                if (giveTransform != null)
                {
                    button = giveTransform.GetComponent<Button>();
                    button.onClick.AddListener(Give);
                }
            }
        }

        molemanInfo = moleman.GetComponent<MolemanClass>();
        if (title != null)
        {
            title.text = molemanInfo.fName + " " + molemanInfo.lName + " (" + molemanInfo.job + ")";
        }
        if (quote != null)
        {
            quote.text = molemanInfo.quote;
        }
        if (icon != null)
        {
            icon.sprite = molemanInfo.icon;
        }

        GenerateQuest();
    }

    void Update()
    {
        if (request != null && image != null)
        {
            request.text = "Can you bring me " + itemQuantity + " " + itemTitle + "?";
            image.sprite = itemSprite;
        }

        float distanceToPlayer = Vector2.Distance(transform.position, player.transform.position);

        if (distanceToPlayer < 2.5f)
        {
            text1.SetActive(true);
        }
        else
        {
            text1.SetActive(false);
        }
    }

    public void GenerateQuest()
    {
        float r = Random.Range(0, 100);

        if (r >= 0 && r < 50)
        {
            itemType = Random.Range(1, 5);
            if (itemType == 1)
            {
                itemTitle = "Coal";
                itemSprite = coal;
            }
            if (itemType == 2)
            {
                itemTitle = "Mushrooms";
                itemSprite = mushroom;
            }
            if (itemType == 3)
            {
                itemTitle = "Plastic";
                itemSprite = plastic;
            }
            if (itemType == 4)
            {
                itemTitle = "Steel";
                itemSprite = steel;
            }
            itemQuantity = Random.Range(20, 31);
        }
        else if (r >= 50 && r < 65)
        {
            itemType = Random.Range(5, 7);
            if (itemType == 5)
            {
                itemTitle = "Wood";
                itemSprite = wood;
            }
            if (itemType == 6)
            {
                itemTitle = "Trinitite";
                itemSprite = trinitite;
            }
            itemQuantity = Random.Range(3, 10);
        }
        else if (r >= 65 && r <= 100)
        {
            int i = Random.Range(1, 8);
            if (i == 1 || i == 2)
            {
                itemType = 7;
                itemTitle = "Waddlepus Steaks";
                itemSprite = steak;
            }
            if (i == 3 || i == 4)
            {
                itemType = 8;
                itemTitle = "Womster Dogs";
                itemSprite = dog;
            }
            if (i == 5 || i == 6)
            {
                itemType = 9;
                itemTitle = "BombleBee HoneyHams";
                itemSprite = honeyham;
            }
            if (i == 7)
            {
                itemType = 10;
                itemTitle = "Ration Packs";
                itemSprite = rations;
            }
            itemQuantity = Random.Range(1, 5);
        }
    }

    public void Give()
    {
        if (itemType == 1)
        {
            if (player.GetComponent<Inventory>().coalCount >= itemQuantity)
            {
                player.GetComponent<Inventory>().coalCount = player.GetComponent<Inventory>().coalCount - itemQuantity;
                tradeAudio.PlayOneShot(tradeAudio.clip);
                logic.GetComponent<UILogic>().score += 2500;
                GenerateQuest();
            }
            else
            {
                errorAudio.PlayOneShot(errorAudio.clip);
            }
        }
        else if (itemType == 2)
        {
            if (player.GetComponent<Inventory>().mushroomCount >= itemQuantity)
            {
                player.GetComponent<Inventory>().mushroomCount = player.GetComponent<Inventory>().mushroomCount - itemQuantity;
                tradeAudio.PlayOneShot(tradeAudio.clip);
                logic.GetComponent<UILogic>().score += 2500;
                GenerateQuest();
            }
            else
            {
                errorAudio.PlayOneShot(errorAudio.clip);
            }
        }
        else if (itemType == 3)
        {
            if (player.GetComponent<Inventory>().plasticCount >= itemQuantity)
            {
                player.GetComponent<Inventory>().plasticCount = player.GetComponent<Inventory>().plasticCount - itemQuantity;
                tradeAudio.PlayOneShot(tradeAudio.clip);
                logic.GetComponent<UILogic>().score += 2500;
                GenerateQuest();
            }
            else
            {
                errorAudio.PlayOneShot(errorAudio.clip);
            }
        }
        else if (itemType == 4)
        {
            if (player.GetComponent<Inventory>().steelCount >= itemQuantity)
            {
                player.GetComponent<Inventory>().steelCount = player.GetComponent<Inventory>().steelCount - itemQuantity;
                tradeAudio.PlayOneShot(tradeAudio.clip);
                logic.GetComponent<UILogic>().score += 2500;
                GenerateQuest();
            }
            else
            {
                errorAudio.PlayOneShot(errorAudio.clip);
            }
        }
        else if (itemType == 5)
        {
            if (player.GetComponent<Inventory>().woodCount >= itemQuantity)
            {
                player.GetComponent<Inventory>().woodCount = player.GetComponent<Inventory>().woodCount - itemQuantity;
                tradeAudio.PlayOneShot(tradeAudio.clip);
                logic.GetComponent<UILogic>().score += 2500;
                GenerateQuest();
            }
            else
            {
                errorAudio.PlayOneShot(errorAudio.clip);
            }
        }
        else if (itemType == 6)
        {
            if (player.GetComponent<Inventory>().trinititeCount >= itemQuantity)
            {
                player.GetComponent<Inventory>().trinititeCount = player.GetComponent<Inventory>().trinititeCount - itemQuantity;
                tradeAudio.PlayOneShot(tradeAudio.clip);
                logic.GetComponent<UILogic>().score += 2500;
                GenerateQuest();
            }
            else
            {
                errorAudio.PlayOneShot(errorAudio.clip);
            }
        }
        else if (itemType == 7)
        {
            if (player.GetComponent<Inventory>().steakCount >= itemQuantity)
            {
                player.GetComponent<Inventory>().steakCount = player.GetComponent<Inventory>().steakCount - itemQuantity;
                tradeAudio.PlayOneShot(tradeAudio.clip);
                logic.GetComponent<UILogic>().score += 2500;
                GenerateQuest();
            }
            else
            {
                errorAudio.PlayOneShot(errorAudio.clip);
            }
        }
        else if (itemType == 8)
        {
            if (player.GetComponent<Inventory>().dogCount >= itemQuantity)
            {
                player.GetComponent<Inventory>().dogCount = player.GetComponent<Inventory>().dogCount - itemQuantity;
                tradeAudio.PlayOneShot(tradeAudio.clip);
                logic.GetComponent<UILogic>().score += 2500;
                GenerateQuest();
            }
            else
            {
                errorAudio.PlayOneShot(errorAudio.clip);
            }
        }
        else if (itemType == 9)
        {
            if (player.GetComponent<Inventory>().honeyhamCount >= itemQuantity)
            {
                player.GetComponent<Inventory>().honeyhamCount = player.GetComponent<Inventory>().honeyhamCount - itemQuantity;
                tradeAudio.PlayOneShot(tradeAudio.clip);
                logic.GetComponent<UILogic>().score += 2500;
                GenerateQuest();
            }
            else
            {
                errorAudio.PlayOneShot(errorAudio.clip);
            }
        }
        else if (itemType == 10)
        {
            if (player.GetComponent<Inventory>().rationsCount >= itemQuantity)
            {
                player.GetComponent<Inventory>().rationsCount = player.GetComponent<Inventory>().rationsCount - itemQuantity;
                tradeAudio.PlayOneShot(tradeAudio.clip);
                logic.GetComponent<UILogic>().score += 2500;
                GenerateQuest();
            }
            else
            {
                errorAudio.PlayOneShot(errorAudio.clip);
            }
        }
    }
}
