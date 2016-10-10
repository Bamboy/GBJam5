using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class MenuChoice : MonoBehaviour 
{
	public Transform indicator;
	public Animator anim;
	public static bool showingCredits = false;

	// Update is called once per frame
	void Update () 
	{

		if( showingCredits )
		{
			if( Input.GetMouseButtonDown( 0 ) )
			{
				showingCredits = false;
				anim.Play("menu_intro");

			}

		}
		else
		{
			RaycastHit data = CameraUpscale.instance.DownscaledMouseRaycast();

			if( data.collider == GetComponent<Collider>() )
			{
				indicator.position = transform.position;

				if( Input.GetMouseButtonUp(0) )
				{
					if( gameObject.name == "Start" )
					{
						SceneManager.LoadScene( 1 ); //Load the game
					}
					else if( gameObject.name == "About" )
					{
						anim.Play("menu_credits");
						showingCredits = true;
					}
					else if( gameObject.name == "Menu" )
					{
						SceneManager.LoadScene( 0 );
					}


				}

			}
		}
	}
}
