using UnityEngine;

[CreateAssetMenu(fileName = "KeysObjects", menuName = "Scriptable Objects/KeysObjects")]
public class KeysObjects : ScriptableObject
{
    [SerializeField] private MeshFilter modelOfKey;
    [SerializeField] private MeshRenderer materialOfKey;

    [SerializeField] private string doorToOpen;

    public MeshFilter _modelOfKey => modelOfKey;
    public MeshRenderer _materialOfKey => materialOfKey;
    public string _doorToOpen => doorToOpen;
}
