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
	public TextMesh waveCounter;
	public Sprite heartHalf1;
	public Sprite heartHalf2;
	public Transform gameMenu;

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

	public void GameOverAnim()
	{
		StartCoroutine("LifeFlash");
		StartCoroutine("HeartScale");
	}
	IEnumerator HeartScale()
	{
		while( heartRenderer.transform.localScale.magnitude < 12.5f )
		{
			CameraController.instance.Shake( 10f, heartRenderer.transform.localScale.magnitude * 0.75f );
			heartRenderer.transform.localScale *= 1.025f;
			yield return null;
		}

		CameraController.instance.Shake( 0f, 0f );

		//CameraController.instance.Shake( 10f, heartRenderer.transform.localScale.magnitude );

		yield return new WaitForSeconds(1f);

		CameraController.instance.Shake( 0f, 0f );

		yield return new WaitForSeconds(0.5f);

		GameObject obj1 = GameObject.Instantiate( heartRenderer.gameObject, heartRenderer.transform.position, heartRenderer.transform.rotation ) as GameObject;
		obj1.GetComponent<SpriteRenderer>().sprite = heartHalf1;
		PolygonCollider2D p1 = obj1.AddComponent<PolygonCollider2D>();

		GameObject obj2 = GameObject.Instantiate( heartRenderer.gameObject, heartRenderer.transform.position, heartRenderer.transform.rotation ) as GameObject;
		obj2.GetComponent<SpriteRenderer>().sprite = heartHalf2;
		PolygonCollider2D p2 = obj2.AddComponent<PolygonCollider2D>();

		Physics2D.IgnoreCollision( p1, p2 );

		Rigidbody2D rb1 = obj1.AddComponent<Rigidbody2D>();
		rb1.AddForce( Vector2.left * 59f * Random.value );
		rb1.AddTorque( 20000f );

		Rigidbody2D rb2 = obj2.AddComponent<Rigidbody2D>();
		rb2.AddForce( Vector2.right * 59f * Random.value );
		rb2.AddTorque( -20000f );


		heartRenderer.gameObject.SetActive( false );

		yield return new WaitForSeconds(1f);

		//Show menu

		while( true )
		{
			gameMenu.localPosition = Vector3.MoveTowards( gameMenu.localPosition, new Vector3(80,-70,-47), 3f );
			yield return null;
		}
	}
}
