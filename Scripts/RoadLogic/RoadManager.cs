using System.Collections;
using System.Collections.Generic;
using System.Collections.Concurrent;
using UnityEngine;
using System.Linq;
using Priority_Queue;
using System;

public class RoadManager : MonoBehaviour
{
	public GameObject GraphManager;
	public List<RoadSegment> RoadSegments = new List<RoadSegment>();
	
	void Awake()
	{
		RoadSegments = getRoadSegments();
		connectRoadSegments();
		connectRoadNodes();
		Debug.Log("finished initializing roads and nodes");
	}

	// Finds all road game objects and gets their RoadSegment components
	List<RoadSegment> getRoadSegments() 
	{
		// Get all road game objects in scene
		List<GameObject> roadGameObjects = GameObject.FindGameObjectsWithTag("Road").ToList();

		// Get road data abstractions from road game objects
		List<RoadSegment> vertices = new List<RoadSegment>();
		int id = 0;
		foreach (GameObject obj in roadGameObjects)
		{
			vertices.Add(obj.GetComponent<RoadSegment>());
		}

		foreach (RoadSegment roadSegment in vertices)
		{
			roadSegment.setId(id);
			id++;
			roadSegment.setSpeedLimit(5);
			roadSegment.setPhysicalLength(6);
		}
		return vertices;
	}

    void connectRoadSegments() 
	{
		// Iterate through all road data objects and link them together
		foreach (RoadSegment vertex in RoadSegments) 
		{			
			// Go through each snap point and check if it is connected to another snap point
			foreach (GameObject sp in vertex.snapPoints)
            {
				LayerMask mask = LayerMask.GetMask("SnapPoint");
				float searchRadius = 0.05f;
				// Check for other snap points in the vicinity of this snap point
				var searchResults = Physics.OverlapSphere(sp.transform.position, searchRadius, mask);
				List<Collider> results = searchResults.ToList();
				//results.Remove(this);

				Collider connection = results.FirstOrDefault(x => x.gameObject != sp);
				GameObject connectionGameObject = null;
				GameObject connectionParent = null;
				if (connection != null)
				{
					connectionGameObject = connection.gameObject;
					connectionParent = connection.transform.parent.gameObject; // gets road segment of connection
					
					sp.GetComponent<SnapPoint>().connection = connectionGameObject; // Save connection to snap point itself
				}

				// If connection exists, wire up the approriate type of connection (next or prev)
				if (connectionParent != null)
                {
                    // print("Connection found");
					RoadSegment connectionObj = connectionParent.GetComponent(typeof(RoadSegment)) as RoadSegment;

					// TO-DO: make sure this stuff works
					if (sp.GetComponent<SnapPoint>().isEnd)
						vertex.nexts.Add(connectionObj);
					else
						vertex.prevs.Add(connectionObj);

					// Hide snap points
					sp.GetComponent<MeshRenderer>().enabled = false;
                }
				else
				{
					// print("No connection found");
				}
			}
		}
    }

	void connectRoadNodes()
	{
		foreach (RoadSegment s in RoadSegments) // Convert all road vertices to graph vertices
		{
			// Handle intersections
			if (s.nodes.Count > 1) {
				foreach (Node nodeSelf in s.nodes) {
					SnapPoint spEnd = nodeSelf.spOut.GetComponent<SnapPoint>();
					if (spEnd.connection != null) {
						SnapPoint connectedSp = spEnd.connection.GetComponent<SnapPoint>();
						Node nodeNext = connectedSp.node.GetComponent<Node>();
						nodeSelf.connections.Add(nodeNext);
					}
				}
			}
			// Handle non-intersections
			else {
				Node nodeSelf = s.nodes[0];
				SnapPoint spEnd = nodeSelf.spOut.GetComponent<SnapPoint>();
				if (spEnd.connection != null) {
					SnapPoint connectedSp = spEnd.connection.GetComponent<SnapPoint>();
					Node nodeNext = connectedSp.node.GetComponent<Node>();
					nodeSelf.connections.Add(nodeNext);
				}
			}
		}

		// Make all nodes a child of the graph manager
		var allNodes = GameObject.FindGameObjectsWithTag("Node");
		foreach (GameObject n in allNodes) n.transform.parent = GraphManager.transform;

		// Make the graph class see the children of the graph gameobject as nodes within the graph
		GraphManager.GetComponent<Graph>().nodes.Clear();
		foreach (Transform child in GraphManager.transform)
		{
			Node node = child.GetComponent<Node> ();
			if (node != null)
			{
				GraphManager.GetComponent<Graph>().nodes.Add(node);
			}
		}
	}
}