using System.Collections;
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
        List<Items>inventory = InventoryManager.Instance.Itemss;
        
        Items item = inventory.Find(item => item.id == itm.id);
        if(item == null)
        {
            itemObj.SetActive(true);
        }
        else
        {
            itemObj.SetActive(false);
        }
    }
}