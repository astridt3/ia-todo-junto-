using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemyController : MonoBehaviour
{
    [SerializeField] private Transform player;
    private LineOfSight los;
    private EnemyTree desicionTree;
    private EnemyContext context;

    [SerializeField] private float speed = 3;
    [SerializeField] private float rotationSpeed = 33;
    [SerializeField] private float patrolRotationSpeed = 33;
    private Material defaultMaterial;
    private MeshRenderer renderer;
    private Rigidbody playerRB;
    private Vector3 wanderDirection;
    private float wanderTime;
    [SerializeField] private float WanderchangeInterval = 1.5f;
    private Vector3 dir;
    private bool isAttacking = false;
    [SerializeField] private float arriveRadius = 3f;
    [SerializeField] private float maxPredictionTime = 2f;



    private void Awake()
    {
        los = GetComponent<LineOfSight>();
        desicionTree = GetComponent<EnemyTree>();
        wanderDirection = transform.forward;
        context = new EnemyContext { self = transform, player = player, los = los };

    }

    private void Start()
    {
        player = GameObject.Find("player").transform;
    }

    public void Update()
    {
        context.player = player;
        desicionTree.Evaluate(this, context);
        Move(dir);
    }

    public void FleePlayer()////
    {
        dir = SteeringBehaviours.Flee(transform, player.position);
    }

    public void EvadePlayer()///
    {
        dir = SteeringBehaviours.Evade(transform, player, playerRB, maxPredictionTime);
    }
    public bool IsPlayerLookingAtMe()//
    {
        Vector3 dirToEnemy = (transform.position - player.position).normalized;

        float angle = Vector3.Angle(player.forward, dirToEnemy);

        return angle < 45f;
    }

    public void ArriveToPlayer()///
    {
        dir = SteeringBehaviours.Arrive(transform, player.position, arriveRadius);
    }
    public void Pursue()
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

        renderer.material = defaultMaterial;
    }
    
    public void Attack()
    {
        Debug.Log("Empieza a atacar");
        isAttacking = true;
        if (isAttacking)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            dir = Vector3.zero;
        }
            Debug.Log("Deja de atacar");
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

        Debug.Log("vAYA");
    }
    public void Seek()
    {
        dir = SteeringBehaviours.Seek(transform, player.transform.position);

        Debug.Log("vAYA123");
    }

    private void Move(Vector3 dir)
    {
        transform.position += dir * speed * Time.deltaTime;

        if (dir != Vector3.zero)
        {
            transform.forward = Vector3.Lerp(transform.forward, dir, Time.deltaTime * rotationSpeed);
        }
    }

}
