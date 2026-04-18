using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

interface IInteractable
{
    void Interact(); // esto nos sirve en caso de querer usar la interface
                     // si no chau
}

public class Interactor : MonoBehaviour
{
    private Transform interactorSource;
    public float interactionRange = 33;
    private Camera mainCamera;
    [SerializeField] private GameObject interactionUI;
    private float vectorValue = 0.6f;

    private Movement playerMovement;
    [SerializeField] private Transform holdPoint;
    [SerializeField] private TextMeshProUGUI interactionText;


    private GameObject currentTarget;
    private GameObject heldObject;

    private Image cross;
    [SerializeField] private Sprite normalCross;
    [SerializeField] private Sprite crossSelected;

    private void Start()
    {
        mainCamera = Camera.main;
        interactorSource = GetComponent<Transform>();
        if (interactionUI != null) interactionUI.SetActive(false);

        playerMovement = GetComponentInParent<Movement>();


        if (holdPoint == null) holdPoint = this.transform;


        if (interactionText == null && interactionUI != null)
        {
            interactionText = interactionUI.GetComponentInChildren<TextMeshProUGUI>();
        }

        cross = GameObject.Find("Cross").GetComponent<Image>();
    }

    void Update()
    {

        Ray ray = new Ray(interactorSource.position, interactorSource.forward);
        if (Physics.Raycast(ray, out RaycastHit hitInfo, interactionRange))
        {
            GameObject hitObj = hitInfo.collider.gameObject;
            if (hitInfo.collider.CompareTag("Grabable"))
            {
                if (interactionUI != null)
                {
                    interactionUI.transform.position = hitInfo.collider.transform.position + new Vector3(0, vectorValue, 0);
                    interactionUI.SetActive(true);
                }
                currentTarget = hitInfo.collider.gameObject;
                playerMovement._gameObjectToGrab = currentTarget;

                var ta = currentTarget.GetComponent<TypeAction>();
                string typeName = (ta != null) ? ta._type : "Objeto";
                if (interactionText != null)
                {
                    interactionText.text = $"Press [E] to grab the object";
                    cross.sprite = crossSelected;
                }
                if (hitInfo.collider.gameObject.TryGetComponent(out IInteractable interactObj))
                {
                    interactObj.Interact();
                }
            }
            else
            {
                HideInteraction();
                cross.sprite = normalCross;
            }
            if (hitInfo.collider.CompareTag("Door") && Input.GetKeyDown(KeyCode.Mouse0))
            {
                Debug.Log("Mira una puerta");
                // Buscar la interfaz en el objeto o en su padre
                IInteractable interactable = hitObj.GetComponent<IInteractable>() ?? hitObj.GetComponentInParent<IInteractable>();
                if (interactable != null && GameObject.Find("KeyContainer").GetComponentInChildren<KeyBehaviour>()._keyPropetys != null)
                {
                    if (hitObj.gameObject.name == GameObject.Find("KeyContainer").GetComponentInChildren<KeyBehaviour>()._keyPropetys._doorToOpen)
                    {
                        interactable.Interact();
                        GameObject.Find("SoundManager").GetComponent<SoundManager>().ReproduceSound(5);
                        GameObject.Find("KeyContainer").GetComponentInChildren<KeyBehaviour>().DesUseKey();
                    }
                }
            }
            if (hitInfo.collider.CompareTag("UnlockedDoor") && Input.GetKeyDown(KeyCode.Mouse0))
            {
                Debug.Log("Mira una puerta desbloqueada");
                // Buscar la interfaz en el objeto o en su padre
                IInteractable interactable = hitObj.GetComponent<IInteractable>() ?? hitObj.GetComponentInParent<IInteractable>();
                if (interactable != null)
                {
                    interactable.Interact();
                }
            }
        }
        else
        {
            HideInteraction();
        }

        //if (Input.GetKeyDown(KeyCode.E))
        //{
        //    if (heldObject == null)
        //    {

        //        if (currentTarget != null)
        //        {
        //            GrabObject(currentTarget);

        //            if (playerMovement != null) playerMovement._gameObjectToGrab = null;
        //        }
        //    }
        //    else
        //    {

        //        DropHeldObject();
        //    }
        //}
    }

    private void HideInteraction()
    {
        if (interactionUI != null) interactionUI.SetActive(false);
        currentTarget = null;
        if (playerMovement != null) playerMovement._gameObjectToGrab = null;
    }

    public void ReturnNormalCross()
    {
        cross.sprite = normalCross;
    }

    private void GrabObject(GameObject obj)
    {
        if (obj == null) return;


        if (interactionUI != null) interactionUI.SetActive(false);


        heldObject = obj;


        if (heldObject.TryGetComponent<Rigidbody>(out Rigidbody rb))
        {
            rb.isKinematic = true;
            rb.useGravity = false;
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }


        heldObject.transform.SetParent(holdPoint, true);
        heldObject.transform.position = holdPoint.position;
        heldObject.transform.rotation = holdPoint.rotation;


        var ta = heldObject.GetComponent<TypeAction>();
       
    }

    private void DropHeldObject()
    {
        if (heldObject == null) return;


        if (heldObject.TryGetComponent<Rigidbody>(out Rigidbody rb))
        {
            rb.isKinematic = false;
            rb.useGravity = true;


            rb.AddForce(interactorSource.forward * 1.5f, ForceMode.Impulse);
        }


        heldObject.transform.SetParent(null, true);


        heldObject = null;
    }
}
