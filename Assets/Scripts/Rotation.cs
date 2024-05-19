using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate : MonoBehaviour
{
    public float Speed = 10f;

    private bool isRotating = false;

    private float startMousePositionX;
    private float startMousePositionY;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            isRotating = true;
            startMousePositionX = Input.mousePosition.x;
            startMousePositionY = Input.mousePosition.y;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            isRotating = false;
        }

        if (isRotating)
        {
            float currentMousePositionX = Input.mousePosition.x;
            float currentMousePositionY = Input.mousePosition.y;
            float mouseMovementX = currentMousePositionX - startMousePositionX;
            float mouseMovementY = currentMousePositionY - startMousePositionY;

            // Rotate around the y-axis based on horizontal mouse movement
            transform.Rotate(Vector3.up, -mouseMovementX * Speed * Time.deltaTime);

            // Rotate around the x-axis based on vertical mouse movement
            transform.Rotate(Vector3.right, mouseMovementY * Speed * Time.deltaTime);

            startMousePositionX = currentMousePositionX;
            startMousePositionY = currentMousePositionY;
        }
    }
}
