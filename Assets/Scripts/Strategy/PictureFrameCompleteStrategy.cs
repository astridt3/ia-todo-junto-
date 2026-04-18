using UnityEngine;

public class PictureFrameCompleteStrategy : IObjectActionStrategy
{
    public void DiferentAction(TypeAction context)
    {

        if (context.LeftHand != null)
        {
            GameObject.Find("KeyContainer").GetComponentInChildren<KeyBehaviour>()._keyPropetys = GameObject.Find("KeyContainerInv").GetComponent<KeyBehaviour>()._keyPropetys;
        }
    }
}
