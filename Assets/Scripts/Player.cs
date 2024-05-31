using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    [SerializeField]
    private LayerMask pickableLayerMask;

    [SerializeField]
    private LayerMask laserLayerMask; // New layer mask for lasers

    [SerializeField]
    private Transform playerCameraTransform;

    [SerializeField]
    private GameObject pickUpUI;

    [SerializeField]
    private GameObject RotatePanel;

    internal void AddHealth(int healthBoost)
    {
        Debug.Log($"Health boosted by {healthBoost}");
    }

    [SerializeField]
    [Min(1)]
    private float hitRange = 3;

    [SerializeField]
    private Transform pickUpParent;

    [SerializeField]
    private GameObject inHandItem;

    [SerializeField]
    private InputActionReference interactionInput, dropInput, useInput, Rotate;

    private RaycastHit hit;

    [SerializeField]
    private AudioSource pickUpSource;

    private int originalLayer; // To store the original layer of the picked-up object

    private void Start()
    {
        interactionInput.action.performed += PickUp;
        dropInput.action.performed += Drop;
        useInput.action.performed += Use;
        Rotate.action.performed += Rotation;
    }

    private void Rotation(InputAction.CallbackContext obj)
    {
        if (PauseMenu.GameIsPaused)
            return;

        if (hit.collider != null && hit.collider.GetComponent<Pointer>())
        {
            Vector2 rotationInput = obj.ReadValue<Vector2>();
            float rotationSpeed = 50f;

            Debug.Log("Dapat iikot");
            hit.collider.transform.Rotate(Vector3.up, rotationInput.y * rotationSpeed * Time.deltaTime, Space.World);
        }
    }

    private void Use(InputAction.CallbackContext obj)
    {
        if (PauseMenu.GameIsPaused)
            return;

        if (inHandItem != null)
        {
            IUsable usable = inHandItem.GetComponent<IUsable>();
            if (usable != null)
            {
                usable.Use(this.gameObject);
            }
        }
    }

    private void Drop(InputAction.CallbackContext obj)
    {
        if (PauseMenu.GameIsPaused)
            return;

        if (inHandItem != null)
        {
            // Reset layer to original
            inHandItem.layer = originalLayer;

            inHandItem.transform.SetParent(null);
            Rigidbody rb = inHandItem.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.isKinematic = false;
            }
            inHandItem = null;
        }
    }

    private void PickUp(InputAction.CallbackContext obj)
    {
        if (PauseMenu.GameIsPaused)
            return;

        if (hit.collider != null && inHandItem == null)
        {
            IPickable pickableItem = hit.collider.GetComponent<IPickable>();
            if (pickableItem != null)
            {
                pickUpSource.Play();
                inHandItem = pickableItem.PickUp();
                inHandItem.transform.SetParent(pickUpParent.transform, pickableItem.KeepWorldPosition);
                originalLayer = inHandItem.layer; // Store the original layer
                inHandItem.layer = LayerMask.NameToLayer("PickedUpObject");
            }

            Debug.Log(hit.collider.name);
            Rigidbody rb = hit.collider.GetComponent<Rigidbody>();
            if (hit.collider.GetComponent<Food>() || hit.collider.GetComponent<Weapon>())
            {
                Debug.Log("It's food or a weapon!");
                inHandItem = hit.collider.gameObject;
                inHandItem.transform.position = pickUpParent.position;
                inHandItem.transform.rotation = Quaternion.identity;
                inHandItem.transform.SetParent(pickUpParent.transform, false);
                if (rb != null)
                {
                    rb.isKinematic = true;
                }
                originalLayer = inHandItem.layer;
                inHandItem.layer = LayerMask.NameToLayer("PickedUpObject");
                return;
            }
            if (hit.collider.GetComponent<Item>())
            {
                Debug.Log("It's a useless item!");
                inHandItem = hit.collider.gameObject;
                inHandItem.transform.SetParent(pickUpParent.transform, true);
                if (rb != null)
                {
                    rb.isKinematic = true;
                }
                originalLayer = inHandItem.layer;
                inHandItem.layer = LayerMask.NameToLayer("PickedUpObject");
                return;
            }
        }
    }

    private void Update()
    {
        if (PauseMenu.GameIsPaused)
            return;

        Debug.DrawRay(playerCameraTransform.position, playerCameraTransform.forward * hitRange, Color.red);
        if (hit.collider != null)
        {
            hit.collider.GetComponent<Highlight>()?.ToggleOutline(false);
            pickUpUI.SetActive(false);
            RotatePanel.SetActive(false);
        }

        if (inHandItem != null)
        {
            return;
        }

        if (Physics.Raycast(playerCameraTransform.position, playerCameraTransform.forward, out hit, hitRange, pickableLayerMask))
        {
            hit.collider.GetComponent<Highlight>()?.ToggleOutline(true);
            pickUpUI.SetActive(true);
        }
        else if (Physics.Raycast(playerCameraTransform.position, playerCameraTransform.forward, out hit, hitRange, laserLayerMask))
        {
            hit.collider.GetComponent<Highlight>()?.ToggleOutline(true);
            RotatePanel.SetActive(true);
        }
    }
}
