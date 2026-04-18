using UnityEngine;

public class LadderController : MonoBehaviour
{
    private Rigidbody rb;
    bool inside = false;
    public float speedUpDown = 3f;
    private Movement movementInput;
    void Start()
    {
        movementInput = FindAnyObjectByType<Movement>();
        rb = movementInput.GetComponent<Rigidbody>();

        inside = false;
    }

    void Update()
    {
        if (inside)
        {
            rb.useGravity = false;
            if (Input.GetKey(KeyCode.W))
            {
                rb.MovePosition(rb.position + Vector3.up * (speedUpDown * Time.deltaTime) + (rb.transform.forward * Time.deltaTime));
                Debug.Log("Subiendo");
            }
            if (Input.GetKey(KeyCode.S))
            {
                rb.MovePosition(rb.position + Vector3.down * (speedUpDown * Time.deltaTime) - (rb.transform.forward * Time.deltaTime));
                Debug.Log("Bajand0o");
            }
        }
    }

    private void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "Player")
        {
            if (rb.transform.position.y < 33)
            {
                rb.transform.rotation = new Quaternion(0, 0, 0, 0);
            }
            inside = true;
            Debug.Log("Inside Ladder" + inside);
        }
    }

    private void OnTriggerExit(Collider col)
    {
        if (col.gameObject.tag == "Player")
        { 
            inside = false;
            rb.useGravity = true;
            Debug.Log("Inside Ladder" + inside);
        }
    }
}
