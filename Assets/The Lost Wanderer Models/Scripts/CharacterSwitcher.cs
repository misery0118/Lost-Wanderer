using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;

public class CharacterSwitcher : MonoBehaviour
{
    public GameObject cat; // Reference to the Cat GameObject
    public GameObject player; // Reference to the Player GameObject
    public CinemachineStateDrivenCamera stateDrivenCamera; // Reference to the Cinemachine State Driven Camera

    private InputAction kuroswitchAction;
    private InputAction raijinswitchAction;
    private Transform currentTarget;

    // Public fields to assign the specific camera follow setups in the Inspector
    public Transform defaultFollowTransform;
    public Transform crouchFollowTransform;
    public Transform rollFollowTransform;
    public Transform crawlFollowTransform;
    public Transform zoomFollowTransform;
    public Transform climbFollowTransform;

    private Dictionary<CinemachineVirtualCamera, Vector3> originalCameraYPositions = new Dictionary<CinemachineVirtualCamera, Vector3>();

    void Start()
    {
        var playerInput = FindObjectOfType<PlayerInput>();
        if (playerInput == null)
        {
            Debug.LogError("PlayerInput component not found.");
            return;
        }

        kuroswitchAction = playerInput.actions["KuroSwitch"];
        raijinswitchAction = playerInput.actions["RaijinSwitch"];
        if (kuroswitchAction == null || raijinswitchAction == null)
        {
            Debug.LogError("Input actions not set up correctly.");
            return;
        }

        // Subscribe to the performed event
        kuroswitchAction.performed += OnKuroSwitch;
        raijinswitchAction.performed += OnRaijinSwitch;

        // Cache the original Y positions of each virtual camera
        foreach (var vcam in stateDrivenCamera.GetComponentsInChildren<CinemachineVirtualCamera>())
        {
            originalCameraYPositions[vcam] = new Vector3(vcam.transform.position.x, vcam.transform.position.y, vcam.transform.position.z);
        }

        // Set initial state
        SwitchToPlayer(); // Assuming player is the default character
    }

    private void OnKuroSwitch(InputAction.CallbackContext context)
    {
        Debug.Log("KuroSwitch action performed.");
        SwitchToCat();
    }

    private void OnRaijinSwitch(InputAction.CallbackContext context)
    {
        Debug.Log("RaijinSwitch action performed.");
        SwitchToPlayer();
    }

    private void SwitchToPlayer()
    {
        Debug.Log("Switching to Player");
        player.SetActive(true);

        UpdateCameraFollow(player.transform);
    }

    private void SwitchToCat()
    {
        Debug.Log("Switching to Cat");
        cat.SetActive(true);

        UpdateCameraFollow(cat.transform);
    }

    private void UpdateCameraFollow(Transform target)
    {
        currentTarget = target;
        if (stateDrivenCamera != null)
        {
            foreach (var vcam in stateDrivenCamera.GetComponentsInChildren<CinemachineVirtualCamera>())
            {
                if (vcam == null)
                {
                    Debug.LogError("Virtual camera is null.");
                    continue;
                }

                // Set the camera's rotation to look at the target
                Vector3 directionToTarget = target.position - vcam.transform.position;
                directionToTarget.y = 0; // Keep the camera's vertical position fixed
                Quaternion targetRotation = Quaternion.LookRotation(directionToTarget);
                vcam.transform.rotation = targetRotation;

                // Set the camera's position based on the state
                if (vcam.name == "DefaultFollowCamera")
                {
                    vcam.transform.position = new Vector3(defaultFollowTransform.position.x, originalCameraYPositions[vcam].y, defaultFollowTransform.position.z);
                }
                else if (vcam.name == "CrouchFollowCamera")
                {
                    vcam.transform.position = new Vector3(crouchFollowTransform.position.x, originalCameraYPositions[vcam].y, crouchFollowTransform.position.z);
                }
                else if (vcam.name == "RollFollowCamera")
                {
                    vcam.transform.position = new Vector3(rollFollowTransform.position.x, originalCameraYPositions[vcam].y, rollFollowTransform.position.z);
                }
                else if (vcam.name == "CrawlFollowCamera")
                {
                    vcam.transform.position = new Vector3(crawlFollowTransform.position.x, originalCameraYPositions[vcam].y, crawlFollowTransform.position.z);
                }
                else if (vcam.name == "ZoomFollowCamera")
                {
                    vcam.transform.position = new Vector3(zoomFollowTransform.position.x, originalCameraYPositions[vcam].y, zoomFollowTransform.position.z);
                }
                else if (vcam.name == "ClimbFollowCamera")
                {
                    vcam.transform.position = new Vector3(climbFollowTransform.position.x, originalCameraYPositions[vcam].y, climbFollowTransform.position.z);
                }

                Debug.Log($"Updated camera: {vcam.name}, Position: {vcam.transform.position}, Rotation: {vcam.transform.rotation}");
            }
        }
        else
        {
            Debug.LogWarning("StateDrivenCamera is null.");
        }
    }

    void LateUpdate()
    {
        // Optionally update camera positions if needed
    }

    void OnDestroy()
    {
        // Unsubscribe from the event when the object is destroyed to avoid memory leaks
        kuroswitchAction.performed -= OnKuroSwitch;
        raijinswitchAction.performed -= OnRaijinSwitch;
    }
}
