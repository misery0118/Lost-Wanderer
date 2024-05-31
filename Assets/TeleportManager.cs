using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class nextscene : MonoBehaviour
{
    public SceneLoader sceneLoader; // Reference to the SceneLoader script
    public int sceneIndex;          // Index of the scene to load

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            sceneLoader.LoadScene(sceneIndex);
        }
    }
}
