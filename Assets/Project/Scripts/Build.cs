using UnityEngine;
using System.Collections;

public class Build : MonoBehaviour 
{
	public Sprite canBuild;
	public Sprite cannotBuild;

	public GameObject structure;

	private bool building = false;
	//private bool canBuildHere = false;
	public Transform highlighter;
	SpriteRenderer highlighterSpr;

	void Start()
	{
		highlighterSpr = highlighter.GetComponent<SpriteRenderer>();
		building = false;
		highlighter.gameObject.SetActive( false );
	}


	void Update () 
	{
		if( Input.GetKeyDown(KeyCode.B) )
		{
			building = true;
			highlighter.gameObject.SetActive( true );
		}

		if( building )
		{
			RaycastHit data = CameraUpscale.instance.DownscaledMouseRaycast();
			if( data.point != Vector3.zero )
			{
				//Move indicator

				Vector3 snappedPos = new Vector3( VectorExtras.RoundTo(data.point.x - 8f, 16f), VectorExtras.RoundTo(data.point.y + 8f, 16f), 0f );

				highlighter.transform.position = snappedPos;


				if( data.collider.tag == "Buildable" )
				{
					highlighterSpr.sprite = canBuild;

					if( Input.GetMouseButtonDown(0) )
					{

						//Place structure, disable highlighter
						GameObject newStruct = GameObject.Instantiate( structure, snappedPos, Quaternion.identity ) as GameObject;
						building = false;
						highlighter.gameObject.SetActive( false );

					}


				}
				else
					highlighterSpr.sprite = cannotBuild;

			}
			else
				highlighterSpr.sprite = cannotBuild;

		}


	}
}
