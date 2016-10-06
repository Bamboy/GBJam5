using UnityEngine;
using System.Collections;

[CreateAssetMenu(menuName="Path Set")]
public class PathSet : ScriptableObject
{
	public Sprite[] pathFill;

	public Sprite[] fringeUp;
	public Sprite[] fringeDown;
	public Sprite[] fringeLeft;
	public Sprite[] fringeRight;

	public Sprite[] fringeUpLeft;
	public Sprite[] fringeUpRight;
	public Sprite[] fringeDownLeft;
	public Sprite[] fringeDownRight;
}
