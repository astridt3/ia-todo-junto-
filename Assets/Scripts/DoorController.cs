using System;
using System.Collections;
using UnityEngine;

[System.Serializable]
public class DoorController : MonoBehaviour, IInteractable
{
    public GameObject door;
    public float openRot, closeRot, speed;
    private bool isClose = true;
    private BoxCollider colliderWhenClose;

    public bool _isClose { set { isClose = value; } }

    private void Start()
    {
        colliderWhenClose = GetComponent<BoxCollider>();
    }

    void Update()
    {
        Vector3 currentRot = door.transform.localEulerAngles;
        if (isClose)
        {
            if (currentRot.y > closeRot)
            {
                door.transform.localEulerAngles = Vector3.Lerp(currentRot, new Vector3(currentRot.x, closeRot, currentRot.z), speed * Time.deltaTime);
            }
        }
        else
        {
            if (currentRot.y < openRot)
            {
                door.transform.localEulerAngles = Vector3.Lerp(currentRot, new Vector3(currentRot.x, openRot, currentRot.z), speed * Time.deltaTime);
            }
        }

    }
    public void Interact()
    {
        isClose = !isClose;

        if (isClose == true)
        {
            colliderWhenClose.enabled = true;
        }
        else
        {
            colliderWhenClose.enabled = false;
        }
    }

}

