using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class Fontsize : MonoBehaviour 
{

	public Font font;
	private Font lastfont;
	void Update () 
	{
		#if UNITY_EDITOR

		if( font != lastfont )
		{

			foreach (Transform child in this.transform) 
			{
				TextMesh tm = child.GetComponent<TextMesh>();
				if( tm != null )
				{
					tm.font = font;
					tm.font.material.mainTexture.filterMode = FilterMode.Point;
					//tm.
				}
			}
		
		
			
		

		}

		lastfont = font;


		#endif
	}
}
