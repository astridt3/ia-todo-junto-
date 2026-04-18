using UnityEngine;
using UnityEngine.Events;

public class KeyBehaviour : MonoBehaviour
{
    [SerializeField] private KeysObjects keyPropetys;

    public UnityEvent KeyUse;
    public KeysObjects _keyPropetys { get { return keyPropetys; } set { keyPropetys = value; } }

    private void Start()
    {
        KeyUse = new UnityEvent();
        KeyUse.AddListener(AssignKey);
    }

    public void DesUseKey()
    {
        Inventory playerInventory = GameObject.Find("player").GetComponent<Inventory>();

        playerInventory.DestroyDroppedObject();
        keyPropetys = default;
    }

    private void AssignKey()
    {
        if (keyPropetys != null)
        {
            GetComponent<MeshFilter>().mesh = keyPropetys._modelOfKey.mesh;
            GetComponent<MeshRenderer>().material = keyPropetys._materialOfKey.material;
        }
    }
}
