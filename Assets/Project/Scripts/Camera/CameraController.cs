using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour 
{
	public static CameraController instance;
	void Awake(){ instance = this; }

	public float scrollSpeed = 4f;
	public int targetX;
	public int targetY;

	public int xMax = 2;
	public int yMax = 2;

	public Transform camera;
	Vector3 targetPos;
	Vector3 cameraDefault;
	void Start()
	{
		cameraDefault = camera.localPosition;
	}
	
	bool acceptInputs = true;
	void Update () 
	{
		if( Input.GetKeyDown( KeyCode.P ) && Deck.instance.doDebug  )
			CameraController.instance.Shake( 0.3f, 8f );

		if( acceptInputs )
		{
			if( Input.GetKeyDown( KeyCode.W ) )
			{
				if( targetY > 0 )
				{
					targetY--;
				}
			}
			else if( Input.GetKeyDown( KeyCode.S ) )
			{
				if( targetY < yMax )
				{
					targetY++;
				}
			}
			else if( Input.GetKeyDown( KeyCode.A ) )
			{
				if( targetX > 0 )
				{
					targetX--;
				}
			}
			else if( Input.GetKeyDown( KeyCode.D ) )
			{
				if( targetX < xMax )
				{
					targetX++;

				}
			}
		}
		targetPos = new Vector3(targetX * CameraUpscale.Res.x, targetY * -CameraUpscale.Res.y );

		transform.position = Vector3.MoveTowards( transform.position, targetPos, scrollSpeed );
	}




	public AnimationCurve shakeFalloff;
	float shakeStartTime;
	float shakeEndTime;
	float shakeintensity;

	public void Shake( float duration, float intensity )
	{
		StopCoroutine( "ShakeCamera" );
		shakeEndTime = Time.time + duration;
		shakeintensity = intensity;
		StartCoroutine( "ShakeCamera" );
	}
	IEnumerator ShakeCamera()
	{
		while( Time.time < shakeEndTime )
		{
			Vector3 sphere = Random.insideUnitSphere;
			sphere.z = 0f;
			sphere = sphere * shakeFalloff.Evaluate( VectorExtras.ReverseLerp( Time.time, shakeStartTime, shakeEndTime ) ) * shakeintensity;
			sphere = new Vector3( Mathf.Ceil(sphere.x), Mathf.Ceil(sphere.y), 0f );
			camera.localPosition = cameraDefault + sphere;

			yield return null;
		}
		camera.localPosition = cameraDefault;
	}
}
