using System.Diagnostics;
using Unity.VisualScripting;
using UnityEngine;
using System.Collections;
public class Movement : MonoBehaviour
{
    private Camera playerCamera;
    private float moveSpeed = 6f;
    private float runSpeed = 12f;
    private float jumpForce = 7f;
    private float lookSpeed = 2f;
    private float lookXLimit = 75f;

    private bool canMove = true;

    private Rigidbody rb;
    private bool isGrounded;

    private GameObject gameObjectToGrab; //El gameObject que se agarra
    private Inventory inventory; //La clase de la interfaz, donde se le agregan los objetos agarrados
    private float rotationX = 0;

    private GameObject pointOfPictureEliminate;

    private GameObject menuObject;
    private bool menuActive;
    private bool canUseCamera;

    public bool _canMove { get { return canMove; } set{ canMove = value; } }
    public GameObject _gameObjectToGrab { get { return gameObjectToGrab; } set { gameObjectToGrab = value; } }
    public bool _menuActive { get { return menuActive; } set { menuActive = value; } }
    public bool _canUseCamera { get { return canUseCamera; } set { canUseCamera = value; } }

    private SoundManager soundManager;
    private ProceduralText corrutineRef;

    public GameObject balloonUI;
    public float growDuration = 1.5f;
    float count = 0f;
    float riseDuration = 1.2f;
    float riseAmount = 1000f;

    public void Awake()
    {
        playerCamera = Camera.main;
        menuObject = GameObject.Find("MenuObject");
    }


    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        inventory = GetComponent<Inventory>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        menuActive = false;

        soundManager = GameObject.Find("SoundManager").GetComponent<SoundManager>();

        corrutineRef = GetComponent<ProceduralText>();

