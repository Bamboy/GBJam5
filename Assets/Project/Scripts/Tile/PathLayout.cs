using UnityEngine;
using System.Collections;

[RequireComponent(typeof(SpriteRenderer))]
public class PathLayout : MonoBehaviour 
{
	public enum TileDirection{ Center, Up, Right, Down, Left, UpRight, UpLeft, DownRight, DownLeft }
	public PathSet tileSet;
	public bool placeUp = true;
	public bool placeDown = true;
	public bool placeLeft = false;
	public bool placeRight = false;

	public int tileSize = 16;

	private SpriteRenderer sprite;
	void Awake () 
	{
		sprite = GetComponent<SpriteRenderer>();
		Build();
		Destroy(this);
	}


	void Build()
	{
		Vector2 spriteSize = new Vector2(sprite.sprite.bounds.size.x / transform.localScale.x, sprite.sprite.bounds.size.y / transform.localScale.y);



		//childPrefab.transform.position = transform.position;
		//childSprite.sprite = sprite.sprite;

		int tileCountX = Mathf.RoundToInt(transform.localScale.x);
		int tileCountY = Mathf.RoundToInt(transform.localScale.y);

		GameObject child;
		for (int y = 0; y < tileCountY; y++) 
		{
			for (int x = 0; x < tileCountX; x++) 
			{
				child = new GameObject();
				SpriteRenderer childSprite = child.AddComponent<SpriteRenderer>();

				child.name = "tile: " + (x+1) + ", " + (y+1);
				child.transform.position = transform.position + (new Vector3(tileSize * x, -tileSize * y, 0));
				child.transform.parent = transform;

				childSprite.sprite = GetFromSet( TileDirection.Center );

				childSprite.sortingLayerName = sprite.sortingLayerName;
				childSprite.sortingOrder = sprite.sortingOrder;

				if( x == 0 && placeLeft ) //Place a tile left
				{
					CreateTile( TileDirection.Left, x - 1, y );
				}
				if( x == tileCountX - 1 && placeRight ) //Place a tile right
				{
					CreateTile( TileDirection.Right, x + 1, y );
				}

				if( y == 0 && placeUp ) //Place a tile up
				{
					CreateTile( TileDirection.Up, x, y - 1 );
				}
				if( y == tileCountY - 1 && placeDown ) //Place a tile right
				{
					CreateTile( TileDirection.Down, x, y + 1 );
				}
			}
		}

		sprite.enabled = false;
	}

	void CreateTile( TileDirection dir, int x, int y )
	{
		GameObject child = new GameObject();
		SpriteRenderer childSprite = child.AddComponent<SpriteRenderer>();

		child.name = "tile fringe " + dir.ToString();
		child.transform.position = transform.position + (new Vector3(tileSize * x, -tileSize * y, 0));
		child.transform.parent = transform;

		childSprite.sprite = GetFromSet( dir );

		childSprite.sortingLayerName = sprite.sortingLayerName;
		childSprite.sortingOrder = sprite.sortingOrder;
	}

	Sprite GetFromSet( TileDirection dir )
	{
		switch( dir	)
		{
		case TileDirection.Center:
			return ExtRandom<Sprite>.Choice( tileSet.pathFill );
		case TileDirection.Up:
			return ExtRandom<Sprite>.Choice( tileSet.fringeUp );
		case TileDirection.Left:
			return ExtRandom<Sprite>.Choice( tileSet.fringeLeft );
		case TileDirection.Right:
			return ExtRandom<Sprite>.Choice( tileSet.fringeRight );
		case TileDirection.Down:
			return ExtRandom<Sprite>.Choice( tileSet.fringeDown );
		case TileDirection.UpLeft:
			return ExtRandom<Sprite>.Choice( tileSet.fringeUpLeft );
		case TileDirection.UpRight:
			return ExtRandom<Sprite>.Choice( tileSet.fringeUpRight );
		case TileDirection.DownLeft:
			return ExtRandom<Sprite>.Choice( tileSet.fringeDownLeft );
		case TileDirection.DownRight:
			return ExtRandom<Sprite>.Choice( tileSet.fringeDownRight );
		default: return null;
		}
	}
}
