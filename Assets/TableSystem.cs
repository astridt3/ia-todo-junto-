using UnityEngine;
using System.Collections.Generic;
using UnityEngine.InputSystem;

public class TableSystem : MonoBehaviour
{
    private Dictionary<string, Transform> tablePositions = new Dictionary<string, Transform>();
    [SerializeField] private Transform spawnLlave;
    private int countDelivered = 0;
    private List<string> deliveredObjects = new List<string>();
    private GameObject luzPrefab;
    [SerializeField] private ObjectsNeededForTable requirements;

    private ParticleEmmiter particlesEmmiter;
    private SoundManager soundManager;

    private void Start()
    {
        InitiTable();
    }

    private void OnCollisionEnter(Collision collision)
    {
        GameObject obj = collision.gameObject;
        string objName = obj.name;

        if (tablePositions.ContainsKey(objName))
        {
            DeliveredObj(obj, objName);
        }
    }

    private void DeliveredObj(GameObject obj, string objectName)
    {
        obj.tag = "Untagged";
        Transform targetpos = tablePositions[objectName];
        obj.transform.position = targetpos.position + Vector3.up * 0.2f;
        obj.transform.rotation = targetpos.rotation;

        if (obj.TryGetComponent<Rigidbody>(out Rigidbody rb))
        {
            rb.isKinematic = true;
            rb.useGravity = false;
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }
        deliveredObjects.Add(objectName);
        countDelivered++;
        if (countDelivered == tablePositions.Count)
        {
            GameObject spawnedObject  =Instantiate(requirements._resultGameObject, spawnLlave.position + Vector3.up * 0.5f, spawnLlave.rotation);
            
            particlesEmmiter.DoEmit();
            soundManager.ReproduceSound(1);

            if (spawnedObject.name.Contains("LadderPrefab"))
            {
                SpawnSpotLight(spawnedObject.transform);
            }
        }

        if (GetComponent <ChangeTableFormula>() != null)
        {
            requirements = GetComponent <ChangeTableFormula>()._formulaToChange;
            InitiTable();
            Destroy(GetComponent<ChangeTableFormula>());
        }
    }
    private void SpawnSpotLight(Transform ladderTransform)
    {
        GameObject luz = new GameObject("SpotLightSpawn");
        Light light = luz.AddComponent<Light>();

        light.type = LightType.Spot;
        light.color = Color.yellow;
        light.intensity = 1000f;
        light.range = 25f;
        light.spotAngle = 30f;
        light.innerSpotAngle = 40f;

        luz.transform.position = ladderTransform.position + Vector3.up * 17f;
        ladderTransform.Rotate(0, 180, 0);
        luz.transform.rotation = Quaternion.Euler(90f, 0f, 0f);

    }
    private void InitiTable()
    {
        tablePositions.Clear();
        GameObject pos1 = GameObject.Find("Pos1");
        tablePositions.Add((requirements._objectName1), GameObject.Find(requirements._nameTransform1).transform);
        GameObject pos2 = GameObject.Find("Pos2");
        tablePositions.Add((requirements._objectName2), GameObject.Find(requirements._nameTransform2).transform);
        GameObject pos3 = GameObject.Find("Pos3");
        tablePositions.Add((requirements._objectName3), GameObject.Find(requirements._nameTransform3).transform);

        particlesEmmiter = gameObject.GetComponentInChildren<ParticleEmmiter>();
        soundManager = GameObject.Find("SoundManager").GetComponent<SoundManager>();
    }
}