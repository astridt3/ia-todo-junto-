using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class WinScene : MonoBehaviour
{
    void Start()
    {

    }


    void Update()
    {

    }

    public void MainMenuButton()
    {
        Debug.Log("Volviendo al menu principal");
        Singleton.instance.BackToMenu();
    }
}
