using UnityEngine;
using System;
using System.Collections.Generic;

[Serializable]
public class SaveFile
{
    public float _positionX;
    public float _positionY;
    public float _positionZ;
    public List<GameObject> _doorControllers;

    public SaveFile(GameObject player, List<GameObject> doors) 
    { 
        _positionX = player.transform.position.x;
        _positionY = player.transform.position.y;
        _positionZ = player.transform.position.z;
        _doorControllers = doors;
    } 
}
