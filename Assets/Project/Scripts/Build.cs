using UnityEngine;
using System.Collections;

public class Build : MonoBehaviour 
{
	public static Build instance;
	void Awake(){ instance = this; }

	public Sprite canBuild;
	public Sprite cannotBuild;

	public GameObject structure;

	private bool building = false;
	string structureCard = "";

	public Transform highlighter;
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
		finishedBuildingAction = onBuildingFinished;
	}

	public void CancelBuild()
	{
		if( !building )
			return;
		Debug.Break();
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
			if( PlayerStats.CanAfford( Card.cards[structureCard].cost ) == false || Input.GetMouseButtonDown(1) )
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


				if( data.collider.tag == "Buildable" )
				{
					highlighterSpr.sprite = canBuild;

					if( Input.GetMouseButtonDown(0) )
					{
						ConfirmBuild( snappedPos );
					}
				}
				else
					highlighterSpr.sprite = cannotBuild;
			}
			else
				highlighterSpr.sprite = cannotBuild;
		}



	}
}
