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
        List<BrokenRelics>inventory = InventoryManager.Instance.BrokenRelicss;
        
        BrokenRelics item = inventory.Find(item => item.id == brokenrelics.id);
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
