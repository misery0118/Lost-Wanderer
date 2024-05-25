using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grabber : MonoBehaviour
{
    private GameObject selectedObject;
    private Vector3 initialPosition;
    public GameObject[] objectsToSort; // Objects to sort
    public Transform[] gridCells; // Grid cells to snap the Quads to

    private void Start()
    {
        // Ensure that sortedPositions are set properly
        Debug.Log("Initialization complete. Objects to sort: " + objectsToSort.Length);
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (selectedObject == null)
            {
                RaycastHit hit = CastRay();

                if (hit.collider != null)
                {
                    if (!hit.collider.CompareTag("drag"))
                    {
                        Debug.Log("Hit object is not draggable");
                        return;
                    }

                    selectedObject = hit.collider.gameObject;
                    initialPosition = selectedObject.transform.position;
                    Cursor.visible = false;
                    Debug.Log("Selected object: " + selectedObject.name);
                }
            }
            else
            {
                // Drop the selected object
                SnapToGrid(selectedObject);
                selectedObject = null;
                Cursor.visible = true;

                // Check if objects are sorted
                CheckIfSorted();
            }
        }

        if (selectedObject != null)
        {
            Vector3 position = new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.WorldToScreenPoint(selectedObject.transform.position).z);
            Vector3 worldPosition = Camera.main.ScreenToWorldPoint(position);
            selectedObject.transform.position = new Vector3(worldPosition.x, 1.2f, worldPosition.z);

            if (Input.GetMouseButtonDown(1))
            {
                selectedObject.transform.rotation = Quaternion.Euler(new Vector3(
                    selectedObject.transform.rotation.eulerAngles.x,
                    selectedObject.transform.rotation.eulerAngles.y + 90f,
                    selectedObject.transform.rotation.eulerAngles.z));
            }
        }
    }

    private RaycastHit CastRay()
    {
        Vector3 screenMousePosFar = new Vector3(
            Input.mousePosition.x,
            Input.mousePosition.y,
            Camera.main.farClipPlane);
        Vector3 screenMousePosNear = new Vector3(
            Input.mousePosition.x,
            Input.mousePosition.y,
            Camera.main.nearClipPlane);
        Vector3 worldMousePosFar = Camera.main.ScreenToWorldPoint(screenMousePosFar);
        Vector3 worldMousePosNear = Camera.main.ScreenToWorldPoint(screenMousePosNear);
        RaycastHit hit;
        Physics.Raycast(worldMousePosNear, worldMousePosFar - worldMousePosNear, out hit);

        return hit;
    }

    private void SnapToGrid(GameObject obj)
    {
        // Find the closest grid cell to snap the object to
        float minDistance = Mathf.Infinity;
        Transform closestCell = null;

        foreach (var cell in gridCells)
        {
            float distance = Vector3.Distance(obj.transform.position, cell.position);
            if (distance < minDistance)
            {
                minDistance = distance;
                closestCell = cell;
            }
        }

        // Snap the object to the closest grid cell
        if (closestCell != null)
        {
            Vector3 newPosition = closestCell.position;
            newPosition.x += 0.01f; // Set the exact X position to the desired value
            newPosition.y += 0f; // Adjust the Y position to ensure the Quad is on top
            obj.transform.position = newPosition;
            Debug.Log("Snapped " + obj.name + " to " + closestCell.name);
        }
    }

    private void CheckIfSorted()
    {
        // Check if all objects are in their sorted positions
        bool allSorted = true;

        for (int i = 0; i < objectsToSort.Length; i++)
        {
            Vector3 objectPosition = objectsToSort[i].transform.position;
            Vector3 gridCellPosition = gridCells[i].position;

            // Set the exact expected X and Y position
            gridCellPosition.x += 0.01f;
            gridCellPosition.y += 1.2f;

            if (Vector3.Distance(objectPosition, gridCellPosition) > 0.01f)
            {
                allSorted = false;
                Debug.Log("Object " + objectsToSort[i].name + " is not in the correct position.");
                break; // If one object is out of place, no need to check further
            }
        }

        if (allSorted)
        {
            Debug.Log("Puzzle Complete! All objects are sorted.");
        }
    }
}