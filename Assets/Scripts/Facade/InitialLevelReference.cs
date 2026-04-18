using System.Collections;
using UnityEngine;

public class InitialLevelReference : MonoBehaviour
{
    private void Awake()
    {
        if (Singleton.instance != null)
        {
            Singleton.instance._initLevel = this;
        }
    }
    public void LoadPlayerRef()
    {
        StartCoroutine("FindPlayer");
    }

    private IEnumerator FindPlayer()
    {
        yield return new WaitForSeconds(0.3f);

        GameObject.Find("player").GetComponent<Movement>().LoadPlayerState();
        Debug.Log("loaded");

        yield return null;
    }
}
