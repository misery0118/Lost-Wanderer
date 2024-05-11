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
        public CanvasGroup titleScreenCanvasGroup;
        public CanvasGroup mainMenuCanvasGroup;
        public CanvasGroup storyExpandCanvasGroup;
        public AudioSource startSound;
        public AudioSource backSound;
        public float fadeDuration = 1f;

        private bool titleScreenActive = true;

        public bool storyExpandActive = false;

        void Start()
        {
            titleScreenCanvasGroup.alpha = 1f;
            mainMenuCanvasGroup.alpha = 0f;
            storyExpandCanvasGroup.alpha = 0f;
        }

        void Update()
        {
            if (titleScreenActive && Input.anyKeyDown)
            {
                StartCoroutine(FadeOutTitleScreen());
                titleScreenActive = false;
            }
            else if (buttonManager.IsStoryExpandActive() && Input.GetKeyDown(KeyCode.Backspace))
            {
                StartCoroutine(FadeOutStoryExpand());
                storyExpandActive = false;
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
            buttonManager.Unselect();
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
        }
    }
}
