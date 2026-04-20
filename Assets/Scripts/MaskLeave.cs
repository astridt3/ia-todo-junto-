using UnityEngine;
using UnityEngine.UIElements;

public class MaskLeave : MonoBehaviour
{
    [SerializeField] private string maskLeaved;
    private GameObject keyToLeave;
    [SerializeField] private Light spotLight;
    private void Start()
    {
        keyToLeave = GameObject.Find("KeyExit");
        if (spotLight != null)
            spotLight.intensity = 0f;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name == maskLeaved)
        {
            GameObject maskLeavedName = GameObject.Find (maskLeaved);
            GameObject maskPosition = GameObject.Find ("MaskPosition");

            if (maskPosition != null)
            {
            maskLeavedName.transform.position = maskPosition.transform.position;
            maskLeavedName.transform.rotation = maskPosition.transform.rotation;

                Rigidbody rb = maskLeavedName.GetComponent<Rigidbody>();
                if (rb != null)
                {
                    rb.isKinematic = true;
                    rb.useGravity = false;
                    rb.linearVelocity = Vector3.zero;
                    rb.angularVelocity = Vector3.zero;
                }
                BoxCollider box = maskLeavedName.GetComponent<BoxCollider>();
                if (box != null)
                {
                    box.enabled = false;
                }
            }

            CreateKey();
            SpawnSpotLight();
        }
    }
    private void SpawnSpotLight()
    {
        if (spotLight != null)
        {
            spotLight.intensity = 5000f;
        }
    }
    private void CreateKey()
    {
        Vector3 newPosition = new Vector3(transform.position.x, transform.position.y, transform.position.z + Vector3.forward.z * 0.2f);
        Instantiate(keyToLeave, newPosition, transform.rotation);
        Destroy(this);
    }
}
