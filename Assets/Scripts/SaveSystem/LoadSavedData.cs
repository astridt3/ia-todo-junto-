using Unity.VisualScripting;
using UnityEngine;

public class LoadSavedData : MonoBehaviour
{
    public void LoadData()
    {
        GameObject.Find ("player").transform.position = new Vector3(SaveManager.LoadDataWithJson()._positionX, SaveManager.LoadDataWithJson()._positionY, SaveManager.LoadDataWithJson()._positionZ);
        foreach (GameObject door in SaveManager.LoadDataWithJson()._doorControllers)
        {
            GameObject.Find(door.name).GetComponent<DoorController>()._isClose = false;
            UnityEngine.Debug.Log(GameObject.Find(door.name).name);
        }
    }
}
