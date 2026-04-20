using System.Xml.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagerBehaviors : MonoBehaviour
{
    private void Start()
    {
        Singleton.instance._sceneMGR = this;
    }

    public void PlayButton()
    {
        SceneManager.LoadScene("LevelsScenee");
        Singleton.instance._inGame = true;
    }
    public void ExitButton()
    {
        Application.Quit();
    }

    public void BackToMenu()
    {
        SceneManager.LoadScene("MainMenu");
        Singleton.instance._inGame = false;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
    public void GoToWinScene()
    {
        SceneManager.LoadScene("WinScene");
        Singleton.instance._inGame = false;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void GoToControls() 
    {
        SceneManager.LoadScene("ControlsScene");
    }
}
