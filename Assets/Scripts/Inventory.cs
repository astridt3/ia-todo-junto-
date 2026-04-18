using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.XR;

public class Inventory : MonoBehaviour
{
    //private List <GameObject> gameObjectsToAchieve = new List <GameObject> (); //La lista de objetos que se llevar�n, ya sea en la mano o en el "Inventario" (Inventario = Objetos que lleva el jugador y no ve)
    private Dictionary<int, GameObject> gameObjectsToAchieve = new Dictionary<int, GameObject>();

    public Dictionary<int, GameObject> _gameObjectsToAchieve { get { return gameObjectsToAchieve; } set { gameObjectsToAchieve = value; } }

    private GameObject objectInHand;
    private GameObject objectInLeftHand;

    private GameObject leftHand;

    private int numberOfInventory; //El n�mero de index al que se acceder� al inventario
    private ITypeOFObjectInterface typeOFObject;
    private bool hasAnObject;

    private float scaleX = 0.5f;
    private float scaleY = 0.5f;
    private float scaleZ = 0.5f;

    private bool isDropingAnbject;

    private KeyBehaviour keyPlayerContainer;
    private GameObject ObjectDroped;

    private bool canUseInv;

    void Start()
    {
        objectInHand = GameObject.Find("ObjectGrabed"); //Inventario del jugador
        objectInLeftHand = GameObject.Find("LeftHand");
        hasAnObject = false; //Para no dropear un objeto con un ID nulo
        isDropingAnbject = false;

        leftHand = GameObject.Find("LeftHand");
        keyPlayerContainer = GameObject.Find("KeyContainer").GetComponent<KeyBehaviour>();
        keyPlayerContainer._keyPropetys = new KeysObjects();

        canUseInv = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (canUseInv == true)
        {
            if (Input.GetKeyDown(KeyCode.Q) && hasAnObject == true)
            {
                DropObject(numberOfInventory);
            }

            if (Input.GetKeyDown(KeyCode.F) && objectInLeftHand != null)
            {
                DropObjectInLeftHand(numberOfInventory);
            }

            MoveIntoInventary();

            if (Input.GetMouseButtonDown(0))
            {
                UseItem();
            }
        }

    }

    private void MoveIntoInventary()
    {
            if (gameObjectsToAchieve.Count > 0 && isDropingAnbject == false)
            {
                if (Input.mouseScrollDelta.y > 0 || Input.GetKeyDown(KeyCode.KeypadPlus)) //Cuando se sube la ruedita del mouse
                {
                    if (numberOfInventory < gameObjectsToAchieve.Keys.Last())
                    {
                        numberOfInventory++;
                        ChangeEqquiped(numberOfInventory);
                    }
                    else if (numberOfInventory == gameObjectsToAchieve.Keys.Last()) //Si se llega al �ltimo de la lista y se quiere subir
                    {
                        numberOfInventory = gameObjectsToAchieve.Keys.First(); //Te manda al primero
                        ChangeEqquiped(numberOfInventory);
                    }
                }

                if (Input.mouseScrollDelta.y < 0 || Input.GetKeyDown(KeyCode.KeypadMinus)) //Cuando se baja la ruedita del mouse
                {
                    if (numberOfInventory > 0)
                    {
                        numberOfInventory--;
                        ChangeEqquiped(numberOfInventory);
                    }
                    else if (numberOfInventory == 0) //Si se llega al primero de la lista y se quiere bajar
                    {
                        numberOfInventory = gameObjectsToAchieve.Count - 1; //Te manda al �ltimo
                        ChangeEqquiped(numberOfInventory);
                    }

                }
                //UseItem();
            }
    }

