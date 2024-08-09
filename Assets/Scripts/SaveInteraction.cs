using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine;

public class SaveInteraction : MonoBehaviour
{
    public float interactionDistance = 2.0f; // Radius to check for nearby save locations
    public LayerMask interactableLayer; // Layer mask for filtering interactable objects
    public GameObject interactionUI; // Reference to the UI element

    private SaveLocation currentSaveLocation;
    private List<SaveLocation> saveLocations = new List<SaveLocation>();

    private InputAction SaveAction;

    void Start()
    {
        // Load the player's position at the start of the game
        LoadPlayerPosition();

        var playerInput = FindObjectOfType<PlayerInput>();
        if (playerInput != null)
        {
            SaveAction = playerInput.actions["Interact"];
            SaveAction.performed += OnSave;
        }
        else
        {
            Debug.LogError("PlayerInput component not found in the scene.");
        }

        // Find all SaveLocation objects in the scene
        saveLocations.AddRange(FindObjectsOfType<SaveLocation>());
    }

    void Update()
    {
        CheckForInteractable();
    }

    private void OnSave(InputAction.CallbackContext context)
    {
        if (currentSaveLocation != null)
        {
            //Debug.Log("Saving location: " + currentSaveLocation.saveKey);

            // Enable the particle and light effects when saving
            currentSaveLocation.particleEffect.SetActive(true);
            currentSaveLocation.lightEffect.SetActive(true);

            // Save the position of the current save location
            currentSaveLocation.Save();

            // Save the player's position
            SavePlayerPosition();
        }
        else
        {
            Debug.LogWarning("No current save location to save to.");
        }
    }

    void CheckForInteractable()
    {
        SaveLocation closestSaveLocation = null;
        float closestDistance = interactionDistance;

        foreach (SaveLocation saveLocation in saveLocations)
        {
            float distance = Vector3.Distance(transform.position, saveLocation.transform.position);
           // Debug.Log("Distance to SaveLocation " + saveLocation.saveKey + ": " + distance);

            if (distance <= interactionDistance)
            {
                if (closestSaveLocation == null || distance < closestDistance)
                {
                    closestSaveLocation = saveLocation;
                    closestDistance = distance;
                }
            }
        }

        if (closestSaveLocation != null)
        {
            currentSaveLocation = closestSaveLocation;
            ShowInteractionUI(true);
        }
        else
        {
            currentSaveLocation = null;
            ShowInteractionUI(false);
        }
    }

    void ShowInteractionUI(bool show)
    {
        if (interactionUI != null)
        {
            interactionUI.SetActive(show);
            //Debug.Log("Interaction UI set to: " + show);
        }
        else
        {
            //Debug.LogWarning("Interaction UI is not assigned.");
        }
    }

    void SavePlayerPosition()
    {
        PlayerPrefs.SetFloat("playerX", transform.position.x);
        PlayerPrefs.SetFloat("playerY", transform.position.y);
        PlayerPrefs.SetFloat("playerZ", transform.position.z);
        PlayerPrefs.Save();

       // Debug.Log("Saved player position: " + transform.position);
    }

    void LoadPlayerPosition()
    {
        if (PlayerPrefs.HasKey("playerX") && PlayerPrefs.HasKey("playerY") && PlayerPrefs.HasKey("playerZ"))
        {
            float x = PlayerPrefs.GetFloat("playerX");
            float y = PlayerPrefs.GetFloat("playerY");
            float z = PlayerPrefs.GetFloat("playerZ");
            transform.position = new Vector3(x, y, z);

            //Debug.Log("Loaded player position: " + transform.position);
        }
        else
        {
           // Debug.LogWarning("No saved player position found.");
        }
    }
}
