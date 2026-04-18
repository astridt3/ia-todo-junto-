using System.Collections;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    private AudioSource playerSource;

    [SerializeField] private AudioClip takeSound;
    [SerializeField] private AudioClip creationSucces;
    [SerializeField] private AudioClip foots1;
    [SerializeField] private AudioClip foots2;
    [SerializeField] private AudioClip cameraTake;
    [SerializeField] private AudioClip ballon;
    [SerializeField] private AudioClip door;

    private int footNUM; //Decide que paso se va a usar entre el 1 y 2
    private bool isReproducingStep;
    
    void Start()
    {
        playerSource = GameObject.Find("player").GetComponent<AudioSource>();
        playerSource.Play();

        footNUM = 1;
        isReproducingStep = false;
        playerSource.volume = 0.8f;
    }

    public void ReproduceSound(int sound) //0 es takeSound, 1 es creationSucces, 2 para los pasos y 3 para la cámara 
    {
        switch (sound)
        {
            case 0:
                playerSource.PlayOneShot(takeSound);
                break;

            case 1:
                playerSource.PlayOneShot(creationSucces);
                break;

            case 2:
                if (isReproducingStep == false)
                {
                    isReproducingStep = true;
                    if (footNUM == 1)
                    {
                        playerSource.PlayOneShot(foots1);
                        footNUM = 2;
                    }
                    else
                    {
                        playerSource.PlayOneShot(foots2);
                        footNUM = 1;
                    }

                    StartCoroutine("FootsEnable");
                }
                    break;
            case 3:
                playerSource.PlayOneShot(cameraTake);
                break;

            case 4:
                playerSource.PlayOneShot(ballon);
                break;

            case 5:
                playerSource.PlayOneShot(door);
                break;

            default:
                break;
        }
    }

    private IEnumerator FootsEnable()
    {
        yield return new WaitForSeconds(0.8f);

        isReproducingStep = false;
        yield return null;
    }
}
