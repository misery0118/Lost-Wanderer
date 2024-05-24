using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RelicPickup : MonoBehaviour
{
    public Relics RelicData;

    void Pickup() {
        InventoryManager.Instance.AddRelics(RelicData);
        Destroy(gameObject);
    }

    private void OnMouseDown() {
        Pickup();
    }
}
