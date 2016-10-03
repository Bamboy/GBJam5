using UnityEngine;
using System.Collections;



//This goes on tiles we actually create. Return info about the tile.

public class TileScript : MonoBehaviour 
{
	public bool structureCanBePlaced = false;
	public bool hasStructure = false;

	public string tileAssetName = "";
	public int posX;
	public int posY;
	public void Initalize( Tile asset, int x, int y )
	{
		tileAssetName = asset.tileName;

		SpriteRenderer spr = GetComponent<SpriteRenderer>();
		spr.sprite = asset.images[ Mathf.FloorToInt( Random.value * asset.images.Length ) ];

		if( asset.flipX == Tile.FlipOptions.Random )
			spr.flipX = Random.value > 0.5f ? true : false;
		else
			spr.flipX = asset.flipX == Tile.FlipOptions.No ? false : true;

		if( asset.flipY == Tile.FlipOptions.Random )
			spr.flipY = Random.value > 0.5f ? true : false;
		else
			spr.flipY = asset.flipY == Tile.FlipOptions.No ? false : true;

		structureCanBePlaced = asset.isBuildable;

		posX = x;
		posY = y;

		transform.position = new Vector3( x * 16, y * 16, 0 );
	}




	void Start () 
	{
		
	}
	
	void OnMouseEnter()
	{
		Debug.Log("Enter");
	}
	void OnMouseExit()
	{
		Debug.Log("Exit");
	}
}
