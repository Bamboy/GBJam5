using UnityEngine;
using System.Collections;

public class FontMaterialFilter : MonoBehaviour 
{

	// Use this for initialization
	void Start () 
	{
		TextMesh tm = GetComponent<TextMesh>();
		tm.font.material.mainTexture.filterMode = FilterMode.Point;
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}
}