    private void ChangeEqquiped(int IDRequest)
    {
        if (gameObjectsToAchieve[IDRequest] != null)
        {
            gameObjectsToAchieve[IDRequest].SetActive(true);
            Vector3 positionToSave = objectInHand.transform.position;
            objectInHand.transform.localRotation = gameObjectsToAchieve[IDRequest].transform.localRotation;
            objectInHand.GetComponent<TypeAction>()._type = gameObjectsToAchieve[IDRequest].GetComponent<TypeAction>()._type;
            keyPlayerContainer._keyPropetys = new KeysObjects();

            if (objectInHand.GetComponent<TypeAction>()._type != "Camera" && objectInHand.GetComponent<TypeAction>()._type != "Key")
            {
                Vector3 defaultScale = new Vector3(1, 1, 1);
                if (gameObjectsToAchieve[IDRequest].transform.localScale == defaultScale) 
                {
                    Debug.Log(objectInHand.transform.localScale);
                    objectInHand.transform.localScale = new Vector3(scaleX, scaleY, scaleZ); 
                } else
                {
                    objectInHand.transform.localScale = gameObjectsToAchieve[IDRequest].transform.localScale;
                    Debug.Log("s");
                }
                if (objectInHand.GetComponent<CameraBehaviour>() != null)
                {
                    objectInHand.GetComponent<CameraBehaviour>().enabled = false;
                }
            }
            else if (objectInHand.GetComponent<TypeAction>()._type == "Camera")
            {
                if (objectInHand.GetComponent<CameraBehaviour>() == null)
                {
                    objectInHand.transform.localScale = gameObjectsToAchieve[IDRequest].transform.localScale;
                    
                    objectInHand.AddComponent<CameraBehaviour>();
                    CameraBehaviour cameraBehaviour = objectInHand.GetComponent<CameraBehaviour>();
                    
                    cameraBehaviour._photographyCamera = gameObjectsToAchieve[IDRequest].GetComponent<CameraBehaviour>()._photographyCamera;
                }
                else
                {
                    objectInHand.transform.localScale = gameObjectsToAchieve[IDRequest].transform.localScale;
                   // objectInHand.transform.localRotation = objectInHand.GetComponent<CameraBehaviour>()._rotationForHand.transform.rotation;
                    objectInHand.GetComponent<CameraBehaviour>().enabled = true;
                }
                Vector3 newDirection = new Vector3(0, 180, 0);
                objectInHand.transform.Rotate(newDirection);
            }
            else
            {
                keyPlayerContainer._keyPropetys = gameObjectsToAchieve[IDRequest].GetComponent<KeyBehaviour>()._keyPropetys;
                Debug.Log(keyPlayerContainer._keyPropetys._doorToOpen);
            }
            if (gameObjectsToAchieve[IDRequest].GetComponent<MeshFilter>() != null)
            {
                objectInHand.GetComponent<MeshFilter>().mesh = gameObjectsToAchieve[IDRequest].GetComponent<MeshFilter>().mesh;
                objectInHand.GetComponent<MeshRenderer>().materials = gameObjectsToAchieve[IDRequest].GetComponent<MeshRenderer>().materials;
            }
            objectInHand.transform.localScale = gameObjectsToAchieve[IDRequest].transform.localScale;
            objectInHand.transform.position = positionToSave;
            gameObjectsToAchieve[IDRequest].SetActive(false);

            hasAnObject = true;
        }
    }

    private void UseItem()
    {
        if (objectInHand.GetComponent<TypeAction>() != null)
        {
            objectInHand.GetComponent<TypeAction>().Action();
            if (objectInHand.GetComponent<TypeAction>()._type.Equals ("PictureFrame"))
            {
                PassObjectToLeftHand(numberOfInventory);
            }
        }
    }

    public void DropObject(int objectToDrop)
    {
        isDropingAnbject = true;

        Rigidbody rb = gameObjectsToAchieve[objectToDrop].GetComponent<Rigidbody>();

        gameObjectsToAchieve[objectToDrop].SetActive(true);
        gameObjectsToAchieve[objectToDrop].GetComponent<Rigidbody>().useGravity = true;
        gameObjectsToAchieve[objectToDrop].transform.position = transform.position;

        ObjectDroped = gameObjectsToAchieve[objectToDrop];

        if (rb != null)
        {
            rb.isKinematic = false;
            rb.useGravity = true;
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;

            gameObjectsToAchieve[objectToDrop].transform.position = (transform.forward * 1.3f)+ gameObjectsToAchieve[objectToDrop].transform.position ;
            gameObjectsToAchieve[objectToDrop].GetComponent<Rigidbody>().AddForce(transform.forward * 233);
        }

        if (objectInHand.GetComponent<CameraBehaviour>() != null)
        {
            objectInHand.GetComponent<CameraBehaviour>().enabled = false;
        }

        objectInHand.GetComponent<MeshFilter>().mesh = null;
        objectInHand.GetComponent<MeshRenderer>().material = null;
        objectInHand.GetComponent<TypeAction>()._type = "Nothing";
        gameObjectsToAchieve.Remove(objectToDrop);

        hasAnObject = false;

        if (gameObjectsToAchieve.Count >= 1)
        {
            SortingInventory();
        }

        isDropingAnbject = false;
    }

