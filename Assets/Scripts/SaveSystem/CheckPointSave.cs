using System.Collections.Generic;
using UnityEngine;

public class CheckPointSave : MonoBehaviour
{
    [SerializeField] private List <GameObject> doorsOpenedAt;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "player")
        {
            SaveManager.SaveDataWithJson(other.gameObject, doorsOpenedAt);

            Debug.Log("Saved");
            gameObject.SetActive(false);
        }
    }
}
