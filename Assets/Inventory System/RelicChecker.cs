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
        List<Relics>inventory = InventoryManager.Instance.Relicss;
        
        Relics item = inventory.Find(item => item.id == relics.id);
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
