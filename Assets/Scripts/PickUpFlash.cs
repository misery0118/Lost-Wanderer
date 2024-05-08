using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PickUpFlash : MonoBehaviour
{
    public GameObject PickUpText;
    public GameObject Flashlight;
    public Transform ItemHolder;
    void Start()
    {
        Flashlight.GetComponent<Rigidbody>().isKinematic = true;
        PickUpText.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKey(KeyCode.G))
        {
            Drop();
        }
    }

    void Drop()
    {
        ItemHolder.DetachChildren();
        Flashlight.transform.eulerAngles = new Vector3(Flashlight.transform.position.x, Flashlight.transform.position.z, Flashlight.transform.position.y);
        Flashlight.GetComponent<Rigidbody>().isKinematic = false;
        Flashlight.GetComponent<MeshCollider>().enabled = true;
    }

    void Equip()
    {
        Flashlight.GetComponent<Rigidbody>().isKinematic = true;

        Flashlight.transform.position = ItemHolder.transform.position;
        Flashlight.transform.rotation = ItemHolder.transform.rotation;

        Flashlight.GetComponent<MeshCollider>().enabled = false;

        Flashlight.transform.SetParent(ItemHolder);
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            PickUpText.SetActive(true);
            if(Input.GetKey(KeyCode.E))
            {
                this.gameObject.SetActive(false);
                Flashlight.SetActive(true);
                Equip();
                PickUpText.SetActive(false);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        PickUpText.SetActive(false);
    }
}