using UnityEngine;
using System.Collections;

[RequireComponent(typeof(BoxCollider))]
public class Structure : MonoBehaviour 
{
	public static Structure[] structures;

	public virtual void Start()
	{
		if( structures == null )
			structures = new Structure[0];
		structures = ArrayTools.PushLast( structures, this );
	}

	public float areaOfInfluence = 32f; //Tower attack range and other things
	public float tickTime = 1f; //Attack speed, "do this every X seconds"

	public delegate void OnTickAction( Structure owner );
	public OnTickAction onTick = (Structure owner) => { Debug.Log("I don't do anything."); };

	internal float timeLeft;
	public virtual void Update()
	{
		timeLeft -= Time.deltaTime;
		if( timeLeft <= 0f )
		{
			timeLeft = tickTime;
			onTick( this );
		}
	}

	public virtual void OnDestroy()
	{
		structures = ArrayTools.Remove( structures, this );
	}

	public Vector3 center //Center of the tile the structure is placed on (rather than top left)
	{
		get{ return transform.position + new Vector3( 8, -8, 0 ); }
	}

	void OnDrawGizmos()
	{
		Gizmos.color = Color.magenta;
		Gizmos.DrawWireSphere( center, areaOfInfluence );
	}
}
