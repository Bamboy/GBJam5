using UnityEngine;
using System.Collections;

public class Tower : Structure
{
	public int damage = 1;
	public Enemy target;

	public override void Start () 
	{
		base.Start();
		onTick = (Structure owner) => 
		{
			if( target != null )
				target.Health -= damage;
			else
				timeLeft = 0.001f; //Don't waste the attack timer until we actually can attack an enemy.
		};
	}

	public override void Update () 
	{
		target = findTarget( this );
		base.Update();
	}

	public delegate Enemy TargetFilterMethod( Tower owner );
	public TargetFilterMethod findTarget = FindTargetClosest;

	private static Enemy FindTargetClosest( Tower owner )
	{
		//Default sort based on distance to the enemy
		float closestDist = Mathf.Infinity;
		int closestIndex = -1;

		for( int i = 0; i < Enemy.alive.Length; i++ )
		{
			float dist = Vector3.Distance( owner.center, Enemy.alive[i].transform.position );
			if( dist < owner.areaOfInfluence )
			{
				if( dist < closestDist )
				{
					closestDist = dist;
					closestIndex = i;
				}
			}
		}

		if( closestIndex == -1 )
			return null;
		else
			return Enemy.alive[ closestIndex ];
	}
}
