using UnityEngine;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance; // Singleton instance
    public CubeColorChanger[] cubes; // Array to store all the cubes
    public SphereColorChanger[] spheres; // Array to store all the spheres
    public GameObject door; // Reference to the door object
    public float revertColorDelay = 1f; // Delay before reverting the color back to original

    private int currentStep = 0; // Track the current step in the sequence
    private List<CubeColorChanger> correctCubes = new List<CubeColorChanger>(); // Track correctly triggered cubes
    private List<SphereColorChanger> correctSpheres = new List<SphereColorChanger>(); // Track correctly triggered spheres

    private Door doorScript;

    void Awake()
    {
        // Ensure there's only one instance of the GameManager
        if (Instance == null)
        {
            Instance = this;
            doorScript = door.GetComponent<Door>(); // Get the Door component
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void CubeTriggered(CubeColorChanger triggeredCube, SphereColorChanger triggeringSphere)
    {
        // Check if the triggered cube is the correct one in the sequence
        if (triggeredCube.cubeIndex == currentStep && triggeringSphere.sphereIndex == currentStep)
        {
            // Change the cube's color to green
            triggeredCube.SetColor(Color.green);
            correctCubes.Add(triggeredCube); // Add to the list of correct cubes
            correctSpheres.Add(triggeringSphere); // Add to the list of correct spheres
            currentStep++;

            // Lock the sphere's position
            triggeringSphere.LockPosition(triggeredCube.transform.position);

            // Check if the player completed the sequence
            if (currentStep >= cubes.Length)
            {
                PuzzleComplete(); // Notify puzzle completion
                UnlockDoor();
            }
        }
        else
        {
            // Change the wrong cube's color to red and revert back to original color after delay
            triggeredCube.SetColor(Color.red);
            triggeredCube.RevertToOriginalColor(revertColorDelay);
            
            // Allow the sphere to be moved again
            triggeringSphere.UnlockPosition();

            // Reset the sequence
            ResetSequence();
        }
    }

    private void PuzzleComplete()
    {
        Debug.Log("Puzzle Complete");
        // You can add additional actions here when the puzzle is completed
    }

    private void UnlockDoor()
    {
        // Open the door with animation
        if (doorScript != null)
        {
            // Assume the player's position is the parameter for opening the door
            Vector3 userPosition = transform.position; // or any suitable position
            doorScript.Open(userPosition);
        }
        else
        {
            Debug.LogError("Door script not found on the door object");
        }
    }

    private void ResetSequence()
    {
        // Reset the sequence and all cube colors to their original state
        currentStep = 0;
        foreach (var cube in cubes)
        {
            if (cube != null)
            {
                cube.ResetColor();
            }
        }
        foreach (var sphere in correctSpheres)
        {
            if (sphere != null)
            {
                sphere.UnlockPosition(); // Ensure the spheres can be moved again
            }
        }
        correctCubes.Clear(); // Clear the list of correct cubes
        correctSpheres.Clear(); // Clear the list of correct spheres
    }
}