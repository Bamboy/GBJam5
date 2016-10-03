using UnityEngine;
using System.Collections;

public class WaypointFollower : MonoBehaviour 
{
	public float speed = 10f;

	private int nextWaypoint = -1;

	void Start () 
	{
		StartCoroutine("MoveToNode");
	}


	IEnumerator MoveToNode()
	{
		nextWaypoint++;

		while( Vector3.Distance(transform.position, WaypointManager.instance.waypoints[ nextWaypoint ].position) > 0.5f )
		{
			Vector3 newPos = transform.position + (VectorExtras.Direction(transform.position, WaypointManager.instance.waypoints[ nextWaypoint ].position) * speed * Time.deltaTime);
			transform.position = newPos;


			yield return null;
		}

		if( WaypointManager.instance.waypoints.Length != nextWaypoint + 1 )
		{
			StartCoroutine("MoveToNode");
		}

	}

}
