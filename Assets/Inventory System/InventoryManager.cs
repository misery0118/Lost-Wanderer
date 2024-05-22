using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance;
    public List<Items> Items = new List<Items>();

    private void Awake() {
        Instance = this;
    }

    public void Add(Items item) {
        Items.Add(item);
    }

    public void Remove(Items item) {
        Items.Remove(item);
    }
}
