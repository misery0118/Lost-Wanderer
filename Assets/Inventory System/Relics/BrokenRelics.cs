using System;
using UnityEngine;

[CreateAssetMenu(fileName = "New Broken Relic", menuName = "Item/Create new Broken Relic")]
public class BrokenRelics : ScriptableObject {
    public int id;
    public string brokenrelicName;
    public int value;
    public Sprite icon;
}
 