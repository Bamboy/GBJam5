using UnityEngine;
using System.Collections;

public class ArcherTower : ProjectileTower 
{
	public Transform bowVisual;


	public override void Start() 
	{
		base.Start();
	}

	public override void Update () 
	{

		base.Update();

		if( target != null )
		{

			bowVisual.LookAt( target.transform.position + target.centerOffset );
		}
	}
}
