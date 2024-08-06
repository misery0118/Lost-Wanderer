using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetPlayerPrefs : MonoBehaviour
{
    void Start()
    {
        if (!PlayerPrefs.HasKey("FirstRun"))
        {
            PlayerPrefs.DeleteAll();
            PlayerPrefs.SetInt("FirstRun", 1);
            PlayerPrefs.Save();
            Debug.Log("PlayerPrefs have been cleared on first run.");
        }
    }
}
