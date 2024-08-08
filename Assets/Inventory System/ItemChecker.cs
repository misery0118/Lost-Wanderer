using System.Collections.Generic;
using UnityEngine;

public class ItemChecker : MonoBehaviour
{
    [SerializeField] Items itm;
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

        List<Items> inventory = InventoryManager.Instance.Itemss;

        if (inventory == null)
        {
            Debug.LogWarning("Inventory is null.");
            return;
        }

        Items item = inventory.Find(i => i.id == itm.id);
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
