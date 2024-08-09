using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterSwitcher : MonoBehaviour
{
    [Header("Input Actions")]
    public InputActionProperty kuroSwitchAction;
    public InputActionProperty raijinSwitchAction;

    [Header("Kuro Components")]
    public Animator catAnimator;
    public Avatar kuroAvatar;
    public GameObject kuroGameObject;
    public RuntimeAnimatorController catAnimatorController; // Ensure this is assigned

    [Header("Raijin Components")]
    public Animator raijinAnimator;
    public Avatar raijinAvatar;
    public MonoBehaviour raijinController;
    public GameObject raijinGameObject;
    public RuntimeAnimatorController raijinAnimatorController; // Ensure this is assigned

    [Header("CS Character Controller")]
    public GameObject CSCharacterController;
    private Animator csCharacterControllerAnimator;
    private MonoBehaviour csPlayerController;
    private MonoBehaviour catController;

    private Animator currentAnimator;
    private Avatar currentAvatar;
    private MonoBehaviour currentController;

    private void Start()
    {
        // Initialize controllers and switch to Raijin as the default character
        InitializeControllers();
        SwitchToRaijin();
    }

    private void OnEnable()
    {
        kuroSwitchAction.action.performed += OnKuroSwitch;
        raijinSwitchAction.action.performed += OnRaijinSwitch;
    }

    private void OnDisable()
    {
        kuroSwitchAction.action.performed -= OnKuroSwitch;
        raijinSwitchAction.action.performed -= OnRaijinSwitch;
    }

    private void InitializeControllers()
    {
        if (CSCharacterController != null)
        {
            csPlayerController = CSCharacterController.GetComponent("CSPlayerController") as MonoBehaviour;
            catController = CSCharacterController.GetComponent("CatController") as MonoBehaviour;
            csCharacterControllerAnimator = CSCharacterController.GetComponent<Animator>();

            if (csPlayerController == null)
            {
                Debug.LogError("CSPlayerController component not found on CSCharacterController");
            }
            if (catController == null)
            {
                Debug.LogError("CatController component not found on CSCharacterController");
            }
            if (csCharacterControllerAnimator == null)
            {
                Debug.LogError("Animator component not found on CSCharacterController");
            }
        }
        else
        {
            Debug.LogError("CSCharacterController GameObject is not assigned.");
        }
    }

    private void OnKuroSwitch(InputAction.CallbackContext context)
    {
        SwitchToKuro();
    }

    private void OnRaijinSwitch(InputAction.CallbackContext context)
    {
        SwitchToRaijin();
    }

    private void SwitchToKuro()
    {
        // Disable the current controller if one is active
        if (currentController != null)
        {
            Debug.Log("Disabling current controller.");
            currentController.enabled = false;
        }

        // Enable Kuro components and game object
        if (catAnimator != null)
        {
            Debug.Log("Enabling Kuro Animator.");
            catAnimator.enabled = true;
            catAnimator.avatar = kuroAvatar;
            currentAnimator = catAnimator;
            currentAvatar = kuroAvatar;
        }

        if (catController != null)
        {
            Debug.Log("Enabling Kuro CatController.");
            catController.enabled = true;
            currentController = catController;
        }

        // Enable Kuro GameObject and disable Raijin GameObject
        if (kuroGameObject != null)
        {
            Debug.Log("Activating Kuro GameObject.");
            kuroGameObject.SetActive(true);
        }
        
        if (raijinGameObject != null)
        {
            Debug.Log("Deactivating Raijin GameObject.");
            raijinGameObject.SetActive(false);
        }

        // Switch CS Character Controller to Kuro settings
        if (csCharacterControllerAnimator != null)
        {
            Debug.Log("Updating Animator for Kuro.");
            csCharacterControllerAnimator.runtimeAnimatorController = catAnimatorController;
            csCharacterControllerAnimator.avatar = kuroAvatar;
        }

        // Disable CSPlayerController and enable CatController
        if (csPlayerController != null)
        {
            Debug.Log("Disabling CSPlayerController.");
            csPlayerController.enabled = false;
        }
        if (catController != null)
        {
            Debug.Log("Enabling CatController.");
            catController.enabled = true;
        }
    }

    private void SwitchToRaijin()
    {
        // Disable the current controller if one is active
        if (currentController != null)
        {
            Debug.Log("Disabling current controller.");
            currentController.enabled = false;
        }

        // Enable Raijin components and game object
        if (raijinAnimator != null)
        {
            Debug.Log("Enabling Raijin Animator.");
            raijinAnimator.enabled = true;
            raijinAnimator.avatar = raijinAvatar;
            currentAnimator = raijinAnimator;
            currentAvatar = raijinAvatar;
        }

        if (raijinController != null)
        {
            Debug.Log("Enabling Raijin Controller.");
            raijinController.enabled = true;
            currentController = raijinController;
        }

        // Enable Raijin GameObject and disable Kuro GameObject
        if (raijinGameObject != null)
        {
            Debug.Log("Activating Raijin GameObject.");
            raijinGameObject.SetActive(true);
        }

        if (kuroGameObject != null)
        {
            Debug.Log("Deactivating Kuro GameObject.");
            kuroGameObject.SetActive(false);
        }

        // Switch CS Character Controller to Raijin settings
        if (csCharacterControllerAnimator != null)
        {
            Debug.Log("Updating Animator for Raijin.");
            csCharacterControllerAnimator.runtimeAnimatorController = raijinAnimatorController;
            csCharacterControllerAnimator.avatar = raijinAvatar;
        }

        // Disable CatController and enable CSPlayerController
        if (catController != null)
        {
            Debug.Log("Disabling CatController.");
            catController.enabled = false;
        }

        if (csPlayerController != null)
        {
            Debug.Log("Enabling CSPlayerController.");
            csPlayerController.enabled = true;
        }
    }
}
