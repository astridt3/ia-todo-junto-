using UnityEngine;

public class Billboard : MonoBehaviour
{
    private Camera mainCamera;

    void Start()
    {
        mainCamera = Camera.main;
    }

    void LateUpdate()
    {
        if (mainCamera == null)
        {
            mainCamera = Camera.main;
            if (mainCamera == null) return;
        }

        // Hace que el objeto mire hacia la cámara pero manteniendo el 'up' vertical
        Vector3 lookPos = mainCamera.transform.position - transform.position;
        lookPos.y = 0; // mantiene el objeto vertical (no inclinar hacia arriba/abajo)
        if (lookPos.sqrMagnitude > 0.0001f)
        {
            transform.rotation = Quaternion.LookRotation(lookPos);
        }
    }
}
