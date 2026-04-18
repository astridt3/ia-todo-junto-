using UnityEngine;

public class enemyModel : MonoBehaviour
{
    private GameObject player;
    private Rigidbody playerRB;
    [SerializeField] float speed = 3f;
    [SerializeField] float rotationSpeed = 5f;
    private Vector3 wanderDirection;
    private float wanderTime;
    [SerializeField] private float WanderchangeInterval = 1.5f;
    private Vector3 dir
    {
        get { return dir; }
        set { dir = value; }
    }

    private void Start()
    {
        player = GameObject.Find("player");

    }

    private void Update()
    {
    }

    public void Pursuit()
    {
        Vector3 direction = player.transform.position - transform.position;
        direction.y = 0;
        Vector3 moveDirection = direction.normalized;

        transform.position += moveDirection * speed * Time.deltaTime;

        transform.forward = Vector3.Lerp(transform.forward, moveDirection, Time.deltaTime * rotationSpeed);
    }

    public void Patrol()
    {
        transform.Rotate(0, rotationSpeed * Time.deltaTime, 0);
    }
    public void Wander()
    {
        wanderTime -= Time.deltaTime;
        if (wanderTime <= 0f)
        {
            wanderDirection = SteeringBehaviours.Wander(wanderDirection, 180f);
            wanderTime = WanderchangeInterval;
        }
        dir = wanderDirection;
    }
    public void Seek()
    {
        dir = SteeringBehaviours.Seek(transform, player.transform.position);
    }
}
