using System;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Item/Create new Item")]
public class Items : ScriptableObject {
    public int id;
    public string itemName;
    public int value;
    public Sprite icon;

    //Temporary Code
    public string iconPath;
}