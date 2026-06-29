using System.Collections.Generic;
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

    [SerializeField] private Node[] allNodes;

    private List<Node> currentPath = new List<Node>();
    private int currentNodeIndex = 0;
    private bool usingPath = false;

    [SerializeField] private LayerMask obstacleMask;

    [SerializeField] private float decisionInterval = 0.5f;
    private float decisionTimer;

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
        decisionTimer += Time.deltaTime;
        context.player = player;
        if (decisionTimer >= decisionInterval)
        {
            decisionTimer = 0;
            tree.Evaluate(this, context);
        }
        Move(dir);
    }
    private void CalculateThetaPath()
    {
        Node start = GetClosestNode(transform.position);
        Node goal = GetClosestNode(player.position);

        currentPath = ThetaStar.Run(

            start,

            node => node == goal,

            node => node.neightbourds,

            (a, b) => Vector3.Distance(a.transform.position, b.transform.position),

            node => Vector3.Distance(node.transform.position,
                                     goal.transform.position),

            (a, b) =>
            {
                Vector3 dir = b.transform.position - a.transform.position;

                return !Physics.Raycast(
                    a.transform.position + Vector3.up * 0.5f,
                    dir.normalized,
                    dir.magnitude,
                    obstacleMask);
            }
        );

        if (currentPath.Count == 0)
        {
            usingPath = false;
            currentPath.Clear();
            currentNodeIndex = 0;
            return;
        }

        currentNodeIndex = 1;
        usingPath = true;
    }
    public void ArriveThetaStar()
    {
        Vector3 dirToPlayer = player.position - transform.position;

        bool obstacleBetween =
            Physics.Raycast(
                transform.position + Vector3.up * 0.5f,
                dirToPlayer.normalized,
                dirToPlayer.magnitude,
                obstacleMask);

        // Si ya no hay obstáculo, perseguir directamente
        if (!obstacleBetween)
        {
            usingPath = false;
            currentPath.Clear();
            currentNodeIndex = 0;

            ArriveToPlayer();
            return;
        }

        // Si hay obstáculo, seguir usando Theta*
        if (!usingPath)
        {
            CalculateThetaPath();
        }

        FollowThetaPath();
    }
    private void FollowThetaPath()
    {
        if (!usingPath)
            return;

        if (currentNodeIndex >= currentPath.Count)
        {
            usingPath = false;
            currentPath.Clear();
            currentNodeIndex = 0;
            return;
        }

        Vector3 target = currentPath[currentNodeIndex].transform.position;

        dir = SteeringBehaviours.Seek(transform, target);

        if (Vector3.Distance(transform.position, target) < 1f)
        {
            CalculateThetaPath();
        }
    }

    private Node GetClosestNode(Vector3 pos)
    {
        Node closest = null;
        float minDist = Mathf.Infinity;

        foreach (Node node in allNodes)
        {
            float d = Vector3.Distance(pos, node.transform.position);

            if (d < minDist)
            {
                minDist = d;
                closest = node;
            }
        }

        return closest;
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