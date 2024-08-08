using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrokenRelicChecker : MonoBehaviour
{
    [SerializeField] BrokenRelics brokenrelics;
    [SerializeField] GameObject itemObj;
    void Update()
    {
        CheckItem();
    }

    private void CheckItem()
    {
      /* if (itemObj == null)
        {
            Debug.LogWarning("Item Obj is null.");
            return;
        }*/  

        List<BrokenRelics> inventory = InventoryManager.Instance.BrokenRelicss;

        if (inventory == null)
        {
            Debug.LogWarning("Inventory is null.");
            return;
        }

        BrokenRelics item = inventory.Find(i => i.id == brokenrelics.id);
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