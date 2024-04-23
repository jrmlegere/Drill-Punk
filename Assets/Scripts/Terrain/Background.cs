using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Background : MonoBehaviour
{
    public GameObject mainCamera;
    public GameObject logicManager;
    public Sprite daySprite;
    public Sprite nightSprite;

    public int dayTime = 6;
    public int nightTime = 18;

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector2(mainCamera.transform.position.x, (1f / 3f) * mainCamera.transform.position.y + GetComponent<SpriteRenderer>().bounds.size.y / 2f);

        if (logicManager.GetComponent<UILogic>().minutes < dayTime || logicManager.GetComponent<UILogic>().minutes >= nightTime)
        {
            GetComponent<SpriteRenderer>().sprite = nightSprite;
        }
        else
        {
            GetComponent<SpriteRenderer>().sprite = daySprite;
        }
    }
}