using UnityEngine;

public class RequestNeeded : MonoBehaviour
{
    [SerializeField] private Vector2 rotationNeededX; //"x" es la rotación minima que debe superar y "y" es la rotación que no debe pasar. Esto es base a la mirada en el eje "x"
    [SerializeField] private Vector2 rotationNeededY; //Funciona de la misma froma que el requisito anterior, siendo este en base a la altura hacía donde se debe ver
    [SerializeField] private string nameRequest;

    public Vector2 _rotationNeededX => rotationNeededX;
    public Vector2 _rotationNeededY => rotationNeededY;
    public string _nameRequest => nameRequest;
}
