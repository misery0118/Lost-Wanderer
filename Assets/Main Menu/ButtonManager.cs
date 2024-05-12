using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Unity.Mathematics;

namespace MenuSystem
{
    public class ButtonManager : MonoBehaviour
    {
        public MainMenu mainMenu;
        public Button storyButton;
        public Button extrasButton;
        public Button optionsButton;
        public Button quitButton;

        // Story Expanded Buttons
        public Button newGameButton;
        public Button loadGameButton;
        public Button chapterSelectButton;

        // Quit Expanded Buttons
        public GameObject quitExpand;
        public Button YesButton;
        public Button NoButton;

        public GameObject backgroundPanel;
        public GameObject storyExpand;
        public float animationDuration = 0.5f;
        public float animationOffset = 25f;

        public AudioSource buttonSound;
        public AudioSource buttonClickSound;

        private Vector3[] originalPositions;
        private int selectedIndex = -1;

        void Start()
        {
            if (backgroundPanel == null)
            {
                Debug.LogError("Background panel is not assigned.");
                return;
            }

            // Store the original positions of all buttons
            originalPositions = new Vector3[]
            {
                storyButton.transform.localPosition,
                extrasButton.transform.localPosition,
                optionsButton.transform.localPosition,
                quitButton.transform.localPosition,
                newGameButton.transform.localPosition,
                loadGameButton.transform.localPosition,
                chapterSelectButton.transform.localPosition,
                YesButton.transform.localPosition,
                NoButton.transform.localPosition
            };

            // Attach functions to handle button events for all buttons
            AttachButtonEvents(storyButton, 0, ShowStoryExpand);
            AttachButtonEvents(extrasButton, 1);
            AttachButtonEvents(optionsButton, 2);
            AttachButtonEvents(quitButton, 3, ShowQuitExpand);
            AttachButtonEvents(newGameButton, 4);
            AttachButtonEvents(loadGameButton, 5);
            AttachButtonEvents(chapterSelectButton, 6);
            AttachButtonEvents(YesButton, 7);
            AttachButtonEvents(NoButton, 8);



            // Attach a listener to detect clicks on the background panel
            EventTrigger backgroundTrigger = backgroundPanel.GetComponent<EventTrigger>();
            if (backgroundTrigger == null)
            {
                Debug.LogError("Background panel does not have an EventTrigger component.");
                return;
            }
            EventTrigger.Entry clickEntry = new EventTrigger.Entry { eventID = EventTriggerType.PointerClick };
            clickEntry.callback.AddListener((data) => { OnBackgroundClicked(); });
            backgroundTrigger.triggers.Add(clickEntry);
        }

        //Button Function
        private void ShowStoryExpand()
        {
            // Activate the storyExpand GameObject if it's not already active
            if (!storyExpand.activeSelf)
            {
                storyExpand.SetActive(true);

                // Start the fade-in animation for storyExpand
                StartCoroutine(mainMenu.FadeInStoryExpand());
            }
        }

        //Button Function
        private void ShowQuitExpand()
        {
            // Activate the quitExpand GameObject if it's not already active
            if (!quitExpand.activeSelf)
            {
                quitExpand.SetActive(true);

                // Start the fade-in animation for quitExpand
                StartCoroutine(mainMenu.FadeInQuitExpand());
            }
        }

        // Function to attach events to a button
        void AttachButtonEvents(Button button, int index, UnityEngine.Events.UnityAction clickHandler = null)
        {
            if (button == null)
            {
                Debug.LogError("Button " + index + " is not assigned.");
                return;
            }

            // Handle button click
            button.onClick.AddListener(() => OnButtonClicked(index, clickHandler));

            // Handle pointer enter event (hover)
            EventTrigger trigger = button.gameObject.GetComponent<EventTrigger>();
            if (trigger == null)
                trigger = button.gameObject.AddComponent<EventTrigger>();

            EventTrigger.Entry entry = new EventTrigger.Entry { eventID = EventTriggerType.PointerEnter };
            entry.callback.AddListener((data) => { OnButtonHighlighted(index); });
            trigger.triggers.Add(entry);

            // Handle pointer exit event
            entry = new EventTrigger.Entry { eventID = EventTriggerType.PointerExit };
            entry.callback.AddListener((data) => { OnButtonUnhighlighted(index); });
            trigger.triggers.Add(entry);
        }

