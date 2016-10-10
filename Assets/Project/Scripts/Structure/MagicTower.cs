using UnityEngine;
using System.Collections;

public class MagicTower : ProjectileTower 
{

	//public Transform dartVisual;
	public float percentageDamage = 0.1f;

	public override void Start() 
	{
		base.Start();


		findTarget = FindTargetMostHP;
	}

	private static Enemy FindTargetMostHP( Tower owner )
	{
		int mostHP = -1;
		int index = -1;

		for( int i = 0; i < Enemy.alive.Length; i++ )
		{
			float dist = Vector3.Distance( owner.center, Enemy.alive[i].transform.position );
			if( dist < owner.areaOfInfluence )
			{
				if( Enemy.alive[i].Health > mostHP )
				{
					mostHP = Enemy.alive[i].Health;
					index = i;
				}
			}
		}

		if( index == -1 )
			return null;
		else
			return Enemy.alive[ index ];
	}

	public override void Update () 
	{
		base.Update();

		//if( target != null )
		//{
		//	dartVisual.LookAt( target.transform.position + target.centerOffset );
		//}
	}
	public override void ProjectileHit( Enemy e )
	{

		e.Health = Mathf.CeilToInt( e.Health - (e.Health * percentageDamage) );

		//e.AddStatus("Poison");
	}


}
