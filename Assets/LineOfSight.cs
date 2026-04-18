using UnityEngine;

public class LineOfSight : MonoBehaviour
{
    [SerializeField] private int distance = 33;
    [SerializeField] private float angle = 90;

    [SerializeField] private LayerMask obs;

    private GameObject player;
    private MeshRenderer playerRenderer;

    //[SerializeField] private Material noSee;
    //[SerializeField] private Material seen;

    [SerializeField] private float distanceForAttack;
    private void Start()
    {
        player = GameObject.Find("player");
        playerRenderer = player.GetComponent<MeshRenderer>();
    }

    //private void Update()
    //{
    //    if (IsRange(transform, player.transform) == true && 
    //        IsAngle (transform, player.transform) == true && 
    //        IsObstacle (transform, player.transform) == true) 
    //    {
    //        playerRenderer.material = seen;
    //    }
    //    else 
    //    {
    //        playerRenderer.material = noSee;
    //    }
    //}

    public bool IsRange(Transform self, Transform target)
    {
        return Vector3.Distance(self.position, target.position) < distance;
    }

    public bool IsRangeAttack(Transform self, Transform target)
    {
        return Vector3.Distance(self.position, target.position) < distanceForAttack;
    }

    public bool IsAngle(Transform self, Transform target)
    {
        Vector3 dir = target.position - self.position;

        return Vector3.Angle(self.forward, dir) < angle / 2;
    }

    public bool IsObstacle(Transform self, Transform target)
    {
        Vector3 dir = target.position - self.position;

        return Physics.Raycast(self.position, dir.normalized, dir.magnitude, obs);
    }
}
