using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ItemPickup : MonoBehaviour
{
    public Items ItemData;

    public float pickupRange = 2.0f; // Range within which the player can pick up the item

    private Transform playerTransform;
    private InputAction pickUpAction;

    void Start()
    {
        // Get the player's transform (assuming the player has a tag "Player")
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;

        // Set up input action
        var playerInput = FindObjectOfType<PlayerInput>();
        pickUpAction = playerInput.actions["PickItem"];
        pickUpAction.performed += OnPickUp;
    }

    private void OnDestroy()
    {
        // Unsubscribe from the input action event
        pickUpAction.performed -= OnPickUp;
    }

    private void OnPickUp(InputAction.CallbackContext context)
    {
        if (!PauseMenu.GameIsPaused && IsWithinPickupRange())
        {
            Pickup();
        }
        else
        {
            Debug.Log("Cannot pick up item: Game is paused or item is out of range");
        }
    }
    private bool IsWithinPickupRange()
    {
        // Check if the player is within the pickup range
        float distance = Vector3.Distance(playerTransform.position, transform.position);
        return distance <= pickupRange;
    }

    void Pickup()
    {
        // Add the relic to the inventory and destroy the game object
        InventoryManager.Instance.AddItems(ItemData);
        Destroy(gameObject);
    }

    private void OnMouseDown() {
       // Debug.Log("OnMouseDown called");
        if (!PauseMenu.GameIsPaused && IsWithinPickupRange())
        {
            Pickup();
        }
        else
        {
            Debug.Log("Game is paused or item is out of range, cannot pick up item");
        }
    }
}

