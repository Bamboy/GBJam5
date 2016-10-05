﻿using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour 
{
	public static CameraController instance;
	void Awake(){ instance = this; }

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
					StartCoroutine("MoveCamera", new Vector3(targetX * CameraUpscale.Res.x, targetY * -CameraUpscale.Res.y ));
				}
			}
			else if( Input.GetKey( KeyCode.S ) )
			{
				if( targetY < yMax )
				{
					targetY++;
					StartCoroutine("MoveCamera", new Vector3(targetX * CameraUpscale.Res.x, targetY * -CameraUpscale.Res.y ));
				}
			}
			else if( Input.GetKey( KeyCode.A ) )
			{
				if( targetX > 0 )
				{
					targetX--;
					StartCoroutine("MoveCamera", new Vector3(targetX * CameraUpscale.Res.x, targetY * -CameraUpscale.Res.y ));
				}
			}
			else if( Input.GetKey( KeyCode.D ) )
			{
				if( targetX < xMax )
				{
					targetX++;
					StartCoroutine("MoveCamera", new Vector3(targetX * CameraUpscale.Res.x, targetY * -CameraUpscale.Res.y ));
				}
			}
		}
	}


	IEnumerator MoveCamera( Vector3 pos )
	{
		yield return null;
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