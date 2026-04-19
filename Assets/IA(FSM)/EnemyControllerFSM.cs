using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class EnemyControllerFSM : MonoBehaviour

{
    public Transform player;
    private LineOfSight los;
    //private EnemyTree desicionTree;
    //private EnemyContext context;

    [SerializeField] private float speed = 3;
    [SerializeField] private float rotationSpeed = 33;
    [SerializeField] private float patrolRotationSpeed = 33;

    //[SerializeField] private Material attackMaterial;
    //private Material defaultMaterial;
    //private MeshRenderer renderer;

    private Rigidbody playerRB;
    private Vector3 wanderDirection;
    private float wanderTime;
    [SerializeField] private float WanderchangeInterval = 1.5f;
    private Vector3 dir;
    private bool isAttacking = false;
    private FSMClasses fsm;
    private Coroutine freezeRoutine;


    private void Awake()
    {
        fsm = GetComponent<FSMClasses>();
        los = GetComponent<LineOfSight>();
        //desicionTree = GetComponent<EnemyTree>();
        wanderDirection = transform.forward;
        //c/*ontext = new EnemyContext { self = transform, player = player, los = los };*/

        //dir = Vector3.zero;
        //renderer = GetComponent<MeshRenderer>();

    }

    private void Start()
    {
        player = GameObject.Find("player").transform;

        //renderer = GetComponent<MeshRenderer>();
        //defaultMaterial = GetComponent<MeshRenderer>().material;
    }

    public void Update()
    {
        bool canSeePlayer = los.IsRange(transform, player) && los.IsObstacle(transform, player);
        Debug.Log(canSeePlayer);
        fsm.UpdateState(canSeePlayer);

        Move(dir);
    }
    public void StopAttack()
    {
        isAttacking = false;
    }
    public void Pursuit()
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

    public void Patrol()
    {
        transform.Rotate(0, rotationSpeed * Time.deltaTime, 0);

        //renderer.material = defaultMaterial;
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
        //renderer.material = attackMaterial;
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
    public void FreezePlayer(float duration)
    {
        // evita que se acumulen freezes
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

