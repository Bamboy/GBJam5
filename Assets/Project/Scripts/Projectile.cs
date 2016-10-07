using UnityEngine;
using System.Collections;

public class Projectile : MonoBehaviour 
{
	public ProjectileTower owner;

	public Enemy target;
	
	public float speed = 1f;

	void Update () 
	{
		if( target == null )
		{
			Destroy( this.gameObject );
			return;
		}

		transform.LookAt( target.transform.position + target.centerOffset );

		transform.position = Vector3.MoveTowards( transform.position, target.transform.position + target.centerOffset, speed * Time.deltaTime );
		if( Vector3.Distance( target.transform.position + target.centerOffset, transform.position ) < 4f )
		{
			owner.ProjectileHit( target );

			//transform.SetParent( target.transform );
			Destroy( this.gameObject );
		}
			

	}
}
