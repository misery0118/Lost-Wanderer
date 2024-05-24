using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphereColorChanger : MonoBehaviour
{
    public int sphereIndex; // The index of this sphere in the sequence
    private bool isLocked; // Indicates if the sphere is locked in place
    private Vector3 lockedPosition; // The position where the sphere should be locked

    private Rigidbody rb;
    private Collider col;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        col = GetComponent<Collider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!isLocked && other.CompareTag("Cube"))
        {
            CubeColorChanger cube = other.GetComponent<CubeColorChanger>();
            GameManager.Instance.CubeTriggered(cube, this);
        }
    }

    public void LockPosition(Vector3 position)
    {
        if (!isLocked)
        {
            isLocked = true;
            rb.isKinematic = true; // Disable physics on the sphere
            col.enabled = false; // Disable the collider to prevent further interactions
            lockedPosition = position; // Store the position to lock to
            StartCoroutine(MoveToPosition(lockedPosition)); // Move smoothly to the locked position
        }
    }

    private IEnumerator MoveToPosition(Vector3 position)
    {
        while (Vector3.Distance(transform.position, position) > 0.01f)
        {
            transform.position = Vector3.Lerp(transform.position, position, Time.deltaTime * 10);
            yield return null;
        }
        transform.position = position; // Ensure final position is accurate
    }

    public void UnlockPosition()
    {
        if (isLocked)
        {
            isLocked = false;
            rb.isKinematic = false; // Enable physics on the sphere
            col.enabled = true; // Enable the collider
        }
    }
}