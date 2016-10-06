using UnityEngine;
using System.Collections;

public class ProjectileTower : Tower
{
	public GameObject projectile;

	public override void Start() 
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
}
