using UnityEngine;
using UnityEngine.UI;

public class MainMenuInitializer : MonoBehaviour
{
    private GameObject playButton;
    private GameObject exitButton;
    private GameObject loadSave;
    private GameObject controlButton;
    void Start()
    {
        playButton = GameObject.Find("PlayButton");
        exitButton = GameObject.Find("ExitGameButton");
        loadSave = GameObject.Find("LoadSave");
        controlButton = GameObject.Find("ControlsButton");

        playButton.GetComponent<Button>().onClick.AddListener(Singleton.instance.PlayButton);
        exitButton.GetComponent<Button>().onClick.AddListener(Singleton.instance.ExitButton);
        loadSave.GetComponent<Button>().onClick.AddListener(Singleton.instance.LoadState);
        controlButton.GetComponent<Button>().onClick.AddListener(Singleton.instance.GoToControls);

        Cursor.visible = true;
    }
}
