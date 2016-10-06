using UnityEngine;
using System.Collections;

//TAKEN FROM: http://answers.unity3d.com/questions/599263/how-to-make-2d-sprite-tiled.html

// @NOTE the attached sprite's position should be "top left" or the children will not align properly
// Strech out the image as you need in the sprite render, the following script will auto-correct it when rendered in the game

// Generates a nice set of repeated sprites inside a streched sprite renderer
// @NOTE Vertical only, you can easily expand this to horizontal with a little tweaking




[RequireComponent (typeof (SpriteRenderer))]
public class RepeatSpriteBoundary : MonoBehaviour 
{
	public Sprite[] variants;
	SpriteRenderer sprite;

	void Awake () 
	{
		// Get the current sprite with an unscaled size
		sprite = GetComponent<SpriteRenderer>();
		Rebuild();
		Destroy(this);
	}


	void Rebuild()
	{
		Vector2 spriteSize = new Vector2(sprite.sprite.bounds.size.x / transform.localScale.x, sprite.sprite.bounds.size.y / transform.localScale.y);



		//childPrefab.transform.position = transform.position;
		//childSprite.sprite = sprite.sprite;

		int tileCountX = Mathf.RoundToInt(transform.localScale.x);
		int tileCountY = Mathf.RoundToInt(transform.localScale.y);

		GameObject child;
		for (int i = 0; i < tileCountY; i++) 
		{
			/*
			child = Instantiate(childPrefab) as GameObject;
			child.transform.position = transform.position - (new Vector3(0, 16, 0) * i);
			child.transform.parent = transform;

			if( variants.Length > 0 )
			{
				Sprite replacement = ExtRandom<Sprite>.Choice( variants );
				child.GetComponent<SpriteRenderer>().sprite = replacement;
			} */

			for (int j = 0; j < tileCountX; j++) 
			{
				child = new GameObject();
				SpriteRenderer childSprite = child.AddComponent<SpriteRenderer>();

				//child = Instantiate(childPrefab) as GameObject;
				child.name = "tile: " + (j+1) + ", " + (i+1);
				child.transform.position = transform.position + (new Vector3(16 * j, -16 * i, 0));
				child.transform.parent = transform;

				if( variants.Length > 0 )
				{
					Sprite replacement = ExtRandom<Sprite>.Choice( variants );
					childSprite.sprite = replacement;
				}
				else
					childSprite.sprite = sprite.sprite;

				childSprite.sortingLayerName = sprite.sortingLayerName;
				childSprite.sortingOrder = sprite.sortingOrder;
			}
		}




		// Set the parent last on the prefab to prevent transform displacement
		//childPrefab.transform.parent = transform;

		// Disable the currently existing sprite component since its now a repeated image
		sprite.enabled = false;
	}
}