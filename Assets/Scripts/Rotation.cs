using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate : MonoBehaviour
{
    public float Speed = 10f;
    private bool isRotating = false;
    private bool reverseRotation = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Y))
        {
            isRotating = true;
            reverseRotation = false;
        }
        else if (Input.GetKeyDown(KeyCode.R))
        {
            isRotating = true;
            reverseRotation = true;
        }
        else if (Input.GetKeyDown(KeyCode.T))
        {
            isRotating = false;
        }

        if (isRotating)
        {
            // Rotate around the y-axis automatically
            float direction = reverseRotation ? -1 : 1;
            transform.Rotate(Vector3.up, direction * Speed * Time.deltaTime);
        }
    }
}
