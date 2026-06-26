using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour
{
    public List<Node> neightbourds = new List<Node>();

    public bool hasTrap;

    private Renderer rend;

    private void Awake()
    {
        rend = GetComponent<Renderer>();
    }

    private void Update()
    {
        if (rend != null)
            rend.material.color = hasTrap ? Color.red : Color.white;
    }
}