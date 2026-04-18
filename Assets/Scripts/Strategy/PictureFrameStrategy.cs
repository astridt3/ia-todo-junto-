using UnityEngine;

public class PictureFrameStrategy : IObjectActionStrategy
{
    public void DiferentAction(TypeAction context)
    {
        if (context.TextUI != null)
            context.TextUI.text = "Picture Frame";

        if (context.LeftHand != null)
        {
            var leftAction = context.LeftHand.GetComponent<TypeAction>();
            if (leftAction._type != "PictureFrame" && leftAction._type != "PictureComplete")
            {
                context.LeftHand.transform.localScale = context.transform.localScale;
                context.LeftHand.GetComponent<MeshFilter>().mesh = context.GetComponent<MeshFilter>().mesh;
                context.LeftHand.GetComponent<MeshRenderer>().materials = context.GetComponent<MeshRenderer>().materials;
                leftAction._type = context._type;
            }
        }
    }
}
