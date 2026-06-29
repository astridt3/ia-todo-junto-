using TMPro;
using UnityEngine;

public class BalloonCounter : MonoBehaviour
{
    [SerializeField] private TMP_Text counterText;

    private int balloons = 0;

    private void Start()
    {
        UpdateUI();
    }
    private void Awake()
    {
        counterText = GameObject.Find("GoUI").GetComponent<TMP_Text>();
    }
    public void AddBalloon()
    {
        balloons++;
        UpdateUI();
    }

    private void UpdateUI()
    {
        counterText.text = balloons.ToString();
    }
}