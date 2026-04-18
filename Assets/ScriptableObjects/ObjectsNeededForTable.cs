using UnityEngine;

[CreateAssetMenu(fileName = "ObjectsNeededForTable", menuName = "Scriptable Objects/ObjectsNeededForTable")]
public class ObjectsNeededForTable : ScriptableObject
{
    [SerializeField] private string objectName1;
    [SerializeField] private string objectName2;
    [SerializeField] private string objectName3;

    [SerializeField] private string nameTransform1;
    [SerializeField] private string nameTransform2;
    [SerializeField] private string nameTransform3;

    [SerializeField] private GameObject resultGameObject;
    public string _objectName1 => objectName1;
    public string _objectName2 => objectName2;
    public string _objectName3 => objectName3;

    public string _nameTransform1 => nameTransform1;
    public string _nameTransform2 => nameTransform2;
    public string _nameTransform3 => nameTransform3;
    public GameObject _resultGameObject => resultGameObject;
}