        canUseCamera = true;
    }

    void Update()
    {
        MovementPlayer();
        Controls();
    }

    private void Controls()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }

        if (Input.GetKeyDown(KeyCode.E)) //Accion y lo que pasa cuando se agarra un objeto
        {
            EquipItem();
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            OpenMenu();
        }

        if (Input.GetKeyDown(KeyCode.L))
        {
            UnityEngine.Debug.Log("Forward " + transform.forward);
            UnityEngine.Debug.Log("Rotation " + ((transform.rotation.y) * 100));
        }
    }

    private void EquipItem()
    {
        if (gameObjectToGrab != null)
        {
            inventory._gameObjectsToAchieve.Add(inventory._gameObjectsToAchieve.Count, gameObjectToGrab);
            if (inventory._gameObjectsToAchieve.Count == 1)
            {
                inventory.AutoEquipItem();
            }

            if (gameObjectToGrab.GetComponent<TypeAction>()._type != "Nothing")
            {
                soundManager.ReproduceSound(0);
                corrutineRef.CallEventWihCorrutine(0);
            }
            gameObjectToGrab.GetComponent<Rigidbody>().useGravity = false;
            gameObjectToGrab.SetActive(false);
            gameObjectToGrab = null;
            GetComponentInChildren<Interactor>().ReturnNormalCross();
        }
    }

    private void MovementPlayer()
    {
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");

        Vector3 move = transform.right * moveX + transform.forward * moveZ;
        Vector3 velocity = new Vector3(move.x * moveSpeed, rb.linearVelocity.y, move.z * moveSpeed);
        rb.linearVelocity = velocity;
        float curSpeedX = canMove ? moveSpeed * Input.GetAxis("Vertical") : 0;
        float curSpeedY = canMove ? moveSpeed * Input.GetAxis("Horizontal") : 0;

        if (!canMove)
        {
            rb.linearVelocity = Vector3.zero;
            return;
        }

        if (Input.GetAxis("Vertical") != 0 || moveSpeed * Input.GetAxis("Horizontal") != 0)
        {
            soundManager.ReproduceSound(2);
        }


        if (canUseCamera == true && menuActive == false)
        {
            rotationX += -Input.GetAxis("Mouse Y") * lookSpeed;
            rotationX = Mathf.Clamp(rotationX, -lookXLimit, lookXLimit);
            playerCamera.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);
            transform.rotation *= Quaternion.Euler(0, Input.GetAxis("Mouse X") * lookSpeed, 0);    
        }
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Piston"))
        {
            StartCoroutine(PlayBalloonAndWin());
        }
    }
    IEnumerator PlayBalloonAndWin()
    {
        canMove = false;
        rb.linearVelocity = Vector3.zero;
        balloonUI.SetActive(true);
        balloonUI.transform.localScale = Vector3.zero;
        soundManager.ReproduceSound(4); 
        Vector3 startPos = balloonUI.transform.localPosition;
        while (count < growDuration)
        {
            count += Time.deltaTime;
            balloonUI.transform.localScale = Vector3.Lerp(Vector3.zero, Vector3.one * 30, count / growDuration);
            yield return null;
        }
        balloonUI.transform.localScale = Vector3.one * 30;

        count = 0f;
        while (count < riseDuration)
        {
            count += Time.deltaTime;
            balloonUI.transform.localPosition = Vector3.Lerp(startPos, startPos + Vector3.up * riseAmount, count / riseDuration);
            yield return null;
        }

        yield return new WaitForSeconds(1.5f);
        Singleton.instance.GoToWinScene();
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PicturePoint"))
        {
            if (GetComponentInChildren<CameraBehaviour>() != null)
            {
                pointOfPictureEliminate = other.gameObject;
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("PicturePoint"))
        {
            Vector2 rotationNeed = other.gameObject.GetComponent<RequestNeeded>()._rotationNeededX;
            Vector2 rotationUp = other.gameObject.GetComponent<RequestNeeded>()._rotationNeededY;
            string nameToPhoto = other.gameObject.GetComponent<RequestNeeded>()._nameRequest;

            if (GetComponentInChildren<CameraBehaviour>() != null && (Mathf.Abs(transform.rotation.y) * 100) > rotationNeed.x && (Mathf.Abs(transform.rotation.y) * 100) < rotationNeed.y) 
            {
               // if ((Mathf.Abs(transform.rotation.w) * 100)> rotationUp.x && (Mathf.Abs(transform.rotation.w)* 100)< rotationUp.y)
                //{
                    GetComponentInChildren<CameraBehaviour>()._stateOfCamera = 1;
                    GetComponentInChildren<CameraBehaviour>()._nameOfThePicture = nameToPhoto;
               // }

            }
            else if (GetComponentInChildren<CameraBehaviour>() != null)
            {
                GetComponentInChildren<CameraBehaviour>()._stateOfCamera = 0;
                GetComponentInChildren<CameraBehaviour>()._nameOfThePicture = string.Empty;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("PicturePoint"))
        {
            if (GetComponentInChildren<CameraBehaviour>() != null)
            {
                GetComponentInChildren<CameraBehaviour>()._stateOfCamera = 0;

            }
        }
    }

    public void DestroyLastPoint()
    {
        Destroy(pointOfPictureEliminate);
        pointOfPictureEliminate = null;
    }

    private void OpenMenu()
    {
        if (Time.timeScale == 0f)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            menuObject.SetActive(false);
            menuActive = false;
            Time.timeScale = 1f;
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            menuObject.SetActive(true);
            menuActive = true;
            Time.timeScale = 0f;
        }
    }

    public void LoadPlayerState()
    {
        transform.position = new Vector3(SaveManager.LoadDataWithJson()._positionX, SaveManager.LoadDataWithJson()._positionY, SaveManager.LoadDataWithJson()._positionZ);
        if (SaveManager.LoadDataWithJson()._doorControllers.Count > 0)
        {
            foreach (GameObject door in SaveManager.LoadDataWithJson()._doorControllers)
            {
                if (door != null)
                {
                    GameObject.Find(door.name).GetComponent<DoorController>()._isClose = false;
                    UnityEngine.Debug.Log(GameObject.Find(door.name).name);
                }
            }
        }

        UnityEngine.Debug.Log("loaded");
    }
}
