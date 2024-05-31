using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    public Transform playerCamera;
    public float yoffset = 5f;
    // Start is called before the first frame update
    

    // Update is called once per frame
    void Update()
    {
        Vector3 newPosition = playerCamera.position;
        newPosition.y = yoffset;

        this.transform.position = newPosition;
    }
}
