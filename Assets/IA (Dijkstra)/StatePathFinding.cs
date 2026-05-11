using System.Collections.Generic;
using UnityEngine;

public class StatePathFinding : MonoBehaviour
{
    [SerializeField] private Node goal;
    [SerializeField] private Node end;

    public void SetPath()
    {
        List<Node> path = BFS.Run(goal, IsSatisfied, GetConections);

        // A partir de aca implementación del profe

        List<Vector3> points = new List<Vector3>();

        for (int i = 0; i < points.Count; i++)
        {
            points.Add(path[i].transform.position);
        }

        //Aca iría un metodo del profe que hace que se ponga Crash en la posición inicial
    }

    public void SetPatDijkstrah()
    {
        List<Node> path = Dijkstra.Run(goal, IsSatisfied, GetConections, GetCosts);
        List<Vector3> points = new List<Vector3>();

        // A partir de aca implementación del profe

        for (int i = 0; i < points.Count; i++)
        {
            points.Add(path[i].transform.position);
        }

        //Aca iría un metodo del profe que hace que se ponga Crash en la posición inicial
    }

    public bool IsSatisfied(Node node)
    {
        if (node == end)
        {
            return true;
        }

        return false;
    }

    public List<Node> GetConections(Node node)
    {
        return node.neightbourds;
    }

    public float GetCosts(Node node1, Node node2)
    {
        float costs = 0.0f;
        costs += Vector3.Distance(node1.transform.position, node2.transform.position);
        if (node2.hasTrap)
        {
            costs += 100;
        }

        return costs;
    }
}