        private void OnButtonClicked(int index, UnityEngine.Events.UnityAction clickHandler)
        {   

            if (selectedIndex == index)
            {
                // If the same button is clicked again, deselect it and return to its original position
                selectedIndex = -1;
                ReturnToOriginalPosition(index);
            }
            else
            {
                // Deselect the previously selected button and return it to its original position
                if (selectedIndex != -1)
                {
                    ReturnToOriginalPosition(selectedIndex);
                }

                // Select the clicked button
                selectedIndex = index;
                // Animate the clicked button to move to the left
                LeanTween.moveLocalX(GetButtonObject(index), originalPositions[index].x - animationOffset, animationDuration);

                // Hide QuitExpand when No Button is pressed
                if (selectedIndex == 8)
                {
                    StartCoroutine(mainMenu.FadeOutQuitExpand());
                    mainMenu.quitExpandActive = false;
                }

                // Hide QuitExpand when Story Button is pressed
                if (selectedIndex == 0)
                {
                    StartCoroutine(mainMenu.FadeOutQuitExpand());
                    mainMenu.quitExpandActive = false;
                }

                // If storyExpand or quitExpand is active and one of the buttons (extras, options, quit) is clicked,
                // close the storyExpand or quitExpand panel accordingly
                if ((IsStoryExpandActive() || IsQuitExpandActive()) && (index == 1 || index == 2 || index == 3))
                {
                    StartCoroutine(mainMenu.FadeOutStoryExpand());
                    mainMenu.storyExpandActive = false;
                    StartCoroutine(mainMenu.FadeOutQuitExpand());
                    mainMenu.quitExpandActive = false;
                }

                // Play the button click sound
                if (buttonClickSound != null)
                {
                    buttonClickSound.PlayOneShot(buttonClickSound.clip);
                }
            }

            // Execute the provided click handler
            clickHandler?.Invoke();
        }

        // Function to handle button hover
        private void OnButtonHighlighted(int index)
        {
            // Do nothing if the button is already selected
            if (selectedIndex == index) return;

            // Animate the button to move to the left only if not already highlighted
            LeanTween.moveLocalX(GetButtonObject(index), originalPositions[index].x - animationOffset, animationDuration);

            // Play the button highlight sound
            if (buttonSound != null && buttonSound.clip != null)
            {
                buttonSound.PlayOneShot(buttonSound.clip);
            }
        }

        // Function to handle button unhighlight
        private void OnButtonUnhighlighted(int index)
        {
            // If the button is selected, do nothing
            if (selectedIndex == index) return;

            // Return the button to its original position if it's not selected
            ReturnToOriginalPosition(index);
        }

        // Function to return the button to its original position
        public void ReturnToOriginalPosition(int index)
        {
            // Animate the specified button to return to its original position
            LeanTween.moveLocal(GetButtonObject(index), originalPositions[index], animationDuration);
        }

        // Helper function to get the button object by index
        private GameObject GetButtonObject(int index)
        {
            switch (index)
            {
                case 0:
                    return storyButton.gameObject;
                case 1:
                    return extrasButton.gameObject;
                case 2:
                    return optionsButton.gameObject;
                case 3:
                    return quitButton.gameObject;
                case 4:
                    return newGameButton.gameObject;
                case 5:
                    return loadGameButton.gameObject;
                case 6:
                    return chapterSelectButton.gameObject;
                case 7:
                    return YesButton.gameObject;
                case 8:
                    return NoButton.gameObject;
                default:
                    return null;
            }
        }

        // Function to handle click on the background panel
        private void OnBackgroundClicked()
        {
            // Deselect the currently selected button (if any) and return it to its original position
            if (selectedIndex != -1)
            {
                ReturnToOriginalPosition(selectedIndex);
                selectedIndex = -1;
                if (IsStoryExpandActive())
                {
                    StartCoroutine(mainMenu.FadeOutStoryExpand());
                    mainMenu.storyExpandActive = false;
                }
                if (IsQuitExpandActive())
                {
                    StartCoroutine(mainMenu.FadeOutQuitExpand());
                    mainMenu.quitExpandActive = false;
                }
            }
        }

        // Unselect buttons being called on MainMenu.cs
        public void Unselect()
        {
            if (selectedIndex != -1)
            {
                ReturnToOriginalPosition(selectedIndex);
                selectedIndex = -1;
            }
        }

        public bool IsStoryExpandActive()
        {
            return storyExpand != null && storyExpand.activeSelf;
        }

        public bool IsQuitExpandActive()
        {
            return quitExpand != null && quitExpand.activeSelf;
        }

        // Function to handle Backspace key press
        private void Update()
        {
            if ((IsStoryExpandActive() || IsQuitExpandActive()) && Input.GetKeyDown(KeyCode.Backspace))
            {
                // Deselect the currently selected button (if any) and return it to its original position
                if (selectedIndex != -1)
                {
                    EventSystem.current.SetSelectedGameObject(null);
                    ReturnToOriginalPosition(selectedIndex);
                    selectedIndex = -1;
                    if (IsStoryExpandActive())
                    {
                        StartCoroutine(mainMenu.FadeOutStoryExpand());
                        mainMenu.storyExpandActive = false;
                    }
                    if (IsQuitExpandActive())
                    {
                        StartCoroutine(mainMenu.FadeOutQuitExpand());
                        mainMenu.quitExpandActive = false;
                    }
                }
            }
        }
    }
}
