using UnityEngine;
using System.Collections;

public class DrawGrid : MonoBehaviour 
{
	public Color screenEdgeColor = Color.blue;


	void OnDrawGizmos()
	{
		Gizmos.color = screenEdgeColor;
		for (int i = 0; i < (CameraUpscale.Res.x * 4); i += (int)CameraUpscale.Res.x) 
		{
			
			for (int j = 0; j < (CameraUpscale.Res.y * 4); j += (int)CameraUpscale.Res.y) 
			{
				
				Vector3 center = new Vector3( i, -j, 0 );
				Gizmos.DrawLine( center + new Vector3( -16, 0, 0 ), center + new Vector3( 16, 0, 0 ) );

				Gizmos.DrawLine( center + new Vector3( 0, -16, 0 ), center + new Vector3( 0, 16, 0 ) );

			}
		}


	}
}
