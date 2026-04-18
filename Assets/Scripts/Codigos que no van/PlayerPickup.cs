using UnityEngine;

public class PlayerPickup : MonoBehaviour
{
    public Transform holdPoint;   
    private GameObject heldObject;
    private Collider playerCollider;

    void Start()
    {
        
        playerCollider = GetComponent<Collider>();
    }

    void Update()
    {
        //if (Input.GetKeyDown(KeyCode.E))
        //{
        //    if (heldObject != null)
        //    {
        //        DropItem();
        //    }
        //}
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (heldObject == null && collision.gameObject.CompareTag("Pickup"))
        {
            PickUpItem(collision.gameObject);
            collision.gameObject.GetComponent<Action>()?.DoAction();
        }
    }

    void PickUpItem(GameObject item)
    {
        heldObject = item;
        heldObject.transform.SetParent(holdPoint);
        heldObject.transform.localPosition = Vector3.zero;

        Rigidbody rb = heldObject.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = true;
            rb.useGravity = false;
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }

        
        Collider itemCol = heldObject.GetComponent<Collider>();

        if (itemCol != null && playerCollider != null)
        {
            Physics.IgnoreCollision(itemCol, playerCollider, true);
        }
    }

    void DropItem()
    {
        Rigidbody rb = heldObject.GetComponent<Rigidbody>();
        Collider itemCol = heldObject.GetComponent<Collider>();

        heldObject.transform.SetParent(null);

        if (rb != null)
        {
            rb.isKinematic = false;
            rb.useGravity = true;
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;

            
            heldObject.transform.position = holdPoint.position + transform.forward * 1f;
        }

        
        if (itemCol != null && playerCollider != null)
        {
            Physics.IgnoreCollision(itemCol, playerCollider, false);
        }

        heldObject = null;
    }
}
