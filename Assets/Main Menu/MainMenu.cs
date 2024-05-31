using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.UI;

namespace MenuSystem
{
    public class MainMenu : MonoBehaviour
    {
        public ButtonManager buttonManager;
        public GameObject titleScreen;
        public GameObject mainMenu;
        public GameObject storyExpand;
        public GameObject quitExpand; // New QuitExpand GameObject
        public CanvasGroup titleScreenCanvasGroup;
        public CanvasGroup mainMenuCanvasGroup;
        public CanvasGroup storyExpandCanvasGroup;
        public CanvasGroup quitExpandCanvasGroup; // New QuitExpand CanvasGroup
        public AudioSource startSound;
        public AudioSource backSound;
        public float fadeDuration = 1f;

        private bool titleScreenActive = true;

        public bool storyExpandActive = false;
        public bool quitExpandActive = false;

        void Start()
        {
            titleScreenCanvasGroup.alpha = 1f;
            mainMenuCanvasGroup.alpha = 0f;
            storyExpandCanvasGroup.alpha = 0f;
            quitExpandCanvasGroup.alpha = 0f;

            // Ensure cursor is visible and unlocked at the start
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

        void Update()
        {
            if (titleScreenActive && Input.anyKeyDown)
            {
                StartCoroutine(FadeOutTitleScreen());
                titleScreenActive = false;
            }
            else if ((buttonManager.IsStoryExpandActive() || buttonManager.IsQuitExpandActive()) && Input.GetKeyDown(KeyCode.Backspace))
            {
                if (storyExpandActive)
                    StartCoroutine(FadeOutStoryExpand());
                else if (quitExpandActive) // Check if quitExpand is active
                    StartCoroutine(FadeOutQuitExpand());
            }
            else if (!titleScreenActive && Input.GetKeyDown(KeyCode.Backspace))
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

            // Ensure cursor is visible and unlocked
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
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
            storyExpand.SetActive(false);
            quitExpand.SetActive(false); // Hide quitExpand
            buttonManager.Unselect();
            backSound.Play();
            StartCoroutine(FadeInTitleScreen());

            // Ensure cursor is visible and unlocked
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
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

        public IEnumerator FadeInStoryExpand()
        {
            float elapsedTime = 0f;
            while (elapsedTime < fadeDuration)
            {
                elapsedTime += Time.deltaTime;
                storyExpandCanvasGroup.alpha = elapsedTime / fadeDuration;
                yield return null;
            }
            storyExpand.SetActive(true);
            storyExpandActive = true; // Set storyExpand as active
        }

        public IEnumerator FadeOutStoryExpand()
        {
            float elapsedTime = 0f;
            while (elapsedTime < fadeDuration)
            {
                elapsedTime += Time.deltaTime;
                storyExpandCanvasGroup.alpha = 1f - (elapsedTime / fadeDuration);
                yield return null;
            }
            storyExpand.SetActive(false);
            storyExpandActive = false; // Set storyExpand as inactive
        }

        public IEnumerator FadeInQuitExpand()
        {
            float elapsedTime = 0f;
            while (elapsedTime < fadeDuration)
            {
                elapsedTime += Time.deltaTime;
                quitExpandCanvasGroup.alpha = elapsedTime / fadeDuration;
                yield return null;
            }
            quitExpand.SetActive(true);
            quitExpandActive = true; // Set quitExpand as active
        }

        public IEnumerator FadeOutQuitExpand()
        {
            float elapsedTime = 0f;
            while (elapsedTime < fadeDuration)
            {
                elapsedTime += Time.deltaTime;
                quitExpandCanvasGroup.alpha = 1f - (elapsedTime / fadeDuration);
                yield return null;
            }
            quitExpand.SetActive(false);
            quitExpandActive = false; // Set quitExpand as inactive
        }
    }
}
