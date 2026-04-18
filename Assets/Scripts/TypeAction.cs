using TMPro;
using UnityEngine;
using System.Collections.Generic;

public class TypeAction : MonoBehaviour, ITypeOFObjectInterface
{
    [SerializeField] private string type;

    private TextMeshProUGUI text;
    private GameObject leftHand;

    private static Dictionary<string, IObjectActionStrategy> strategies;

    public string _type
    {
        get => type;
        set => type = value;
    }

    public TextMeshProUGUI TextUI => text;
    public GameObject LeftHand => leftHand;

    private void Start()
    {
        if (strategies == null)
        {
            strategies = new Dictionary<string, IObjectActionStrategy>
            {
                { "Frame", new SimpleTextStrategy("Frame") },
                { "SmallTool", new SimpleTextStrategy("Small Tool") },
                { "RollOfFilm", new SimpleTextStrategy("Roll Of Film") },
                { "Mask", new SimpleTextStrategy("Mask") },
                { "Serpentine", new SimpleTextStrategy("Serpentine") },
                { "Paint", new SimpleTextStrategy("Paint") },
                { "Key", new SimpleTextStrategy("Key") },
                { "Wood", new SimpleTextStrategy("Wood") },
                { "Tape", new SimpleTextStrategy("Tape") },
                { "Rope", new SimpleTextStrategy("Rope") },
                { "Ladder", new SimpleTextStrategy("Ladder") },
                { "BrokenGlass", new SimpleTextStrategy("Broken Glass") },
                { "Camera", new SimpleTextStrategy("Camera") },
                { "PictureFrame", new PictureFrameStrategy() },
                { "Confeti", new SimpleTextStrategy("Confeti")},
                { "BrokenCamera", new SimpleTextStrategy("BrokenCamera")},
                { "PictureComplete", new PictureFrameCompleteStrategy ()}
            };
            Action();
        }

        GameObject textObject = GameObject.Find("TextUI");
        if (textObject != null)
            text = textObject.GetComponent<TextMeshProUGUI>();
        

        if (leftHand == null)
            leftHand = GameObject.Find("LeftHand");
    }

    public void Action()
    {
        if (strategies.TryGetValue(type, out var strategy))
        {
            strategy.DiferentAction(this);
        }
        else
        {
            Debug.LogWarning($"No strategy defined for type: {type}");
        }
    }
}
