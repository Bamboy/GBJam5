using UnityEngine;
using System.Collections;

public class Blade : Structure
{
	public int damage = 1;
	public float pivotSpeed = 20f;
	private Transform pivot;

	public override void Start()
	{
		base.Start();
		pivot = transform.GetChild(0);
		onTick = (Structure owner) => 
		{
			foreach (Enemy target in Enemy.GetEnemiesInRange(center, areaOfInfluence)) 
			{
				target.Health -= damage;
			}
		};
	}


	public override void Update()
	{
		base.Update();

		pivot.Rotate( 0f, 0f, pivotSpeed * Time.deltaTime );

	}

}
