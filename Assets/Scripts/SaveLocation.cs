using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveLocation : MonoBehaviour
{

    public string saveKey; // Unique identifier for each save spot
    public float x, y, z;

    void Start()
    {
        // Optional: Load saved position when the game starts
        Load();
    }

    public void Save()
    {
        x = transform.position.x;
        y = transform.position.y;
        z = transform.position.z;

        // Use the unique saveKey to store position data
        PlayerPrefs.SetFloat(saveKey + "_x", x);
        PlayerPrefs.SetFloat(saveKey + "_y", y);
        PlayerPrefs.SetFloat(saveKey + "_z", z);
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
    }
}