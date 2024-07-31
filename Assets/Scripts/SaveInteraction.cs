using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveInteraction : MonoBehaviour
{
    public float interactionDistance = 2.0f;
    public LayerMask interactableLayer;
    public GameObject interactionUI; // Reference to the UI element

    private SaveLocation currentSaveLocation;

    void Start()
    {
        // Load the player's position at the start of the game
        LoadPlayerPosition();
    }

    void Update()
    {
        CheckForInteractable();

        if (Input.GetKeyDown(KeyCode.E) && currentSaveLocation != null)
        {
            // Save the position of the current save location
            currentSaveLocation.Save();

            // Move the player to the saved position of the current save location
            SavePlayerPosition();
        }
    }

    void CheckForInteractable()
    {
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, interactionDistance, interactableLayer))
        {
            SaveLocation saveLocation = hit.collider.GetComponent<SaveLocation>();
            if (saveLocation != null)
            {
                currentSaveLocation = saveLocation;
                ShowInteractionUI(true);
            }
            else
            {
                currentSaveLocation = null;
                ShowInteractionUI(false);
            }
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
        }
    }

    void SavePlayerPosition()
    {
        PlayerPrefs.SetFloat("playerX", transform.position.x);
        PlayerPrefs.SetFloat("playerY", transform.position.y);
        PlayerPrefs.SetFloat("playerZ", transform.position.z);
        PlayerPrefs.Save();
    }

    void LoadPlayerPosition()
    {
        if (PlayerPrefs.HasKey("playerX") && PlayerPrefs.HasKey("playerY") && PlayerPrefs.HasKey("playerZ"))
        {
            float x = PlayerPrefs.GetFloat("playerX");
            float y = PlayerPrefs.GetFloat("playerY");
            float z = PlayerPrefs.GetFloat("playerZ");

            transform.position = new Vector3(x, y, z);
        }
    }
}