    private void DropObjectInLeftHand(int objectToDrop)
    {
        if (objectInLeftHand.GetComponent<TypeAction>()._type != "Nothing")
        {
            if (objectInLeftHand.GetComponent<TypeAction>()._type == "PictureFrame")
            {
                if (objectInHand.GetComponent<TypeAction>()._type == "Camera")
                {
                    objectInHand.GetComponent<CameraBehaviour>()._stateOfCamera = 0;
                }
            }
            objectInLeftHand.SetActive(true);
            objectInLeftHand.GetComponent<MeshRenderer>().material = leftHand.GetComponent<MeshRenderer>().material;
            Rigidbody rb = objectInLeftHand.GetComponent<Rigidbody>();
            objectInLeftHand.GetComponent<Rigidbody>().useGravity = true;
            objectInLeftHand.GetComponent<TypeAction>()._type = leftHand.GetComponent<TypeAction>()._type;
            objectInLeftHand.transform.position = transform.position;

            if (objectInHand.GetComponent<CameraBehaviour>() != null)
            {
                objectInLeftHand.name = objectInHand.GetComponent<CameraBehaviour>()._nameOfThePicture;
            }

            if (rb != null)
            {
                rb.isKinematic = false;
                rb.useGravity = true;
                rb.linearVelocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;


                objectInLeftHand.transform.position = objectInLeftHand.transform.position + transform.forward * 1f;
            }

            leftHand.GetComponent<MeshFilter>().mesh = null;
            leftHand.GetComponent<MeshRenderer>().material = null;
            leftHand.GetComponent<TypeAction>()._type = "Nothing";


            if (gameObjectsToAchieve.Count >= 1)
            {
                SortingInventory();
            }

            objectInLeftHand = null;

            GameObject.Find("Main Camera").GetComponent<Interactor>().ReturnNormalCross();
        }
    }

    private void PassObjectToLeftHand(int IDToPass)
    {
        objectInHand.GetComponent<MeshFilter>().mesh = null;
        objectInHand.GetComponent<MeshRenderer>().material = null;
        objectInHand.GetComponent<TypeAction>()._type = "Nothing";
        objectInLeftHand = gameObjectsToAchieve[IDToPass];
        gameObjectsToAchieve.Remove(IDToPass);

        hasAnObject = false;

        if (gameObjectsToAchieve.Count >= 1)
        {
            SortingInventory();
        }
    }

    private bool SortingInventory()
    {
        for (int i = 0; i < gameObjectsToAchieve.Keys.Last(); i++)
        {
            if (gameObjectsToAchieve.ContainsKey(i) == false)
            {
                for (int j = i; j < gameObjectsToAchieve.Keys.Last(); j++)
                {
                    if (j == i)
                    {
                        gameObjectsToAchieve.Add(i, gameObjectsToAchieve[i + 1]);
                    }
                    else if (j != gameObjectsToAchieve.Keys.Last() - 1)
                    {
                        gameObjectsToAchieve[j] = gameObjectsToAchieve[j + 1];
                    }
                    else
                    {
                        gameObjectsToAchieve[j] = gameObjectsToAchieve[gameObjectsToAchieve.Keys.Last()];
                    }
                }

                gameObjectsToAchieve.Remove(gameObjectsToAchieve.Keys.Last());
                return true;
            }
        }

        return false;
    }

    public void DestroyDroppedObject()
    {
        DropObject(numberOfInventory);
        Destroy(ObjectDroped);
    }

    public void AutoEquipItem()
    {
        ChangeEqquiped(0);
    }

    public void SetMove()
    {
        if (canUseInv == false)
        {
            canUseInv = true;
        }
        else 
        {
            canUseInv = false;
        }
    }

    public void AddNewObject(GameObject gameObjectGrabed)
    {
        Debug.Log("done");
        for (int i = 0; i < gameObjectsToAchieve.Keys.Last() + 1; i++)
        {
            if (gameObjectsToAchieve.ContainsKey(i) == false)
            {
                gameObjectsToAchieve.Add(i, gameObjectGrabed);
            }
            gameObjectsToAchieve.Add(gameObjectsToAchieve.Count, gameObjectGrabed);
            //gameObjectToGrab.transform.localScale = new Vector3 (scaleX, scaleY, scaleZ);
            gameObjectGrabed.GetComponent<Rigidbody>().useGravity = false;
            gameObjectGrabed.SetActive(false);
            gameObjectGrabed = null;
        }
    }
}

