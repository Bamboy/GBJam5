using UnityEngine;
using System.Collections;

//Handles display of card and user interaction
[RequireComponent(typeof(BoxCollider))]
public class CardDisplay : MonoBehaviour 
{
	public static CardDisplay currentHovered; //Current card that the mouse is over, if any.

	public SpriteRenderer cardIconRenderer;
	public SpriteRenderer cardPictureRenderer;
	public TextMesh cardName;
	public TextMesh cardDescription;

	public TextMesh costText;

	private string currentCard = "";
	public string card
	{
		get{ return currentCard; }
		set{
			if( value != currentCard )
			{
				if( Card.cards.ContainsKey( value ) )
				{
					//Set our card to this
					Card newCard = Card.cards[ value ];
					currentCard = newCard.title;
					cardName.text = newCard.title;
					cardPictureRenderer.sprite = newCard.cardPicture;
					cardIconRenderer.sprite = Deck.instance.cardIcons[ (int)newCard.type ];
					cardDescription.text = newCard.description;
					costText.text = Mathf.Clamp(newCard.cost, 0f, 199f).ToString();
				}
				else
					Debug.LogError("No card exists with name "+value);
			}
		}
	}
	public Card cardInstance{ get{ return Card.cards[ card ]; } }

	public void OnMouseEnter()
	{
		Debug.Log("Mouse Enter");
	}
	public void OnMouseStay()
	{
		//transform.position = Vector3.MoveTowards( transform.position, 
	}
	public void OnMouseExit()
	{
		Debug.Log("Mouse Exit");
	}

	public Transform moveTarget;

	public int handIndex;

	public float yHoverOffset = 32f;
	public float yHoverOffsetSelected = 20f;

	public float cardSpeed = 10f;

	private Vector3 targetOffset;
	private float costTextTargetHeight;
	void LateUpdate()
	{
		if( moveTarget != null )
		{
			//Vector3 targetOffset = Vector3.zero;
			if( Deck.instance.DeckState == CardState.Selected )
			{
				targetOffset.y = currentHovered == this ? yHoverOffsetSelected : 0f;
				costTextTargetHeight = 30f;
			}
			else if( Deck.instance.DeckState == CardState.Discarding )
			{
				targetOffset.y = 0f;
				costTextTargetHeight = 30f;
			}
			else
			{
				targetOffset.y = currentHovered == this ? yHoverOffset : 0f;
				costTextTargetHeight = currentHovered == this ? 43f : 30f;
			}

			
			if( moveTarget.transform.name == "Player Hand" )
			{
				if( Deck.instance.handSize == 2 )
				{
					float[] positions = new float[]{ -26f, 26f };
					targetOffset.x = positions[ handIndex ];
				}
				else if( Deck.instance.handSize == 3 )
				{
					float[] positions = new float[]{ -52f, 0f, 52 };
					targetOffset.x = positions[ handIndex ];
				}
				else
					Deck.instance.handSize = 3;
			}

			transform.position = Vector3.MoveTowards( transform.position, moveTarget.position + targetOffset, cardSpeed );

			costText.transform.parent.localPosition = Vector3.MoveTowards( costText.transform.parent.localPosition, new Vector3( -12f, costTextTargetHeight, 5f ), cardSpeed / 2.5f );
		}

		//Palette swap based on if we can afford to play the card.
		if( Card.cards.ContainsKey( card ) )
		{
			if( PlayerStats.CanAfford( Card.cards[ card ].cost ) )
			{
				cardBack.sprite = Deck.instance.cardBacks[0];
				costBack.sprite = Deck.instance.cardBacks[1];
			}
			else
			{
				cardBack.sprite = Deck.instance.cardBacks[2];
				costBack.sprite = Deck.instance.cardBacks[3];
			}
		}
	}
	private SpriteRenderer cardBack;
	private SpriteRenderer costBack;
	void Start()
	{
		cardBack = GetComponent<SpriteRenderer>();
		costBack = costText.transform.parent.GetComponent<SpriteRenderer>();
	}

	public bool IsAtTarget()
	{
		return Mathf.Approximately(Vector3.Distance(transform.position, moveTarget.position + targetOffset), 0f);
	}

	void OnDrawGizmos()
	{
		if( Application.isPlaying && moveTarget != null )
		{
			Gizmos.color = Color.cyan;
			Gizmos.DrawSphere( moveTarget.position + targetOffset, 4f );
		}
	}


}
