using UnityEngine;

public class CameraDesactivation : MonoBehaviour
{
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Ground") == false && other.gameObject.CompareTag("PicturePoint") == false)
        {
            GameObject.Find("CameraToTake").GetComponent<Camera>().enabled = false;

            if (GameObject.Find("ObjectGrabed").GetComponent<CameraBehaviour>() != null)
            {
                GameObject.Find("ObjectGrabed").GetComponent<CameraBehaviour>()._canFullCamera = false;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
            if (GameObject.Find("ObjectGrabed").GetComponent<CameraBehaviour>() != null)
            {
                GameObject.Find("ObjectGrabed").GetComponent<CameraBehaviour>()._canFullCamera = true;
            }
    }
}
