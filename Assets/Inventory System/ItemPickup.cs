using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    public Items ItemData;

    void Pickup() {
        InventoryManager.Instance.AddItems(ItemData);
        Destroy(gameObject);
    }

    private void OnMouseDown() {
        Pickup();
    }
}
