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
        if(Instance != null)
        {
            Debug.Log("Existing");
        }
        Instance = this;
    }

    private void Start()
    {
        LoadInventory();
    }

    public void AddItems(Items item)
    {
        Itemss.Add(item);
        SaveInventory();
    }

    public void RemoveItems(Items item)
    {
        Itemss.Remove(item);
        SaveInventory();
    }

    public void AddRelics(Relics relic)
    {
        Relicss.Add(relic);
        SaveInventory();
    }

    public void RemoveRelics(Relics relic)
    {
        Relicss.Remove(relic);
        SaveInventory();
    }

    public void AddBrokenRelics(BrokenRelics brokenrelic)
    {
        BrokenRelicss.Add(brokenrelic);
        SaveInventory();
    }

    public void RemoveBrokenRelics(BrokenRelics brokenrelic)
    {
        BrokenRelicss.Remove(brokenrelic);
        SaveInventory();
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
            var itemName = obj.transform.Find("ItemName").GetComponent<TextMeshProUGUI>();
            var itemIcon = obj.transform.Find("ItemIcon").GetComponent<Image>();

            itemName.text = item.itemName;
            //itemIcon.sprite = item.icon;
            Sprite iconSprite = Resources.Load<Sprite>(item.iconPath);
            if (iconSprite != null)
            {
                itemIcon.sprite = iconSprite;
            }
            else
            {
                Debug.LogWarning($"Icon not found at path: {item.iconPath}");
            }
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
            var relicName = obj.transform.Find("RelicName").GetComponent<TextMeshProUGUI>();
            var relicIcon = obj.transform.Find("RelicIcon").GetComponent<Image>();

            relicName.text = relics.relicName;
            //relicIcon.sprite = relics.icon;
            Sprite iconSprite = Resources.Load<Sprite>(relics.iconPath);
            if (iconSprite != null)
            {
                relicIcon.sprite = iconSprite;
            }
            else
            {
                Debug.LogWarning($"Icon not found at path: {relics.iconPath}");
            }
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
            var brokenRelicName = obj.transform.Find("BrokenRelicName").GetComponent<TextMeshProUGUI>();
            var brokenRelicIcon = obj.transform.Find("BrokenRelicIcon").GetComponent<Image>();

            brokenRelicName.text = brokenrelics.brokenrelicName;
            //brokenRelicIcon.sprite = brokenrelics.icon;
            Sprite iconSprite = Resources.Load<Sprite>(brokenrelics.iconPath);
            if (iconSprite != null)
            {
                brokenRelicIcon.sprite = iconSprite;
            }
            else
            {
                Debug.LogWarning($"Icon not found at path: {brokenrelics.iconPath}");
            }
        }
    }

    public void CleanBrokenRelics()
    {
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

    public void SaveInventory()
    {
        // Save Items
        for (int i = 0; i < Itemss.Count; i++)
        {
            ItemData itemData = new ItemData
            {
                id = Itemss[i].id,
                itemName = Itemss[i].itemName,
                value = Itemss[i].value,
                iconPath = Itemss[i].iconPath // Save the relative path
            };
            string json = JsonUtility.ToJson(itemData);
            PlayerPrefs.SetString("Item_" + i, json);
        }
        PlayerPrefs.SetInt("ItemsCount", Itemss.Count);

        // Save Relics
        for (int i = 0; i < Relicss.Count; i++)
        {
            ItemData relicData = new ItemData
            {
                id = Relicss[i].id,
                itemName = Relicss[i].relicName,
                value = Relicss[i].value,
                iconPath = Relicss[i].iconPath // Save the relative path
            };
            string json = JsonUtility.ToJson(relicData);
            PlayerPrefs.SetString("Relic_" + i, json);
        }
        PlayerPrefs.SetInt("RelicsCount", Relicss.Count);

        // Save Broken Relics
        for (int i = 0; i < BrokenRelicss.Count; i++)
        {
            ItemData brokenRelicData = new ItemData
            {
                id = BrokenRelicss[i].id,
                itemName = BrokenRelicss[i].brokenrelicName,
                value = BrokenRelicss[i].value,
                iconPath = BrokenRelicss[i].iconPath // Save the relative path
            };
            string json = JsonUtility.ToJson(brokenRelicData);
            PlayerPrefs.SetString("BrokenRelic_" + i, json);
        }
        PlayerPrefs.SetInt("BrokenRelicsCount", BrokenRelicss.Count);

        PlayerPrefs.Save();
    }

    public void LoadInventory()
    {
        Itemss.Clear();
        Relicss.Clear();
        BrokenRelicss.Clear();

        // Load Items
        int itemsCount = PlayerPrefs.GetInt("ItemsCount", 0);
        for (int i = 0; i < itemsCount; i++)
        {
            string json = PlayerPrefs.GetString("Item_" + i);
            ItemData itemData = JsonUtility.FromJson<ItemData>(json);

            Items item = ScriptableObject.CreateInstance<Items>();
            item.id = itemData.id;
            item.itemName = itemData.itemName;
            item.value = itemData.value;
            item.iconPath = itemData.iconPath;
            Itemss.Add(item);
        }

        // Load Relics
        int relicsCount = PlayerPrefs.GetInt("RelicsCount", 0);
        for (int i = 0; i < relicsCount; i++)
        {
            string json = PlayerPrefs.GetString("Relic_" + i);
            ItemData relicData = JsonUtility.FromJson<ItemData>(json);

            Relics relic = ScriptableObject.CreateInstance<Relics>();
            relic.id = relicData.id;
            relic.relicName = relicData.itemName;
            relic.value = relicData.value;
            relic.iconPath = relicData.iconPath;
            Relicss.Add(relic);
        }

        // Load Broken Relics
        int brokenRelicsCount = PlayerPrefs.GetInt("BrokenRelicsCount", 0);
        for (int i = 0; i < brokenRelicsCount; i++)
        {
            string json = PlayerPrefs.GetString("BrokenRelic_" + i);
            ItemData brokenRelicData = JsonUtility.FromJson<ItemData>(json);

            BrokenRelics brokenRelic = ScriptableObject.CreateInstance<BrokenRelics>();
            brokenRelic.id = brokenRelicData.id;
            brokenRelic.brokenrelicName = brokenRelicData.itemName;
            brokenRelic.value = brokenRelicData.value;
            brokenRelic.iconPath = brokenRelicData.iconPath;
            BrokenRelicss.Add(brokenRelic);
        }

        // Refresh the inventory UI
        ListItems();
        ListRelics();
        ListBrokenRelics();
    }
}