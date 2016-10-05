using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class FontMaterialFilter : MonoBehaviour 
{

	// Use this for initialization
	void Start () 
	{
		tm = GetComponent<TextMesh>();
		tmfont = tm.font;
		tm.font.material.mainTexture.filterMode = FilterMode.Point;
		tm.GetComponent<Renderer>().sortingOrder = sortingOrder;
		tm.GetComponent<Renderer>().sortingLayerName = sortingLayer;
	}
	public string sortingLayer;
	public int sortingOrder = 0;
	// Update is called once per frame
	private TextMesh tm;
	private Font tmfont;
	void Update () 
	{
		#if UNITY_EDITOR
		if( tmfont != tm.font )
		{
			tm.font.material.mainTexture.filterMode = FilterMode.Point;
		}
		tm.GetComponent<Renderer>().sortingOrder = sortingOrder;
		tm.GetComponent<Renderer>().sortingLayerName = sortingLayer;
		#endif
	}
}
