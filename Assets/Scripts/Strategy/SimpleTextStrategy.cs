using TMPro;
using UnityEngine;

public class SimpleTextStrategy : IObjectActionStrategy
{
    private string displayText;

    public SimpleTextStrategy(string text)
    {
        displayText = text;
    }

    public void DiferentAction(TypeAction context)
    {
        if (context.TextUI != null)
        {
            context.TextUI.text = displayText;
        }
    }
}
