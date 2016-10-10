using UnityEngine;
using System.Collections;

//Handles drawing cards to the player, as well as detecting mouse inputs on cards.
public enum CardState {
	Drawing, //we're drawing cards
	Choosing, //The player has cards and has the option to choose one
	Selected, //The player has a card selected and is playing it (Not all cards trigger this step)
	Discarding //The player has played a card and the rest are being discarded.
}
public class Deck : MonoBehaviour 
{
	public static Deck instance;
	void Awake(){ instance = this; }
	public bool doDebug = false;
	public Sprite[] cardBacks;
	public Sprite[] cardIcons;
	public int[] cardHeights;

	public Sprite[] healthBarLow;
	public Sprite[] healthBarMed;
	public Sprite[] healthBarHigh;

	private CardState state = CardState.Discarding;
	public static bool stateChanged = false;
	public CardState DeckState
	{
		get{ return state; }
		set{
			if( value != state )
			{
				stateChanged = true;
				switch( state ) //Our old value.
				{
				case CardState.Drawing:
					break;
				case CardState.Choosing:
					break;
				case CardState.Selected:
					break;
				case CardState.Discarding:
					foreach (CardDisplay cd in playerHand)//Destroy old card gameobjects.
						Destroy( cd.gameObject );

					playerHand = new CardDisplay[0];
					break;
				default:
					Debug.LogError("Wtf how'd you get here?");
					break;
				}

				switch( value ) //Our new value.
				{
				case CardState.Drawing:
					cardHandPoint.localPosition = new Vector3( 80, cardHeights[0], 0 );
					//Decide new cards to pick and instantiate them. 
					//We don't need to disable input- cards spawn disabled
					StopCoroutine("CardCreation");
					StartCoroutine("CardCreation");
					CardInput = false;
					break;
				case CardState.Choosing:
					cardHandPoint.localPosition = new Vector3( 80, cardHeights[1], 0 );
					CardInput = true; //Enable the player input with the cards.
					break;
				case CardState.Selected:
					//Disable player input with cards- its assumed something else will be taking inputs
					CardInput = false;
					cardHandPoint.localPosition = new Vector3( 80, cardHeights[2], 0 );
					break;
				case CardState.Discarding:
					CardInput = false;//Disable player input with cards
					StopCoroutine("CardDeletion");
					StartCoroutine("CardDeletion");//Have cards start to move offscreen
					break;
				default:
					Debug.LogError("Wtf how'd you get here?");
					break;
				}

				state = value;
			}
		}
	}

	private bool cardInput = false;
	public bool CardInput
	{
		get{ return cardInput; }
		set{
			if( value != cardInput )
			{
				foreach (CardDisplay card in playerHand) 
					card.GetComponent<BoxCollider>().enabled = value;
				cardInput = value;
			}
		}
	}
	public Transform cardCreatePoint;
	public Transform cardHandPoint;
	public Transform cardDestroyPoint;

	public GameObject blankCard;
	public int handSize = 2; //How many cards we will draw
	public CardDisplay[] playerHand;



	void Start()
	{
		Card.BuildCardDatabase();
		Enemy.BuildEnemies();
		StatusEffect.BuildEffects();
		DeckState = CardState.Drawing;
	}

	void Drawing()
	{

	}
	void Choosing() 
	{
		RaycastHit data = CameraUpscale.instance.DownscaledMouseRaycast();
		if( data.collider != null )
		{
			if( data.collider.tag == "Card" )
			{
				CardDisplay cd = data.collider.GetComponent<CardDisplay>();
				if( cd != null )
				{
					if( cd == CardDisplay.currentHovered )
						cd.OnMouseStay();
					else
					{
						if( CardDisplay.currentHovered != null )
							CardDisplay.currentHovered.OnMouseExit();
						
						cd.OnMouseEnter();
						CardDisplay.currentHovered = cd;
					}
				}
				else
				{
					if( CardDisplay.currentHovered != null )
					{
						CardDisplay.currentHovered.OnMouseExit();
						CardDisplay.currentHovered = null;
					}
				}
			}
			else
			{
				if( CardDisplay.currentHovered != null )
				{
					CardDisplay.currentHovered.OnMouseExit();
					CardDisplay.currentHovered = null;
				}
			}
		}
		else
		{
			if( CardDisplay.currentHovered != null )
			{
				CardDisplay.currentHovered.OnMouseExit();
				CardDisplay.currentHovered = null;
			}
		}

		if( CardDisplay.currentHovered != null )
		{
			//Some kind of indicator to show if we can afford to use this card.
			string thiscard = CardDisplay.currentHovered.card;
			Card cardData = CardDisplay.currentHovered.cardInstance;

			if( Input.GetMouseButtonUp(0) )
			{
				if( cardData.cardIsPlayable( thiscard ) )
				{
					cardData.cardPlayStartAction( thiscard );

				}
				else
				{
					//Debug.Log("Nope!");
					StatsUI.instance.FlashGems();
				}
			}
		}



	}
	void Selected()
	{

	}
	void Discarding()
	{

	}

