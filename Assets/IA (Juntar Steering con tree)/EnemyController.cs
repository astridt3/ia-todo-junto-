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

    private bool chasingPlayer = false;
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
    public void PatrolNodes()
    {
        if (!usingPath)
            CalculatePatrolPath();

        FollowPath();
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
    private void CalculatePatrolPath()
    {
        Node start = GetClosestNode(transform.position);

        Node goal = allNodes[Random.Range(0, allNodes.Length)];

        currentPath = Dijkstra.Run(
            start,
            node => node == goal,
            node => node.neightbourds,
            (a, b) => Vector3.Distance(a.transform.position, b.transform.position)
        );

        if (currentPath.Count == 0)
            return;

        currentNodeIndex = currentPath.Count > 1 ? 1 : 0;
        usingPath = true;
    }
    public void Patrol()
    {
        transform.Rotate(0, rotationSpeed * Time.deltaTime, 0);
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
        if (los.IsObstacle(transform, player))
        {
            CalculatePath();

            FollowPath();
        }
        else
        {
            usingPath = false;
            dir = SteeringBehaviours.Seek(transform, player.position);
        }
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
        if (!usingPath)
            return;

        if (currentPath == null || currentPath.Count == 0)
        {
            usingPath = false;
            return;
        }

        if (currentNodeIndex >= currentPath.Count)
        {
            usingPath = false;
            return;
        }

        //Debug.Log("Voy hacia el nodo: " + currentPath[currentNodeIndex].name);

        Vector3 targetPos = currentPath[currentNodeIndex].transform.position;

        dir = SteeringBehaviours.Seek(transform, targetPos);

        if (Vector3.Distance(transform.position, targetPos) < 0.3f)
        {
            //Debug.Log("Llegué al nodo: " + currentPath[currentNodeIndex].name);

            currentNodeIndex++;

            if (currentNodeIndex < currentPath.Count) ;
            //Debug.Log("Siguiente nodo: " + currentPath[currentNodeIndex].name);
            else
            {
                //Debug.Log("Fin del camino");
                usingPath = false;
            }
        }
    }
    public void ChaseWithPath()
    {
        if (!usingPath)
            CalculatePath();

        FollowPath();

        if (!los.IsObstacle(transform, player))
        {
            usingPath = false;
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
    private void OnDrawGizmos()
    {
        if (currentPath == null || currentPath.Count < 2)
            return;

        Gizmos.color = Color.green;

        for (int i = 0; i < currentPath.Count - 1; i++)
        {
            Gizmos.DrawLine(
                currentPath[i].transform.position,
                currentPath[i + 1].transform.position);

            Gizmos.DrawSphere(currentPath[i].transform.position, 0.15f);
        }

        Gizmos.DrawSphere(currentPath[currentPath.Count - 1].transform.position, 0.15f);
    }
}
