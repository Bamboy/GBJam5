using UnityEngine;
using System.Collections;

public class ProjectileTower : Tower
{
	public GameObject projectileVisual;
	public float projectileSpeed = 128f;

	public override void Start() 
	{
		base.Start();
		onTick = (Structure owner) => 
		{
			if( target != null )
			{
				Fire();
			}
			else
				timeLeft = 0.001f; //Don't waste the attack timer until we actually can attack an enemy.
		};
	}
	public override void Update () 
	{
		base.Update();
	}

	private void Fire()
	{
		//Create projectile

		GameObject p = GameObject.Instantiate( projectileVisual, center, Quaternion.identity, transform ) as GameObject;
		Projectile proj = p.GetComponent<Projectile>();
		proj.speed = projectileSpeed;
		proj.owner = this;
		proj.target = target;



	}
	public void ProjectileHit( Enemy e )
	{
		e.Health -= damage;
	}
}
