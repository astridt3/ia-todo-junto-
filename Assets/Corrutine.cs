using TMPro;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Corrutine : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI introText;
    [SerializeField] private string Text = "Grab one… the WHEEL will guide your choice";
    [SerializeField] private float typingSpeed = 0.05f;

    void Start()
    {
        StartCoroutine(ShowText());
    }

    IEnumerator ShowText()
    {
        introText.text = "";
        yield return new WaitForSeconds(5f);
        foreach (char letter in Text.ToCharArray())
        {
            introText.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }
        yield return new WaitForSeconds(5f);
        introText.text = "";
    }

}

