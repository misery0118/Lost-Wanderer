using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 
public class BrokenRelicPickup : MonoBehaviour
{
    public BrokenRelics BrokenRelicData;

    void Pickup() {
        InventoryManager.Instance.AddBrokenRelics(BrokenRelicData);
        Destroy(gameObject);
    }

    private void OnMouseDown() {
        Pickup();
    }
}
