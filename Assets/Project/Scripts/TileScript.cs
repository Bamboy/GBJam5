using UnityEngine;
using System.Collections;



//This goes on tiles we actually create. Return info about the tile.

public class TileScript : MonoBehaviour 
{
	public bool isBuildable = false;
	public bool hasStructure = false;

	public Sprite[] imageVariants = new Sprite[0];



	void Start () 
	{
		hasStructure = false;

		if( imageVariants.Length > 0 )
		{
			SpriteRenderer spr = GetComponent<SpriteRenderer>();
			spr.sprite = imageVariants[ Mathf.FloorToInt( Random.value * imageVariants.Length ) ];
		}
	}
}
