using UnityEngine;
using System.Collections;

public class DartTower : ProjectileTower
{
	public Transform dartVisual;


	public override void Start() 
	{
		base.Start();
	}

	public override void Update () 
	{
		base.Update();

		if( target != null )
		{
			dartVisual.LookAt( target.transform.position + target.centerOffset );
		}
	}
	public override void ProjectileHit( Enemy e )
	{
		e.Health -= damage;
		//e.AddStatus("Poison");
	}
}
