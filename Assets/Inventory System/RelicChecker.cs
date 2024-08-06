using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RelicChecker : MonoBehaviour
{
    [SerializeField] Relics relics;
    [SerializeField] GameObject itemObj;

    void Update()
    {
        CheckItem();
    }

    private void CheckItem()
    {
        if (itemObj == null)
        {
            Debug.LogWarning("Item Obj is null.");
            return;
        }

        List<Relics> inventory = InventoryManager.Instance.Relicss;

        if (inventory == null)
        {
            Debug.LogWarning("Inventory is null.");
            return;
        }

        Relics item = inventory.Find(item => item.id == relics.id);
        if (item == null)
        {
            itemObj.SetActive(true);
        }
        else
        {
            itemObj.SetActive(false);
        }
    }
}