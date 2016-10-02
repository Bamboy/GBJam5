using UnityEngine;
using System.Collections;

public class CameraUpscale : MonoBehaviour 
{
	public enum ScaleMults { One = 1, Two = 2, Three = 3, Four = 4, Five = 5, Six = 6 }
	public static readonly Vector2 Res = new Vector2( 160.0f, 144.0f );

	private ScaleMults scale = ScaleMults.Two;
	public ScaleMults Scaling
	{
		get{
			return scale;
		}
		set{
			if( value != scale )
			{
				scale = value; 
				UpdateScaling();
			}
		}
	}

	public Transform renderQuad;
	private PixelPerfectCam renderPPC;
	private int scaleMult = 2;

	void Start () 
	{
		renderPPC = GetComponent<PixelPerfectCam>();

		Scaling = ScaleMults.Six;
	}

	void UpdateScaling()
	{
		int width = Mathf.RoundToInt( Res.x * (int)scale );
		int height = Mathf.RoundToInt( Res.y * (int)scale );

		renderPPC.targetViewportSizeInPixels = Res * (int)scale;
		renderQuad.transform.localScale = new Vector3( width, height, 1 );

		Screen.SetResolution( width, height, false, 60 );
	}


	void Update()
	{
		if( Input.GetKeyDown(KeyCode.LeftArrow) )
			scaleMult = Mathf.Clamp( scaleMult - 1, 1, 6 );
		if( Input.GetKeyDown(KeyCode.RightArrow) )
			scaleMult = Mathf.Clamp( scaleMult + 1, 1, 6 );

		Scaling = (ScaleMults)scaleMult;
		//Scaling = ScaleMults.Six;
	}
}
