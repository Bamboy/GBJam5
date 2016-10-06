using UnityEngine;
using System.Collections;

public class StatsUI : MonoBehaviour 
{
	public static StatsUI instance;
	void Awake() { instance = this; }

	public float flashDelay = 0.1f;
	public Sprite[] hearts;
	public SpriteRenderer heartRenderer;
	public TextMesh heartText;
	public Sprite[] gems;
	public SpriteRenderer gemRenderer;
	public TextMesh gemText;

	public void SetGems( int val )
	{
		gemText.text = val.ToString();
	}
	public void FlashGems()
	{
		StopCoroutine( "GemFlash" );
		StartCoroutine( "GemFlash" );
	}

	IEnumerator GemFlash()
	{
		//We start at 0
		gemRenderer.sprite = gems[1];
		yield return new WaitForSeconds(flashDelay);
		gemRenderer.sprite = gems[0];
		yield return new WaitForSeconds(flashDelay);
		gemRenderer.sprite = gems[1];
		yield return new WaitForSeconds(flashDelay);
		gemRenderer.sprite = gems[0];
		yield return new WaitForSeconds(flashDelay);
		gemRenderer.sprite = gems[1];
		yield return new WaitForSeconds(flashDelay);
		gemRenderer.sprite = gems[0];
	}

	public void SetLife( int val, bool doFlash )
	{
		heartText.text = val.ToString();
		if( doFlash )
		{
			StopCoroutine( "LifeFlash" );
			StartCoroutine( "LifeFlash" );
		}
	}

	IEnumerator LifeFlash()
	{
		//We start at 0
		heartRenderer.sprite = hearts[1];
		yield return new WaitForSeconds(flashDelay);
		heartRenderer.sprite = hearts[2];
		yield return new WaitForSeconds(flashDelay);
		heartRenderer.sprite = hearts[0];
		yield return new WaitForSeconds(flashDelay);
		heartRenderer.sprite = hearts[1];
		yield return new WaitForSeconds(flashDelay);
		heartRenderer.sprite = hearts[2];
		yield return new WaitForSeconds(flashDelay);
		heartRenderer.sprite = hearts[0];
		yield return new WaitForSeconds(flashDelay);
		heartRenderer.sprite = hearts[1];
		yield return new WaitForSeconds(flashDelay);
		heartRenderer.sprite = hearts[2];
		yield return new WaitForSeconds(flashDelay);
		heartRenderer.sprite = hearts[0];
		yield return new WaitForSeconds(flashDelay);
		heartRenderer.sprite = hearts[1];
		yield return new WaitForSeconds(flashDelay);
		heartRenderer.sprite = hearts[2];
		yield return new WaitForSeconds(flashDelay);
		heartRenderer.sprite = hearts[0];
	}

	
}
