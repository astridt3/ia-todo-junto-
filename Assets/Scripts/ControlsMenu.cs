using UnityEngine;

public class ControlsMenu : MonoBehaviour
{
    public void ReturnToMenu() 
    {
        Singleton.instance.BackToMenu();
    }
}
