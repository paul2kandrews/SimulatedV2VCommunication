using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vehicle : MonoBehaviour
{
    public Node nodeStart;
    public Node nodeEnd;
    private Graph graph;
    private Follower follower;

    void Start() 
    {
        // Debug.Log("Started getting path for car.");
        // Path path = getPath(nodeStart, nodeEnd);
        
        // Debug.Log("Path received. Following path...");
        // followPath(path);
    }

    void Awake() 
    {
        graph = GameObject.Find("GraphManager").GetComponent<Graph>();
        follower = GetComponent<Follower>();
    }

    public void initCar() 
    {
        // Debug.Log(graph.nodes.Count);
        getStartAndDestination();

        Debug.Log("Started getting path for car.");
        Path path = getPath(nodeStart, nodeEnd);
        
        Debug.Log("Path received. Following path...");
        followPath(path);
    }

    // function to get two nodes to find a path
    void getStartAndDestination()
    {
        var graphNodeCount = graph.nodes.Count;

        var a = Random.Range(0, graphNodeCount);
        var b = Random.Range(0, graphNodeCount);

        nodeStart = graph.nodes[a];
        nodeEnd = graph.nodes[b];
        while (nodeStart == nodeEnd)
        {
            b = Random.Range(0, graphNodeCount);
            nodeEnd = graph.nodes[b];
        }
    }

    Path getPath(Node start, Node end)
    {
        return graph.GetShortestPath(start, end);
    }

    void followPath(Path path) 
    {
        follower.Follow(path);
    }

    void Update() {
        // Delete car when it reaches the end of its trip
        if (this.transform.position == nodeEnd.transform.position) Destroy(this.gameObject);
    }

    // function to detect collision with other cars
    // void onTriggerEnter(Collider col) {
    //     Debug.Log("Collision detected.");
    //     if (col.CompareTag ("CFront")) {
    //         // todo: insert game logic
    //         Debug.Log("Collision detected with car in front.");
    //     } else if (col.CompareTag ("CBack")) {
    //         // todo: insert game logic
    //         Debug.Log("Collision detected with car behind.");
    //     } else if (col.CompareTag ("CGirthy")) {
    //         // todo: insert game logic
    //         Debug.Log("Collision detected with car on the side.");
    //     }
    // }
}

