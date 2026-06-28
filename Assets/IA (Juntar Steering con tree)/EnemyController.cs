using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemyController : MonoBehaviour
{
    [SerializeField] private Transform player;
    private LineOfSight los;
    private EnemyTree desicionTree;
    private EnemyContext context;
    private Vector3 lastSeenPosition;
    private bool hasLastSeenPosition = false;
    private bool obstacleDetected = false;
    [SerializeField] private float speed = 3;
    [SerializeField] private float rotationSpeed = 33;
    [SerializeField] private float patrolRotationSpeed = 33;
    private Rigidbody playerRB;
    private Vector3 wanderDirection;
    private float wanderTime;
    [SerializeField] private float WanderchangeInterval = 1.5f;
    private Vector3 dir;
    private bool isAttacking = false;
    [SerializeField] private float arriveRadius = 3f;
    [SerializeField] private float maxPredictionTime = 2f;
    [SerializeField] private Node[] allNodes;

    private List<Node> currentPath = new List<Node>();
    private int currentNodeIndex = 0;
    private bool usingPath = false;

    [SerializeField] private LayerMask obstacleLayer;
    [SerializeField] private float obstacleDetectionDistance = 2f;
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
        if (!los.IsObstacle(transform, player) && los.IsRange(transform, player))
        {
            lastSeenPosition = player.position;
            hasLastSeenPosition = true;
        }
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
    private bool DetectObstacleAhead()
    {
        Vector3 origin = transform.position + Vector3.up * 0.5f;

        Vector3 direction = (player.position - transform.position).normalized;
        Debug.DrawRay(origin, direction * obstacleDetectionDistance, Color.red);
        return Physics.Raycast(
            origin,
            direction,
            obstacleDetectionDistance,
            obstacleLayer
        );

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
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Obstacle"))
        {
            usingPath = false;
            CalculatePath();
        }
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
        if (DetectObstacleAhead())
        {
            CalculatePath();
            FollowPath();
        }

        usingPath = false;
        dir = SteeringBehaviours.Seek(transform, player.position);
    }

    private Node GetClosestNode(Vector3 pos)
    {
        Node closest = null;
        float minDist = Mathf.Infinity;

        foreach (Node node in allNodes)
        {
            float dist = Vector3.Distance(pos, node.transform.position);

            if (dist < minDist)
            {
                minDist = dist;
                closest = node;
            }
        }

        return closest;
    }

    private void CalculatePath()
    {
        Debug.Log("Camino encontrado: " + currentPath.Count);
        Node start = GetClosestNode(transform.position);
        if (!hasLastSeenPosition)
            return;

        Node goal = GetClosestNode(player.position);

        currentPath = Dijkstra.Run(
            start,
            node => node == goal,
            node => node.neightbourds,
            (a, b) => Vector3.Distance(a.transform.position, b.transform.position)
        );
        if (currentPath.Count == 0)
        {
            usingPath = false;
            return;
        }
        usingPath = true;
        currentNodeIndex = currentPath.Count > 1 ? 1 : 0;
        usingPath = currentPath.Count > 0;
        foreach (Node n in currentPath)
        {
            Debug.Log(n.name);
        }
    }

    private void FollowPath()
    {
        Debug.Log("Siguiendo camino");
        if (!usingPath)
            return;

        if (currentNodeIndex >= currentPath.Count)
        {
            usingPath = false;
            return;
        }

        Vector3 targetPos = currentPath[currentNodeIndex].transform.position;

        dir = SteeringBehaviours.Seek(transform, targetPos);

        if (Vector3.Distance(transform.position, targetPos) < 0.3f)
        {
            currentNodeIndex++;
        }
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
