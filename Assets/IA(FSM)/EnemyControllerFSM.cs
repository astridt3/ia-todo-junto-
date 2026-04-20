using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class EnemyControllerFSM : MonoBehaviour

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
    private FSMClasses fsm;
    private Coroutine freezeRoutine;
    [SerializeField] private float freezeCooldown = 5f;
    private bool canFreeze = true;

    private void Awake()
    {
        fsm = GetComponent<FSMClasses>();
        los = GetComponent<LineOfSight>();
        wanderDirection = transform.forward;
    }

    private void Start()
    {
        player = GameObject.Find("player").transform;
    }

    public void Update()
    {
        bool canSeePlayer = los.IsRange(transform, player) && !los.IsObstacle(transform, player);
        Debug.Log(canSeePlayer);
        fsm.UpdateState(canSeePlayer);

        Move(dir);
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