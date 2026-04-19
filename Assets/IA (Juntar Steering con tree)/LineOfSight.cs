using UnityEngine;

public class LineOfSight : MonoBehaviour
{
    [SerializeField] private int distance = 33;
    [SerializeField] private float angle = 90;
    [SerializeField] private LayerMask obs;
    private GameObject player;
    private MeshRenderer playerRenderer;

    [SerializeField] private float distanceForAttack;
    private void Start()
    {
        player = GameObject.Find("player");
        playerRenderer = player.GetComponent<MeshRenderer>();
    }

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
