using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance;
    public List<Items> Itemss = new List<Items>();

    public Transform ItemContent;
    public GameObject InventoryItem;

    private void Awake() {
        Instance = this;
    }

    public void Add(Items item) {
        Itemss.Add(item);
    }

    public void Remove(Items item) {
        Itemss.Remove(item);
    }

    public void ListItems() {
        foreach (Transform Itemss in ItemContent) {
            Destroy(Itemss.gameObject);
        }
        foreach (var item in Itemss) {
            GameObject obj = Instantiate(InventoryItem, ItemContent);
            var itemName = obj.transform.Find("ItemName").GetComponent<TMPro.TextMeshProUGUI>();
            var itemIcon = obj.transform.Find("ItemIcon").GetComponent<Image>();

            itemName.text = item.itemName;
            itemIcon.sprite = item.icon;
        }
    }

    public void CleanItems() {
        foreach (Transform Itemss in ItemContent) {
            Destroy(Itemss.gameObject);
        }
    }
}
