using UnityEngine;
using System.Collections;
using UnityEngine.Events;

public class CameraBehaviour : MonoBehaviour
{
    public UnityEvent cameraPhotoEvent;

    private Camera photographyCamera;
    private int stateOfCamera; // "0" representa que no se puede sacar la foto, "1" representa que se puede sacar la foto, "2" representa que se esta sacando la foto y "3" para cuando se tiene una foto en la mano izquierda
    private ItemFactory creatorOfPictures;
    private string nameForThePicture; 
    private GameObject pictureInLeftHand;
    private GameObject rotationForHand; //Para que en la mano se vea de frente la camra, se guarda la variable para acceder luego
    
    private SoundManager soundManager;

    private ProceduralText textWithPhoto;

    Animator animatorUI;

    private KeyBehaviour keyRef;

    private bool canFullCamera;

    public Camera _photographyCamera { get { return photographyCamera; } set { photographyCamera = value; } }
    public int _stateOfCamera { get { return stateOfCamera; } set { stateOfCamera = value; } }
    public string _nameOfThePicture { get { return nameForThePicture; } set { nameForThePicture = value; } }
    public GameObject _rotationForHand => rotationForHand;
    public KeyBehaviour _keyRef { get { return keyRef; } set { keyRef = value; } }
    public bool _canFullCamera { get { return canFullCamera; } set { canFullCamera = value; } }

    private void Awake()
    {
        rotationForHand = new GameObject();
        rotationForHand.transform.rotation.Set (0,180,0,0); 
    }

    private void Start()
    {
        cameraPhotoEvent = new UnityEvent();
        cameraPhotoEvent.AddListener (TakePhoto);
        cameraPhotoEvent.AddListener(GetComponentInParent<Movement>().DestroyLastPoint);

        photographyCamera = GetComponentInChildren<Camera>();
        stateOfCamera = 0;
        creatorOfPictures = new ItemFactory();

        pictureInLeftHand = GameObject.Find("LeftHand");
        animatorUI = GameObject.Find("UIFailPicture").GetComponent<Animator>();

        textWithPhoto = GameObject.Find("player").GetComponent<ProceduralText>();

        soundManager = GameObject.Find ("SoundManager").GetComponent<SoundManager>();

        keyRef = new KeyBehaviour();

        canFullCamera = true;
    }

    void Update()
    {
        if (GetComponent<TypeAction>()._type.Equals("Camera") == true)
        {
            if (Input.GetMouseButtonDown(0))
        {
            switch (stateOfCamera)
            {
                case 0:
                    StartCoroutine("TakeWrongPhoto");
                    break;

                case 1:
                    if (pictureInLeftHand.GetComponent<TypeAction>()._type == "PictureFrame")
                    {
                        cameraPhotoEvent.Invoke();
                    }
                    break;

                case 2:
                    break;

                case 3:
                    textWithPhoto.CallEventWihCorrutine(1);
                    break;
            }
        }

        if (Input.GetMouseButtonDown(1))
        {
            if (photographyCamera.enabled == true)
            {
                photographyCamera.enabled = false;
            }
            else if (canFullCamera == true)
            {
                photographyCamera.enabled = true;
            }

            GameObject.Find("player").GetComponent<Inventory>().SetMove();
        }
    }
    }

    private void TakePhoto()
    {
        stateOfCamera = 2;
        creatorOfPictures.CreateFrame(1920, 1080, photographyCamera, pictureInLeftHand, this);
        soundManager.ReproduceSound(3);

        if (keyRef != null)
        {
            GameObject.Find("KeyContainer").GetComponentInChildren<KeyBehaviour>()._keyPropetys = keyRef._keyPropetys;

        }
    }

    private IEnumerator TakeWrongPhoto()
    {
        animatorUI.SetBool("Transition",true);

        yield return new WaitForSeconds(0.3f);

        animatorUI.SetBool("Transition", false);

        yield return null;
    }

    //Crear metodo que destruya el punto de foto

}
