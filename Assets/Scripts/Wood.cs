using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Wood : MonoBehaviour
{
    private Dictionary<string, Transform> woodTypes = new Dictionary<string, Transform>();


    void Start()
    {
        GameObject pos = GameObject.Find("PosWood");
        woodTypes.Add(("Wood"), GameObject.Find("PosWood").transform);
    }

    private void OnCollisionEnter(Collision collision)
    {
        GameObject obj = collision.gameObject;
        string objectName = obj.name;

        if (woodTypes.ContainsKey(objectName))
        {
            DeliveredObj(obj, objectName);
        }
    }
    private void DeliveredObj(GameObject obj, string objectName)
    {
        Transform targetpos = woodTypes[objectName];
        obj.transform.position = targetpos.position + Vector3.up * 0.2f;
        obj.transform.rotation = targetpos.rotation;
    }
}
