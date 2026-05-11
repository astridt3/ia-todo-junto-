using System.Collections.Generic;
using UnityEngine;

public class EnemyPathfinding : MonoBehaviour
{
    [SerializeField] private List<Node> patrolPoints;
    [SerializeField] private float speed = 3f;
    [SerializeField] private Transform player;
    [SerializeField] private EnemyController enemyController;

    private LineOfSight los;

    private int currentPatrolIndex;

    private List<Node> currentPath = new List<Node>();

    private int currentPathIndex;

    private Node currentNode;

    private bool chasing;

    private void Start()
    {
        currentNode = GetClosestNode();

        los = GetComponent<LineOfSight>();
    }

    private void Update()
    {
        if (CanSeePlayer())
        {
            chasing = true;

            Node playerNode = GetClosestNodeToPlayer();

            CalculatePath(playerNode);

            AlertEnemy();
        }

        if (chasing)
        {
            FollowPath();
        }
        else
        {
            Patrol();
        }
    }

    void Patrol()
    {
        if (patrolPoints.Count == 0)
            return;

        Node target = patrolPoints[currentPatrolIndex];

        MoveTo(target.transform.position);

        if (Vector3.Distance(transform.position,
            target.transform.position) < 0.5f)
        {
            currentPatrolIndex++;

            if (currentPatrolIndex >= patrolPoints.Count)
            {
                currentPatrolIndex = 0;
            }
        }
    }

    void FollowPath()
    {
        if (currentPath.Count == 0)
            return;

        if (currentPathIndex >= currentPath.Count)
        {
            chasing = false;
            currentPath.Clear();
            return;
        }
        Node targetNode = currentPath[currentPathIndex];
        MoveTo(targetNode.transform.position);
        if (Vector3.Distance(transform.position,
     targetNode.transform.position) < 1f)
        {
            currentPathIndex++;
        }
    }

    void MoveTo(Vector3 target)
    {
        Debug.Log("Me estoy moviendo");
        Vector3 dir =
            (target - transform.position).normalized;

        transform.position +=
            dir * speed * Time.deltaTime;

        if (dir != Vector3.zero)
        {
            transform.forward = dir;
        }
    }

    bool CanSeePlayer()
    {
        return
            los.IsRange(transform, player) &&
            los.IsAngle(transform, player) &&
            !los.IsObstacle(transform, player);
    }

    void AlertEnemy()
    {
        enemyController.Alert();
    }

    void CalculatePath(Node targetNode)
    {
        currentNode = GetClosestNode();

        currentPath = Dijkstra.Run(
            currentNode,
            x => x == targetNode,
            GetConnections,
            GetCosts
        );

        currentPathIndex = 0;
    }

    List<Node> GetConnections(Node node)
    {
        return node.neightbourds;
    }

    float GetCosts(Node a, Node b)
    {
        return Vector3.Distance(
            a.transform.position,
            b.transform.position);
    }

    Node GetClosestNode()
    {
        Node[] allNodes = FindObjectsOfType<Node>();

        Node closest = null;

        float minDist = Mathf.Infinity;

        foreach (Node node in allNodes)
        {
            float dist =
                Vector3.Distance(transform.position,
                node.transform.position);

            if (dist < minDist)
            {
                minDist = dist;
                closest = node;
            }
        }

        return closest;
    }

    Node GetClosestNodeToPlayer()
    {
        Node[] allNodes = FindObjectsOfType<Node>();

        Node closest = null;

        float minDist = Mathf.Infinity;

        foreach (Node node in allNodes)
        {
            float dist =
                Vector3.Distance(player.position,
                node.transform.position);

            if (dist < minDist)
            {
                minDist = dist;
                closest = node;
            }
        }

        return closest;
    }
}