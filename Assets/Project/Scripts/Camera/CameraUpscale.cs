using UnityEngine;
using System.Collections;

public class CameraUpscale : MonoBehaviour 
{
	public static CameraUpscale instance;

	public enum ScaleMults { One = 1, Two = 2, Three = 3, Four = 4, Five = 5, Six = 6 }
	public static readonly Vector2 Res = new Vector2( 160.0f, 144.0f );

	private static ScaleMults scale = ScaleMults.Six;
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
	public Camera smallCam;
	private PixelPerfectCam renderPPC;
	private int scaleMult = 6;

	void Awake()
	{
		instance = this;
	}
	void Start () 
	{
		renderPPC = GetComponent<PixelPerfectCam>();

		Scaling = ScaleMults.Six;
		UpdateScaling();

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


		if( Input.GetMouseButton(0) )
		{
			DownscaledMouseRaycast();
		}
	}


	public RaycastHit DownscaledMouseRaycast()
	{
		RaycastHit hit; 
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition); 

		Vector3 downscaledOrigin = (ray.origin - Camera.main.transform.position) / scaleMult;

		Ray transfered = new Ray( downscaledOrigin + smallCam.transform.position, ray.direction );


		Debug.DrawLine( ray.origin, ray.origin + new Vector3( 0, 0, 10000 ), Color.red );
		Debug.DrawLine( transfered.origin, transfered.origin + new Vector3( 0, 0, 10000 ), Color.green );
		Physics.Raycast( transfered, out hit, 10000.0f);

		return hit;
	} 
}
