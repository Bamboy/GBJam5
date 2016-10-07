using UnityEngine;
using System.Collections;

public class WaypointManager : MonoBehaviour 
{
	public static WaypointManager instance;
	void Awake () 
	{
		instance = this;
	}

	public Color gizmoNodeColor = Color.red;
	public Color gizmoPathColor = Color.red;
	public Transform[] waypoints = new Transform[0];

	void Update()
	{
		SlowAura.Evalulate();
	}


	
	void OnDrawGizmos()
	{
		Gizmos.color = gizmoNodeColor;
		foreach (Transform t in waypoints) 
		{
			Gizmos.DrawWireSphere( t.position, 8f );
		}
		Gizmos.color = gizmoPathColor;
		for (int i = 1; i < waypoints.Length; i++) 
		{
			Gizmos.DrawLine( waypoints[ i-1 ].position, waypoints[ i ].position );
		}
	}
}
