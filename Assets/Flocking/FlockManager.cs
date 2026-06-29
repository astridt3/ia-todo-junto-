using System.Collections.Generic;
using UnityEngine;

public class FlockManager : MonoBehaviour
{
    [Header("Spawn")]
    [SerializeField] private FlockAgent agentPrefab;
    [SerializeField] private int agentCount = 10;
    [SerializeField] private Vector3 spawnExtents = new Vector3(1f, 1f, 1f);

    [Header("Movement")]
    [SerializeField] private float minSpeed = 4f;
    [SerializeField] private float maxSpeed = 8f;
    [SerializeField] private float maxForce = 10f;

    [Header("Neighbors")]
    [SerializeField] private float neighborRadius = 4f;
    [SerializeField] private float separationRadius = 0.1f;

    [Header("Weights")]
    [SerializeField] private float separationWeight = 0.1f;
    [SerializeField] private float alignmentWeight = 1f;
    [SerializeField] private float cohesionWeight = 1f;
    [SerializeField] private float targetWeight = 0.6f;
    [SerializeField] private float boundsWeight = 1.5f;

    [Header("Optional Target")]
    [SerializeField] private Transform globalTarget;

    private readonly List<FlockAgent> agents = new List<FlockAgent>();
    [SerializeField] private float spawnInterval = 30f;
    public List<FlockAgent> Agents => agents;
    public float MinSpeed => minSpeed;
    public float MaxSpeed => maxSpeed;
    public float MaxForce => maxForce;
    public float NeighborRadius => neighborRadius;
    public float SeparationRadius => separationRadius;
    public float SeparationWeight => separationWeight;
    public float AlignmentWeight => alignmentWeight;
    public float CohesionWeight => cohesionWeight;
    public float TargetWeight => targetWeight;
    public float BoundsWeight => boundsWeight;
    public Transform GlobalTarget => globalTarget;
    public Vector3 BoundsCenter => transform.position;
    public Vector3 BoundsExtents => spawnExtents;
    [SerializeField] private Transform[] spawnPoints;


    private void Start()
    {
        SpawnAgents();
        InvokeRepeating(nameof(SpawnAgents), spawnInterval, spawnInterval);
    }
    private void SpawnAgents()
    {
        Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];

        for (int i = 0; i < agentCount; i++)
        {
            Vector3 spawnPosition = spawnPoint.position;

            Quaternion spawnRotation = spawnPoint.rotation;

            FlockAgent newAgent = Instantiate(agentPrefab, spawnPosition, spawnRotation, transform);
            newAgent.Initialize(this);
            agents.Add(newAgent);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireCube(transform.position, spawnExtents * 2f);
    }
}
