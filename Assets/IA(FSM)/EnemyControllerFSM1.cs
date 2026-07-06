using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemyControllerFSM1 : MonoBehaviour

{
    public Transform player;
    private LineOfSight los;
    [SerializeField] private float speed = 3;
    [SerializeField] private float rotationSpeed = 33;
    [SerializeField] private float patrolRotationSpeed = 33;
    private Rigidbody playerRB;
    private Vector3 wanderDirection;
    private float wanderTime;
    [SerializeField] private float WanderchangeInterval = 1.5f;
    private Vector3 dir;
    private FSMClasses1 fsm;
    private Coroutine freezeRoutine;
    [SerializeField] private float freezeCooldown = 5f;
    private bool canFreeze = true;
    [SerializeField] private Node[] allNodes;

    private List<Node> currentPath = new List<Node>();
    private int currentNodeIndex = 0;
    private bool usingPath = false;
    private float repathTimer;
    [SerializeField] float repathInterval = 0.5f;
    private Vector3 lastKnownPlayerPosition;
    private bool isAttacking = false;


    private bool ignoreObstacles = false;

    private void Awake()
    {
        fsm = GetComponent<FSMClasses1>();
        los = GetComponent<LineOfSight>();
        wanderDirection = transform.forward;
    }

    private void Start()
    {
        player = GameObject.Find("player").transform;
    }

    public void Update()
    {
        repathTimer -= Time.deltaTime;

        if (repathTimer <= 0)
        {
            repathTimer = repathInterval;

            if (usingPath)
                CalculatePath();
        }
        bool canSeePlayer = los.IsRange(transform, player) && !los.IsObstacle(transform, player);
        //Debug.Log(fsm._currentState);

        fsm.UpdateState(canSeePlayer);

        Move(dir);
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

        Debug.Log(closest);
        return closest;
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
    private bool HasLineOfSight(Node from, Node to)
    {
        return !los.IsObstacle(from.transform, to.transform);
    }
    private void CalculatePath()
    {
        Node start = GetClosestNode(transform.position);
        Node goal = GetClosestNode(player.transform.position);
        Debug.Log("CALCULANDO CAMINO");
        Debug.Log("Nodo actual: " + currentNodeIndex + "/" + currentPath.Count);

        currentPath = ThetaStar.Run(
    start,
    node => node == goal,
    node => node.neightbourds,
    (a, b) => Vector3.Distance(a.transform.position, b.transform.position),
    node => Vector3.Distance(node.transform.position, goal.transform.position),
    (a, b) => HasLineOfSight(a, b)
);

        if (currentPath.Count == 0)
        {
            usingPath = false;
            return;
        }

        currentNodeIndex = 1;
        usingPath = true;
    }
    private void FollowPath()
    {
        if (!usingPath)
            return;

        if (currentNodeIndex >= currentPath.Count)
        {
            usingPath = false;
            ignoreObstacles = false;
            Debug.Log("Chauuuuuuuuuuuuu");
            return;
        }

        Vector3 target = currentPath[currentNodeIndex].transform.position;

        dir = SteeringBehaviours.Seek(transform, target);

        Debug.Log(Vector3.Distance(transform.position, target));

        if (Vector3.Distance(transform.position, target) < 1f)
        {
            Debug.Log("Llegó al nodo: " + currentPath[currentNodeIndex].name);
            CalculatePath();
            //currentNodeIndex++;

            Debug.Log("Siguiente índice: " + currentNodeIndex);
        }
    }

    public void Pursue()
    {
        Vector3 direction = player.transform.position - transform.position;
        direction.y = 0;
        Vector3 moveDirection = direction.normalized;

        transform.position += moveDirection * speed * Time.deltaTime;

        transform.forward = Vector3.Lerp(transform.forward, moveDirection, Time.deltaTime * rotationSpeed);
    }
    public void FreezePlayer(bool freeze)
    {
        Rigidbody rb = player.GetComponent<Rigidbody>();

        if (rb != null)
        {
            rb.velocity = Vector3.zero;
            rb.isKinematic = freeze;
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player") && canFreeze)
        {
            fsm.ToFreeze();
            StartCoroutine(FreezeCooldownRoutine());
        }
    }

    private IEnumerator FreezeCooldownRoutine()
    {
        canFreeze = false;

        yield return new WaitForSeconds(freezeCooldown);

        canFreeze = true;
    }
    public void SetDirection(Vector3 newDir)
    {
        dir = newDir;
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

        Debug.Log("vAYA");
    }
    public void Seek()
    {
        dir = SteeringBehaviours.Seek(transform, player.transform.position);

        Debug.Log("vAYA123");
    }
    public void PursueStar()//usa theta solo cambia el nombre
    {
        if (usingPath)
        {
            FollowPath();

            if (!usingPath)
            {
                dir = SteeringBehaviours.Seek(transform, player.position);
            }

            return;
        }

        if (los.ObstacleAhead(transform, 2f) && ignoreObstacles == false)
        {
            Debug.Log("Recalculo");
            ignoreObstacles = true;
            CalculatePath();
        }
        else
        {
            dir = SteeringBehaviours.Seek(transform, player.position);
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
    public void FreezePlayer(float duration)
    {
        if (freezeRoutine != null)
            StopCoroutine(freezeRoutine);

        freezeRoutine = StartCoroutine(FreezeCoroutine(duration));
    }

    private IEnumerator FreezeCoroutine(float duration)
    {
        Rigidbody rb = player.GetComponent<Rigidbody>();

        if (rb != null)
        {
            rb.velocity = Vector3.zero;
            rb.isKinematic = true;
        }

        yield return new WaitForSeconds(duration);

        if (rb != null)
        {
            rb.isKinematic = false;
        }
    }
}