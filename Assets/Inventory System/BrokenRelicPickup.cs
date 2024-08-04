using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 
public class BrokenRelicPickup : MonoBehaviour
{
    public BrokenRelics BrokenRelicData;

    void Pickup()
    {
        InventoryManager.Instance.AddBrokenRelics(BrokenRelicData);
        Destroy(gameObject);
    }

    private void OnMouseDown() {
        Debug.Log("OnMouseDown called");
        if (!PauseMenu.GameIsPaused)
        {
            Debug.Log("Game is not paused, picking up item");
            Pickup();
        }
        else
        {
            Debug.Log("Game is paused, cannot pick up item");
        }
    }
}