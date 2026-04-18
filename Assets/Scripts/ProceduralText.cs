using System.Collections;
using TMPro;
using UnityEngine;

public class ProceduralText : MonoBehaviour
{
    private string emptyText;
    private char[] textForParts; //= { 'I', ',', 'm', 'u', 's', 't', 'us' };
    private string usefulObjectText = "This can be useful";
    private string cameraText = "I must use this photo somewhere...";

    private TextMeshProUGUI textOnUI;

    int numberDe;
    private bool doIt;

    private void Awake()
    {
        doIt = true;
        numberDe = 0;
    }

    private void Start()
    {
        textOnUI = GameObject.Find ("CorrutineText").GetComponent<TextMeshProUGUI>();
        StartCoroutine("EnableCorrutine");
    }

    private void Update()
    {
        if (textForParts != null)
        {
            if (numberDe >= textForParts.Length)
                {
                    CancelInvoke();
                    numberDe = 0;
                }
        }
        
    }

    private void UpdatingText()
    {
        textOnUI.text += textForParts[numberDe];
        numberDe++;
    }

    public void CallEvent()
    {
        InvokeRepeating(nameof(UpdatingText), 1, 1f);
    }

    public void CallEventWihCorrutine(int textToShow)
    {
        if (doIt == false)
        {
            switch (textToShow)
        {
            case 0: //cero es para los textos de agarre a objetos
                {
                    emptyText = usefulObjectText;
                }
                break;
            case 1: //uno es para la cámara de foto
                {
                    emptyText = cameraText;
                }
            break;

        }
            textForParts = emptyText.ToCharArray();
            StartCoroutine("Corrutine");
            doIt = true;
        }
    }

    public IEnumerator Corrutine()
    {
        textOnUI.text += textForParts[numberDe];

        yield return new WaitForSeconds(0.2f);
        numberDe++;

        if (numberDe >= textForParts.Length)
        {
            yield return new WaitForSeconds (0.5f);

            textOnUI.text = "";
            doIt = false;
            yield return null;
        }
        else
        {
            StartCoroutine("Corrutine");
        }
        yield return null;
    }
    public IEnumerator EnableCorrutine()
    {
        yield return new WaitForSeconds(15);
        doIt = false;
    }
}
