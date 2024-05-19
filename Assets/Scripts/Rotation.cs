using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate : MonoBehaviour
{

    public float Speed = 10f;

    private bool isRotating = false;

    private float startMousePosition;

void Start()
{
    
}

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            isRotating = true;
            startMousePosition = Input.mousePosition.x;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            isRotating = false;
        }

        if (isRotating)
        {
            float currentMousePosition = Input.mousePosition.x;
            float mouseMovement = currentMousePosition - startMousePosition;

            transform.Rotate(Vector3.up, -mouseMovement * Speed * Time.deltaTime);
            startMousePosition = currentMousePosition;
        }
    }
}
