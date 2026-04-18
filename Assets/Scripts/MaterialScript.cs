using System.Collections.Generic;
using UnityEngine;

public class MaterialScript : MonoBehaviour
{
    [SerializeField] private List<Texture2D> cluesTexture = new List<Texture2D>();
    [SerializeField] private List<GameObject> cluesGameObject = new List<GameObject>();
    void Start()
    {

        for(int i = 0; i < cluesGameObject.Count; i++)
        {
            cluesGameObject[i].GetComponent<MeshRenderer>().material.mainTexture = cluesTexture[i];
        }
    }

}