	void Update()
	{
		switch( DeckState )
		{
		case CardState.Drawing:
			Drawing();
			break;
		case CardState.Choosing:
			Choosing();
			break;
		case CardState.Selected:
			Selected();
			break;
		case CardState.Discarding:
			Discarding();
			break;
		default:
			Debug.LogError("Wtf how'd you get here?");
			break;
		}

		if( Input.GetKeyDown(KeyCode.O) && Deck.instance.doDebug )
			PlayerStats.instance.Life -= 999;
	}
	void LateUpdate()
	{
		stateChanged = false;
	}


	public bool AllCardsAtTarget()
	{
		bool result = true;
		foreach(CardDisplay cd in playerHand)
		{
			if( cd.IsAtTarget() == false )
			{
				result = false;
				break;
			}
		}
		return result;
	}
	IEnumerator CardDeletion()
	{
		while( AllCardsAtTarget() == false )
			yield return null;
		yield return null;
		//yield return new WaitForSeconds(0.2f);
		cardHandPoint.localPosition = new Vector3( 80, cardHeights[2], 0 );

		while( AllCardsAtTarget() == false )
			yield return null;
		yield return null;
		foreach(CardDisplay cd in playerHand)
			cd.moveTarget = cardDestroyPoint;
		
		while( AllCardsAtTarget() == false )
			yield return null;
		yield return new WaitForSeconds(0.6f);


		DeckState = CardState.Drawing; //This will delete all objects and clear our array
	}
	int[] cardWeights;
	IEnumerator CardCreation()
	{
		cardWeights = Card.GetWeights();
		//Debug.Log("Card weights: " + cardWeights[0] + "," + cardWeights[1] + "," + cardWeights[2]);

		while( playerHand.Length < handSize )
		{
			string card = ExtRandom<string>.WeightedChoice( Card.cardNames, cardWeights );

			bool hasThisCardAlready = false;
			foreach( CardDisplay cd in playerHand )
			{
				if( cd.card == card )
					hasThisCardAlready = true;
			}
			
			if( hasThisCardAlready )
				continue;

			playerHand = ArrayTools.Push( playerHand, CreateCard( card, playerHand.Length ) );
			yield return new WaitForSeconds( 0.25f );
		}

		while( AllCardsAtTarget() == false )
			yield return null;
		yield return new WaitForSeconds( 0.25f ); //Wait a bit extra longer for card animations to finish.

		DeckState = CardState.Choosing;
	}

	private CardDisplay CreateCard( string card, int handIndex )
	{
		GameObject go = GameObject.Instantiate( blankCard, cardCreatePoint.position, cardCreatePoint.rotation, cardCreatePoint ) as GameObject;
		go.GetComponent<BoxCollider>().enabled = false;
		CardDisplay cd = go.GetComponent<CardDisplay>();
		cd.card = card;
		cd.handIndex = handIndex;
		cd.moveTarget = cardHandPoint;
		return cd;
	}

	void OnGUI()
	{
		if( doDebug == false ) 
			return;


		GUI.color = Color.white;
		GUILayout.BeginHorizontal();
		GUILayout.Label("DeckState: " + DeckState.ToString());

		GUILayout.EndHorizontal();
		GUILayout.Label("Wave: " + WaveManager.waveNumber);
	}
}
