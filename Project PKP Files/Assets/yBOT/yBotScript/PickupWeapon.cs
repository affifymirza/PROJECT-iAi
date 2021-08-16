using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupWeapon : MonoBehaviour
{
    public Transform equipPosition;
    public float distance = 5f;
    GameObject currentweapon;
    GameObject wp;

    bool canGrab;

    private void Update()
    {
        CheckWeapon();

        if (canGrab)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                if (currentweapon != null)
                {
                    Drop();
                    
                }

                Pickup();

            }

            if (currentweapon != null)
            {
                if (Input.GetKeyDown(KeyCode.Q))
                {
                    Debug.Log("Drop la babi");
                    Drop();
                }
                   
            }
        }
    }

        private void CheckWeapon()
        {
            RaycastHit hit;

            if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, distance))
            {
                if (hit.transform.tag == "Grab")
                {
                    Debug.Log("AKU BOLEH GRAB LA BODO");
                    canGrab = true;
                    wp = hit.transform.gameObject;
                }
            }
            else
            {
                canGrab = false;
            }
        }

        private void Pickup()
        {
            currentweapon = wp;
            currentweapon.transform.position = equipPosition.position;
            currentweapon.transform.parent = equipPosition;
            currentweapon.transform.localEulerAngles = new Vector3(0f, 180f, 0f);
            currentweapon.GetComponent<Rigidbody>().isKinematic = true;
        }

        private void Drop()
        {
            currentweapon.transform.parent = null;
            currentweapon.GetComponent<Rigidbody>().isKinematic = false;
            currentweapon = null;
        }
    
}
