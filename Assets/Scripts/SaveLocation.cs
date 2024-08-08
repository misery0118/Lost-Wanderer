using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveLocation : MonoBehaviour
{
    public string saveKey; // Unique identifier for each save spot
    public float x, y, z;

    public GameObject particleEffect;  // Reference to the particle effect
    public GameObject lightEffect;     // Reference to the light effect

    void Start()
    {
        // Optional: Load saved position and effect states when the game starts
        Load();
    }

    public void Save()
    {
        x = transform.position.x;
        y = transform.position.y;
        z = transform.position.z;

        // Save position data
        PlayerPrefs.SetFloat(saveKey + "_x", x);
        PlayerPrefs.SetFloat(saveKey + "_y", y);
        PlayerPrefs.SetFloat(saveKey + "_z", z);

        // Save the active state of the particle and light effects
        PlayerPrefs.SetInt(saveKey + "_particleActive", particleEffect.activeSelf ? 1 : 0);
        PlayerPrefs.SetInt(saveKey + "_lightActive", lightEffect.activeSelf ? 1 : 0);

        InventoryManager.Instance.SaveInventory();
        
        PlayerPrefs.Save();

        Debug.Log("Saved location " + saveKey + ": Position: " + x + ", " + y + ", " + z);
    }

    public void Load()
    {
        if (PlayerPrefs.HasKey(saveKey + "_x") && PlayerPrefs.HasKey(saveKey + "_y") && PlayerPrefs.HasKey(saveKey + "_z"))
        {
            x = PlayerPrefs.GetFloat(saveKey + "_x");
            y = PlayerPrefs.GetFloat(saveKey + "_y");
            z = PlayerPrefs.GetFloat(saveKey + "_z");

            transform.position = new Vector3(x, y, z);

            Debug.Log("Loaded location " + saveKey + ": Position: " + x + ", " + y + ", " + z);
        }
        else
        {
            Debug.Log("No saved position found for " + saveKey);
        }

        // Load and set the active state of the particle and light effects
        if (PlayerPrefs.HasKey(saveKey + "_particleActive"))
        {
            bool particleActive = PlayerPrefs.GetInt(saveKey + "_particleActive") == 1;
            particleEffect.SetActive(particleActive);
        }

        if (PlayerPrefs.HasKey(saveKey + "_lightActive"))
        {
            bool lightActive = PlayerPrefs.GetInt(saveKey + "_lightActive") == 1;
            lightEffect.SetActive(lightActive);
        }
    }
}
