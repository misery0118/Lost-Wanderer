using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grabber : MonoBehaviour
{
    private GameObject selectedObject;
    private Vector3 initialPosition;
    public GameObject[] quadsToSort; // Quads to sort
    public Transform[] gridCells; // Grid cells to snap the Quads to

    [SerializeField]
    private float dragHeight = 1.0f; // New field to set the height at which quads are dragged

    private Dictionary<int, GameObject> occupiedGridCells = new Dictionary<int, GameObject>(); // Keeps track of occupied grid cells

    private void Start()
    {
        // Ensure that quadsToSort are set properly
        Debug.Log("Initialization complete. Quads to sort: " + quadsToSort.Length);
    }

    private void Update()
    {
        // Check if the game is paused before allowing object interaction
        if (!PauseMenu.GameIsPaused)
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

                        // Unmark the previous cell if applicable
                        if (selectedObject.TryGetComponent<QuadController>(out QuadController quadController))
                        {
                            int previousCellIndex = quadController.AssignedCellIndex;
                            if (occupiedGridCells.ContainsKey(previousCellIndex) && occupiedGridCells[previousCellIndex] == selectedObject)
                            {
                                occupiedGridCells.Remove(previousCellIndex);
                            }
                        }
                    }
                }
                else
                {
                    // Drop the selected object
                    if (selectedObject.CompareTag("drag"))
                    {
                        SnapToGrid(selectedObject);
                    }
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

                // Use the dragHeight to set the y position
                selectedObject.transform.position = new Vector3(worldPosition.x, dragHeight, worldPosition.z);

                if (Input.GetMouseButtonDown(1))
                {
                    selectedObject.transform.rotation = Quaternion.Euler(new Vector3(
                        selectedObject.transform.rotation.eulerAngles.x,
                        selectedObject.transform.rotation.eulerAngles.y + 90f,
                        selectedObject.transform.rotation.eulerAngles.z));
                }
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
        int closestCellIndex = -1;

        for (int i = 0; i < gridCells.Length; i++)
        {
            float distance = Vector3.Distance(obj.transform.position, gridCells[i].position);
            if (distance < minDistance && !occupiedGridCells.ContainsKey(i))
            {
                minDistance = distance;
                closestCell = gridCells[i];
                closestCellIndex = i;
            }
        }

        // Snap the object to the closest grid cell
        if (closestCell != null)
        {
            Vector3 newPosition = closestCell.position;
            newPosition.x += 0.01f; // Adjust the X position to ensure the Quad is on top
            newPosition.y -= 0.01f; // Set the Y position to the desired value
            newPosition.z -= 0f; // Adjust the Z position to bring the Quad in front of the grid cell
            obj.transform.position = newPosition;

            if (obj.TryGetComponent<QuadController>(out QuadController quadController))
            {
                quadController.AssignedCellIndex = closestCellIndex; // Assign the grid cell index to the quad
                occupiedGridCells[closestCellIndex] = obj; // Mark the cell as occupied
                Debug.Log("Snapped " + obj.name + " to " + closestCell.name);
            }
        }
    }

    private void CheckIfSorted()
    {
        // Check if all Quads are in their sorted positions
        bool allSorted = true;

        for (int i = 0; i < quadsToSort.Length; i++)
        {
            QuadController quadController = quadsToSort[i].GetComponent<QuadController>();
            if (quadController.AssignedCellIndex != i)
            {
                allSorted = false;
                Debug.Log("Quad " + quadsToSort[i].name + " is not in the correct position.");
                break; // If one Quad is out of place, no need to check further
            }
        }

        if (allSorted)
        {
            Debug.Log("Puzzle Complete! All Quads are sorted.");
        }
    }
}
