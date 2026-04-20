//using UnityEngine;

//public class ExitDoorWithKey : MonoBehaviour
//{
//    [SerializeField] private Transform door;
//    [SerializeField] private float openRot = 90f;
//    [SerializeField] private float speed = 3f;

//    private bool isOpening = false;

//    private void OnCollisionEnter(Collision collision)
//    {
//        if (collision.gameObject.name.Contains("KeyExit"))
//        {
//            Debug.Log("TOCO LA LLAVE");
//            isOpening = true;

//        }
//    }

//    private void Update()
//    {
//        if (isOpening)
//        {
//            float currentY = door.localEulerAngles.y;
//            float newY = Mathf.LerpAngle(currentY, openRot, speed * Time.deltaTime);

//            door.localEulerAngles = new Vector3(0, newY, 0);

//            if (Mathf.Abs(Mathf.DeltaAngle(newY, openRot)) < 0.5f)
//            {
//                door.localEulerAngles = new Vector3(0, openRot, 0);
//                isOpening = false;
//            }
//        }
//    }
//}