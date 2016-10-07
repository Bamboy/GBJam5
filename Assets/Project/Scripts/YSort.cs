using UnityEngine;
using System.Collections;

[RequireComponent(typeof(SpriteRenderer))]
public class YSort : MonoBehaviour 
{
	SpriteRenderer sr;
	int startOrder;
	void Start()
	{
		sr = GetComponent<SpriteRenderer>();
		startOrder = sr.sortingOrder;
	}
	// Update is called once per frame
	void LateUpdate () 
	{
		sr.sortingOrder = Mathf.Abs(Mathf.RoundToInt(transform.position.y)) + startOrder;
	}
}
