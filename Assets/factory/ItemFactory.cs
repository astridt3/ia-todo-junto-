using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class ItemFactory : MonoBehaviour
{

    private Shader shader;
    [System.Serializable]
    public struct ItemEntry
    {
        public ItemType.ItemTypee type; 
        public GameObject prefab;
    }

    [SerializeField] private List<ItemEntry> itemList;
    private Dictionary<ItemType.ItemTypee, GameObject> itemPrefabs;

    public Shader _shader { get { return shader; } set{ shader = value; } }


    void Awake()
    {
        itemPrefabs = new Dictionary<ItemType.ItemTypee, GameObject>();
        foreach (var entry in itemList)
        {
            itemPrefabs[entry.type] = entry.prefab;
        }
    }

    public GameObject CreateItem(ItemType.ItemTypee type, Vector3 position)
    {
        if (itemPrefabs.ContainsKey(type))
        {
            return Instantiate(itemPrefabs[type], position, Quaternion.identity);
        }
        
        return null;
    }

    public GameObject CreateFrame( int width, int height, Camera cameraUsed, GameObject frameBase, CameraBehaviour cameraBehaviour)
    {
        Texture2D textureToUseAsMaterial = new Texture2D(width, height);

        textureToUseAsMaterial = TakeAScreenhoot(width, height, cameraUsed);

        frameBase.GetComponent<MeshRenderer>().material.SetTexture ( "_BaseMap", textureToUseAsMaterial);
        frameBase.GetComponent<MeshRenderer>().sharedMaterial.SetTexture("_BaseMap", textureToUseAsMaterial);        

        frameBase.GetComponent<TypeAction>()._type = "PictureComplete";
        cameraBehaviour._stateOfCamera = 3;
        return frameBase;
    }

    private Texture2D TakeAScreenhoot(int width, int height,Camera cameraToTakeTheScreenShoot)
    {
        RenderTexture rt = new RenderTexture(width, height, 24);
        cameraToTakeTheScreenShoot.targetTexture = rt;
        Texture2D screenShot = new Texture2D(width, height, TextureFormat.RGBA32, false);
        cameraToTakeTheScreenShoot.Render();
        RenderTexture.active = rt;
        screenShot.ReadPixels(new Rect(0, 0, width, height), 0, 0);
        byte[] bytes = screenShot.EncodeToPNG();

        cameraToTakeTheScreenShoot.targetTexture = null;
        RenderTexture.active = null;

        screenShot.Apply();
        Destroy(rt);
        return screenShot;
    }
}
