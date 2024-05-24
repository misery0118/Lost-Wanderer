using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance;
    public List<Items> Itemss = new List<Items>();
    public List<Relics> Relicss = new List<Relics>();
    public List<BrokenRelics> BrokenRelicss = new List<BrokenRelics>();


    public Transform ItemsContent;
    public GameObject InventoryItem;

    public Transform RelicsContent;
    public GameObject InventoryRelic;
    public Transform BrokenRelicsContent;
    public GameObject InventoryBrokenRelic;

    private void Awake()
    {
        Instance = this;
    }

    public void AddItems(Items item)
    {
        Itemss.Add(item);
    }

    public void RemoveItems(Items item)
    {
        Itemss.Remove(item);
    }

    public void AddRelics(Relics relic)
    {
        Relicss.Add(relic);
    }

    public void RemoveRelics(Relics relic)
    {
        Relicss.Remove(relic);
    }
    public void AddBrokenRelics(BrokenRelics brokenrelic)
    {
        BrokenRelicss.Add(brokenrelic);
    }

    public void RemoveBrokenRelics(BrokenRelics brokenrelic)
    {
        BrokenRelicss.Remove(brokenrelic);
    }

    public void ListItems()
    {
        foreach (Transform Itemss in ItemsContent)
        {
            Destroy(Itemss.gameObject);
        }
        foreach (var item in Itemss)
        {
            GameObject obj = Instantiate(InventoryItem, ItemsContent);
            var itemName = obj.transform.Find("ItemName").GetComponent<TMPro.TextMeshProUGUI>();
            var itemIcon = obj.transform.Find("ItemIcon").GetComponent<Image>();

            itemName.text = item.itemName;
            itemIcon.sprite = item.icon;
        }
    }

    public void ListRelics()
    {
        foreach (Transform Relicss in RelicsContent)
        {
            Destroy(Relicss.gameObject);
        }
        foreach (var relics in Relicss)
        {
            GameObject obj = Instantiate(InventoryRelic, RelicsContent);
            var relicName = obj.transform.Find("RelicName").GetComponent<TMPro.TextMeshProUGUI>();
            var relicIcon = obj.transform.Find("RelicIcon").GetComponent<Image>();

            relicName.text = relics.relicName;
            relicIcon.sprite = relics.icon;
        }
    }
    
    public void ListBrokenRelics()
    {
        foreach (Transform BrokenRelicss in BrokenRelicsContent)
        {
            Destroy(BrokenRelicss.gameObject);
        }
        foreach (var brokenrelics in BrokenRelicss)
        {
            GameObject obj = Instantiate(InventoryBrokenRelic, BrokenRelicsContent);
            var brokenrelicName = obj.transform.Find("BrokenRelicName").GetComponent<TMPro.TextMeshProUGUI>();
            var brokenrelicIcon = obj.transform.Find("BrokenRelicIcon").GetComponent<Image>();

            brokenrelicName.text = brokenrelics.brokenrelicName;
            brokenrelicIcon.sprite = brokenrelics.icon;
        }
    }

    public void CleanBrokenRelics() {
        foreach (Transform BrokenRelicss in BrokenRelicsContent)
        {
            Destroy(BrokenRelicss.gameObject);
        }
    }

    public void CleanRelics()
    {
        foreach (Transform Relicss in RelicsContent)
        {
            Destroy(Relicss.gameObject);
        }
    }

    public void CleanItems()
    {
        foreach (Transform Itemss in ItemsContent)
        {
            Destroy(Itemss.gameObject);
        }
    }
}
