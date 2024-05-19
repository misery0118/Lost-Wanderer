using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate : MonoBehaviour
{
    public float Speed = 10f;
    private bool isRotating = false;
    private bool reverseRotation = false;

    public void StartRotation()
    {   
        Debug.Log("natatawag");
        isRotating = true;
    }

    public void StartReverseRotation()
    {
        isRotating = true;
        reverseRotation = true;
    }

    public void StopRotation()
    {
        isRotating = false;
    }

    void Update()
    {
        if (isRotating)
        {
            float direction = reverseRotation ? -1 : 1;
            transform.Rotate(Vector3.up, direction * Speed * Time.deltaTime);
        }
    }
}
