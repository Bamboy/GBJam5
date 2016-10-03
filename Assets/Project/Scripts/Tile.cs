using UnityEngine;
using System.Collections;

[CreateAssetMenu(menuName = "Tile")]
public class Tile : ScriptableObject 
{
	public enum FlipOptions { No = 0, Yes = 1, Random = 2 }
	public enum SpriteSize { x4 = 4, x8 = 8, x16 = 16, x32 = 32 }


	public string tileName = "New tile";
	public SpriteSize spriteSize = SpriteSize.x16;
	public bool isBuildable = true;
	public Sprite[] images;

	public FlipOptions flipX = FlipOptions.No;
	public FlipOptions flipY = FlipOptions.No;

	public bool isDetail = false;
	public bool acceptDetail = false;

	//[System.Serializable]

}
