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
	Vector3 cameraDefault;
	void Start()
	{
		cameraDefault = camera.localPosition;
	}
	
	bool acceptInputs = true;
	void Update () 
	{
		if( Input.GetKeyDown( KeyCode.P ) )
			CameraController.instance.Shake( 0.3f, 8f );

		if( acceptInputs )
		{
			if( Input.GetKey( KeyCode.W ) )
			{
				if( targetY > 0 )
				{
					targetY--;
					targetPos = new Vector3(targetX * CameraUpscale.Res.x, targetY * -CameraUpscale.Res.y );
					StartCoroutine("MoveCamera");
				}
			}
			else if( Input.GetKey( KeyCode.S ) )
			{
				if( targetY < yMax )
				{
					targetY++;
					targetPos = new Vector3(targetX * CameraUpscale.Res.x, targetY * -CameraUpscale.Res.y );
					StartCoroutine("MoveCamera");
				}
			}
			else if( Input.GetKey( KeyCode.A ) )
			{
				if( targetX > 0 )
				{
					targetX--;
					targetPos = new Vector3(targetX * CameraUpscale.Res.x, targetY * -CameraUpscale.Res.y );
					StartCoroutine("MoveCamera");
				}
			}
			else if( Input.GetKey( KeyCode.D ) )
			{
				if( targetX < xMax )
				{
					targetX++;
					targetPos = new Vector3(targetX * CameraUpscale.Res.x, targetY * -CameraUpscale.Res.y );
					StartCoroutine("MoveCamera");
				}
			}
		}
	}

	Vector3 targetPos;
	IEnumerator MoveCamera()
	{
		Debug.LogError("FIX ME");
		yield return null;
		/*
		acceptInputs = false;
		while( true )
		{
			Debug.Log( targetPos );
			Vector3 move = Vector3.MoveTowards(transform.position, targetPos, scrollSpeed);

			move = new Vector3( Mathf.Floor(move.x), Mathf.Floor(move.y), 0f );
			Debug.Log( move );
			transform.position = transform.position + move;

			yield return null;

			if( Vector3.Distance(transform.position, targetPos) < 1f )
				break;
		}
		transform.position = targetPos;

		acceptInputs = true; */
	}

	public AnimationCurve shakeFalloff;
	float shakeStartTime;
	float shakeEndTime;
	float shakeintensity;

	public void Shake( float duration, float intensity )
	{
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
