using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ButtonManager : MonoBehaviour
{
    public Button storyButton;
    public Button extrasButton;
    public Button optionsButton;
    public Button quitButton;

    public GameObject backgroundPanel;
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

        // Store the original positions of the buttons
        originalPositions = new Vector3[]
        {
            storyButton.transform.localPosition,
            extrasButton.transform.localPosition,
            optionsButton.transform.localPosition,
            quitButton.transform.localPosition
        };

        // Attach functions to handle button events
        AttachButtonEvents(storyButton, 0);
        AttachButtonEvents(extrasButton, 1);
        AttachButtonEvents(optionsButton, 2);
        AttachButtonEvents(quitButton, 3);

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

    // Function to attach events to a button
    void AttachButtonEvents(Button button, int index)
    {
        if (button == null)
        {
            Debug.LogError("Button " + index + " is not assigned.");
            return;
        }

        // Handle button click
        button.onClick.AddListener(() => OnButtonClicked(index));

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

    // Function to handle button click
    private void OnButtonClicked(int index)
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

            // Play the button click sound
            if (buttonClickSound != null)
            {
                buttonClickSound.PlayOneShot(buttonClickSound.clip);
            }
        }
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
        }
    }
}
