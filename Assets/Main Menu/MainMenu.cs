using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public GameObject titleScreen;
    public GameObject mainMenu;

    private bool titleScreenActive = true;

    void Start()
    {
        titleScreen.SetActive(true);
        mainMenu.SetActive(false);
    }

    void Update()
    {
        if (titleScreenActive && Input.anyKeyDown)
        {
            titleScreen.SetActive(false);
            mainMenu.SetActive(true);
            titleScreenActive = false;
        }

        if (!titleScreenActive && Input.GetKeyDown(KeyCode.Backspace))
        {
            titleScreen.SetActive(true);
            mainMenu.SetActive(false);
            titleScreenActive = true;
        }
    }
}
