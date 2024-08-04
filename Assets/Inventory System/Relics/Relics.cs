using System;
using UnityEngine;

[CreateAssetMenu(fileName = "New Relic", menuName = "Item/Create new Relic")]
public class Relics : ScriptableObject {
    public int id;
    public string relicName;
    public int value;
    public Sprite icon;

    //Temporary Code
    public string iconPath;
}
