using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEditor;

[CustomEditor ( typeof ( Graph ) )]
public class VehicleEditor : Editor
{
	protected Graph m_Graph;
	protected Node m_From;
	protected Node m_To;
	protected Follower m_Follower;
	protected Path m_Path = new Path ();

	

	public override void OnInspectorGUI ()
	{
		m_Graph.nodes.Clear ();
		foreach ( Transform child in m_Graph.transform )
		{
			Node node = child.GetComponent<Node> ();
			if ( node != null )
			{
				m_Graph.nodes.Add ( node );
			}
		}
		base.OnInspectorGUI ();
		EditorGUILayout.Separator ();
		m_From = ( Node )EditorGUILayout.ObjectField ( "From", m_From, typeof ( Node ), true );
		m_To = ( Node )EditorGUILayout.ObjectField ( "To", m_To, typeof ( Node ), true );
		m_Follower = ( Follower )EditorGUILayout.ObjectField ( "Follower", m_Follower, typeof ( Follower ), true );
		if ( GUILayout.Button ( "Show Shortest Path" ) )
		{
			m_Path = m_Graph.GetShortestPath ( m_From, m_To );
			if ( m_Follower != null )
			{
				m_Follower.Follow ( m_Path );
			}
			Debug.Log ( m_Path );
			SceneView.RepaintAll ();
		}
	}
	
}
