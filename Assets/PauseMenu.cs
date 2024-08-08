using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public static bool GameIsPaused = false;

    public GameObject pauseMenuUI;
    public StarterAssets.StarterAssetsInputs starterAssetsInputs;

    private SceneLoader sceneLoader;

    // Input action reference for the Pause action
    private InputAction pauseAction;

    void Start()
    {
        sceneLoader = FindObjectOfType<SceneLoader>();

        // Assuming you have a PlayerInput component in your scene, you can get the InputAction here
        var playerInput = FindObjectOfType<PlayerInput>();
        pauseAction = playerInput.actions["Pause"];

        // Subscribe to the performed event
        pauseAction.performed += OnPause;
    }

    private void OnPause(InputAction.CallbackContext context)
    {
        if (GameIsPaused)
        {
            Resume();
        }
        else
        {
            Pause();
        }
    }

    void OnDestroy()
    {
        // Unsubscribe from the event when the object is destroyed to avoid memory leaks
        pauseAction.performed -= OnPause;
    }

    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        GameIsPaused = false;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        Debug.Log("Game Resumed");
    }

    void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        GameIsPaused = true;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        Debug.Log("Game Paused");
    }

    public void ReturnToMainMenu()
    {
        Resume();
    }

    public void QuitGame()
    {
        Debug.Log("Quitting game...");
        Application.Quit();
    }
}
