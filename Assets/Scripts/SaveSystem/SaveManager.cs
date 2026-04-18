using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using NUnit.Framework;
using System.Collections.Generic;
using NUnit.Framework.Constraints;

public class SaveManager : MonoBehaviour
{
    public static void SaveDataWithBinary(GameObject player, List<GameObject> doors)
    {
        SaveFile save = new SaveFile(player, doors);
        string dataPath = Application.persistentDataPath + "/player.save";
        FileStream fileStream = new FileStream(dataPath, FileMode.Create);
        BinaryFormatter bf = new BinaryFormatter();
        bf.Serialize(fileStream, save);
        fileStream.Close();
    }

    public static void SaveDataWithJson(GameObject player, List<GameObject> doors)
    {
        SaveFile save = new SaveFile(player, doors);
        string json = JsonUtility.ToJson(save);
        string path = Application.persistentDataPath + "/playerData.json";
        File.WriteAllText(path, json);
    }

    public static SaveFile LoadDataWithBinary()
    {
        string dataPath = Application.persistentDataPath + "/player.save";
        if (File.Exists(dataPath))
        {
            FileStream fileStream = new FileStream(dataPath, FileMode.Open);
            BinaryFormatter bf = new BinaryFormatter();
            SaveFile save = (SaveFile)bf.Deserialize(fileStream);
            fileStream.Close();
            return save;
        }
        else
        {
            return null;
        }
    }

    public static SaveFile LoadDataWithJson()
    {
        string path = Application.persistentDataPath + "/playerData.json";
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            SaveFile data = JsonUtility.FromJson<SaveFile>(json);
            Debug.Log("Game Loaded from: " + path);
            return data;
        }
        else
        {
            return null;
        }
    }
}
