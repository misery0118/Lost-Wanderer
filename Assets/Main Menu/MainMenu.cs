using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class MenuManager : MonoBehaviour
{
    public GameObject titleScreen;
    public GameObject mainMenu;
    public CanvasGroup titleScreenCanvasGroup;
    public CanvasGroup mainMenuCanvasGroup;
    public AudioSource startSound;
    public AudioSource backSound;
    public float fadeDuration = 1f;

    private bool titleScreenActive = true;

    void Start()
    {
        titleScreenCanvasGroup.alpha = 1f;
        mainMenuCanvasGroup.alpha = 0f;
    }

    void Update()
    {
        if (titleScreenActive && Input.anyKeyDown)
        {
            StartCoroutine(FadeOutTitleScreen());
            titleScreenActive = false;
        }

        if (!titleScreenActive && Input.GetKeyDown(KeyCode.Backspace))
        {
            StartCoroutine(FadeOutMainMenu());
            titleScreenActive = true;
        }
    }

    IEnumerator FadeOutTitleScreen()
    {
        float elapsedTime = 0f;
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            titleScreenCanvasGroup.alpha = 1f - (elapsedTime / fadeDuration);
            yield return null;
        }
        titleScreen.SetActive(false);
        mainMenu.SetActive(true);
        startSound.Play();
        StartCoroutine(FadeInMainMenu());
    }

    IEnumerator FadeInMainMenu()
    {
        float elapsedTime = 0f;
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            mainMenuCanvasGroup.alpha = elapsedTime / fadeDuration;
            yield return null;
        }
    }

    IEnumerator FadeOutMainMenu()
    {
        float elapsedTime = 0f;
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            mainMenuCanvasGroup.alpha = 1f - (elapsedTime / fadeDuration);
            yield return null;
        }
        titleScreen.SetActive(true);
        mainMenu.SetActive(false);
        backSound.Play();
        StartCoroutine(FadeInTitleScreen());
    }

    IEnumerator FadeInTitleScreen()
    {
        float elapsedTime = 0f;
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            titleScreenCanvasGroup.alpha = elapsedTime / fadeDuration;
            yield return null;
        }
    }
}
