using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    public Items Items;

    void Pickup() {
        InventoryManager.Instance.Add(Items);
        Destroy(gameObject);
    }

    private void OnMouseDown() {
        Pickup();
    }
}
