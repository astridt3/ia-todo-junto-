using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Singleton : MonoBehaviour
{
    public static Singleton instance;
    private bool inGame = false;
    private SceneManagerBehaviors sceneMGR;
    private InitialLevelReference initLevel;

    public bool _inGame { get { return inGame; } set { inGame = true; } }
    public SceneManagerBehaviors _sceneMGR { get { return sceneMGR; } set { sceneMGR = value; } }
    public InitialLevelReference _initLevel { get { return initLevel; } set { initLevel = value; } }

    void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void PlayButton()
    {
        sceneMGR.PlayButton();
    }
    public void ExitButton()
    {
        sceneMGR.ExitButton();
    }

    public void BackToMenu()
    {
        sceneMGR.BackToMenu();
    }
    public void GoToWinScene()
    {
        sceneMGR.GoToWinScene();
    }

    public void LoadState()
    {
        sceneMGR.PlayButton();
        Invoke("UseFacadeGet", 1f);
    }

    public void GoToControls()
    {
        sceneMGR.GoToControls();
    }

    private void UseFacadeGet()
    {
        initLevel.LoadPlayerRef();
    }
}
