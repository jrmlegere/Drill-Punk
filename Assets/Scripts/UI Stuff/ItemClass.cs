using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemClass", menuName = "Item Class")]
public class ItemClass : ScriptableObject
{
    public enum ItemType
    {
        material,
        bullet,
        food,
        tool
    };

    public ItemType itemType;
    public Sprite sprite;
    public int quantity = 0;
}