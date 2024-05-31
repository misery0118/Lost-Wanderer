using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RelicPickup : MonoBehaviour
{
    public Relics RelicData;

    void Pickup()
    {
        InventoryManager.Instance.AddRelics(RelicData);
        Destroy(gameObject);
    }

    private void OnMouseDown()
    {
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
        