using UnityEngine;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance; // Singleton instance
    public CubeColorChanger[] cubes; // Array to store all the cubes
    public GameObject door; // Reference to the door object
    public float revertColorDelay = 1f; // Delay before reverting the color back to original

    private int currentStep = 0; // Track the current step in the sequence
    private List<CubeColorChanger> correctCubes = new List<CubeColorChanger>(); // Track correctly triggered cubes

    void Awake()
    {
        // Ensure there's only one instance of the GameManager
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void CubeTriggered(CubeColorChanger triggeredCube)
    {
        // Check if the triggered cube is the correct one in the sequence
        if (triggeredCube.cubeIndex == currentStep)
        {
            // Change the cube's color to green
            triggeredCube.SetColor(Color.green);
            correctCubes.Add(triggeredCube); // Add to the list of correct cubes
            currentStep++;

            // Check if the player completed the sequence
            if (currentStep >= cubes.Length)
            {
                PuzzleComplete();
                UnlockDoor();
            }
        }
        else
        {
            // Change the wrong cube's color to red and revert back to original color after delay
            triggeredCube.SetColor(Color.red);
            triggeredCube.RevertToOriginalColor(revertColorDelay);
            // Reset the sequence
            ResetSequence();
        }
    }

    private void PuzzleComplete()
    {
        Debug.Log("Puzzle Complete");
    }

    private void UnlockDoor()
    {
        // Implement the logic to unlock the door or open the pathway
        door.SetActive(false); // Example: deactivate the door object
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
        // Reset the colors of correctly triggered cubes
        foreach (var cube in correctCubes)
        {
            cube.ResetColor();
        }
        correctCubes.Clear(); // Clear the list of correct cubes
    }
}
