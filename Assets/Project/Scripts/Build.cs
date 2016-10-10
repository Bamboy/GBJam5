using UnityEngine;
using System.Collections;

public class Build : MonoBehaviour 
{
	public static Build instance;
	void Awake(){ instance = this; }

	public Sprite canBuild;
	public Sprite cannotBuild;

	public Structure structComponent;
	public GameObject structure;
	public Transform raycatcher;

	private bool building = false;
	string structureCard = "";

	public Transform highlighter;
	public LineRenderer rangeIndicator;
	SpriteRenderer highlighterSpr;

	private Card.OnPlayEvent finishedBuildingAction;

	void Start()
	{
		highlighterSpr = highlighter.GetComponent<SpriteRenderer>();
		building = false;
		highlighter.gameObject.SetActive( false );
	}

	public void StartBuild( GameObject structure, string sourceCard, Card.OnPlayEvent onBuildingFinished = null ) //Cost is only used to cancel building if the player lacks funds.
	{
		building = true;
		highlighter.gameObject.SetActive(true);
		structureCard = sourceCard;

		this.structure = structure;
		structComponent = structure.GetComponent<Structure>();
		finishedBuildingAction = onBuildingFinished;
	}

	public void CancelBuild()
	{
		if( !building )
			return;

		building = false;
		highlighter.gameObject.SetActive(false);
		structureCard = "";
		finishedBuildingAction = null;
		Deck.instance.DeckState = CardState.Choosing;
	}

	private void ConfirmBuild( Vector3 pos )
	{
		if( !building )
			return;
		
		//Place structure, disable highlighter
		GameObject.Instantiate( structure, pos, Quaternion.identity );
		building = false;
		highlighter.gameObject.SetActive( false );


		if( finishedBuildingAction != null )
			finishedBuildingAction( structureCard );
		
		finishedBuildingAction = null;
		structureCard = "";
	}

	void Update () 
	{
		if( building )
		{
			if( PlayerStats.CanAfford( Card.cards[structureCard].startCost ) == false || Input.GetMouseButtonDown(1) )
			{
				CancelBuild();
				return;
			}

			RaycastHit data = CameraUpscale.instance.DownscaledMouseRaycast();
			if( data.point != Vector3.zero )
			{
				//Move indicator

				Vector3 snappedPos = new Vector3( VectorExtras.RoundTo(data.point.x - 8f, 16f), VectorExtras.RoundTo(data.point.y + 8f, 16f), 0f );

				highlighter.transform.position = snappedPos;

				rangeIndicator.enabled = true;
				DrawRange( snappedPos + new Vector3( 8, -8, 0 ), structComponent.areaOfInfluence );

				if( data.collider.tag == "Buildable" )
				{
					highlighterSpr.sprite = canBuild;

					if( Input.GetMouseButtonDown(0) )
					{
						ConfirmBuild( snappedPos );


						if( WaveManager.gameStarted == false ) //Dont start the game until the player builds something!
							WaveManager.StartGame();
					}
				}
				else
					highlighterSpr.sprite = cannotBuild;
			}
			else
				highlighterSpr.sprite = cannotBuild;
		}
		else
		{
			RaycastHit data = CameraUpscale.instance.DownscaledMouseRaycast();
			if( data.point != Vector3.zero )
			{
				//Move indicator
				if( data.collider.tag == "Structure" )
				{
					Vector3 snappedPos = new Vector3( VectorExtras.RoundTo(data.point.x - 8f, 16f), VectorExtras.RoundTo(data.point.y + 8f, 16f), 0f );

					rangeIndicator.enabled = true;
					DrawRange( snappedPos + new Vector3( 8, -8, 0 ), data.collider.GetComponent<Structure>().areaOfInfluence );
				}
				else
					rangeIndicator.enabled = false;
			}
			else
				rangeIndicator.enabled = false;


		}



	}

	public float theta_scale = 0.1f;
	void DrawRange( Vector3 center, float radius )
	{
		Vector3[] points = new Vector3[0];

		int i = 0;
		for(float theta = 0f; theta < (2f * Mathf.PI); theta += theta_scale)
		{
			// Calculate position of point
			float x = radius * Mathf.Cos(theta);
			float y = radius * Mathf.Sin(theta);

			// Set the position of this point
			Vector3 pos = new Vector3(x, y, 1) + center;

			points = ArrayTools.Push( points, pos );

			i++;
		}
		points = ArrayTools.Push( points, points[ points.Length - 1] );
		rangeIndicator.SetVertexCount( points.Length );
		rangeIndicator.SetPositions( points );

	}
}
