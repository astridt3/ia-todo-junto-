using TMPro;
using UnityEngine;
using static ItemType;

public class Action : MonoBehaviour, Strategy
{

    [SerializeField] private ItemType.ItemTypee type;

    [SerializeField] private TextMeshProUGUI text;

    void Start()
    {
        text = GameObject.Find("Text").GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void DoAction()
    {
        if (type== ItemType.ItemTypee.Key)
        {
            text.text = "Key";
        }

        if (type == ItemType.ItemTypee.Flashlight)
        {
            text.text = "Flashlight";
        }

        if (type == ItemType.ItemTypee.Rope)
        {
            text.text = "Rope";
        }

        if (type == ItemType.ItemTypee.ClownNose)
        {
            text.text = "ClownNose";
        }
    }
}