using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    private Movement player;

    void Start()
    {
        player = GameObject.Find("player").GetComponent<Movement>();
        gameObject.SetActive(false);
    }

    public void ResumeGame()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        gameObject.SetActive(false);
        player._menuActive = false;
        Time.timeScale = 1f;
    }

    public void GoToTheMainMenu()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        gameObject.SetActive(false);
        Time.timeScale = 1f;
        SceneManager.LoadScene(0);
    }

    public void QuitGame()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        gameObject.SetActive(false);
        Time.timeScale = 1f;
        Application.Quit();
    }
    public void LoadState()
    {
        player.LoadPlayerState();
    }
}
