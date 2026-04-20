using UnityEngine;

public class EnemyController3 : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private Transform checkpoint;
    private Rigidbody playerRB;
    private EnemyDecisionTree tree;
    private EnemyContext context;
    [SerializeField] private float speed = 3;
    [SerializeField] private float rotationSpeed = 33;
    [SerializeField] private float arriveRadius = 3f;
    [SerializeField] private float maxPredictionTime = 2f;

    private Vector3 dir;

    private void Awake()
    {
        tree = GetComponent<EnemyDecisionTree>();

        context = new EnemyContext
        {
            self = transform,
            player = player,
            enemy = this
        };
    }

    private void Start()
    {
        player = GameObject.Find("player").transform;
        playerRB = player.GetComponent<Rigidbody>();
    }

    private void Update()
    {
        context.player = player;
        tree.Evaluate(this, context);
        Move(dir);
    }

    public bool IsPlayerLookingAtMe()
    {
        Vector3 dirToEnemy = transform.position - player.position;
        float angle = Vector3.Angle(player.forward, dirToEnemy);
        bool isLooking = angle < 60f;
        return isLooking;
    }

    public void FleePlayer()
    {
        dir = SteeringBehaviours.Flee(transform, player.position);
    }

    public void ArriveToPlayer()
    {
        dir = SteeringBehaviours.Arrive(transform, player.position, arriveRadius);
    }

    public void EvadePlayer()
    {
        dir = SteeringBehaviours.Evade(transform, player, playerRB, maxPredictionTime);
    }

    public void Idle()
    {
        dir = Vector3.zero;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (playerRB != null)
            {
                playerRB.linearVelocity = Vector3.zero;
            }

            collision.transform.position = checkpoint.position;
        }
    }
    private void Move(Vector3 dir)
    {
        float currentSpeed;

        if (IsPlayerLookingAtMe())
        {
            currentSpeed = speed;
        }
        else
        {
            currentSpeed = speed * 2f;
        }
        transform.position += dir * currentSpeed * Time.deltaTime;
        if (dir != Vector3.zero)
        {
            Quaternion targetRot = Quaternion.LookRotation(dir);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRot, Time.deltaTime * rotationSpeed);
        }
    }
}