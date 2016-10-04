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
	void Update()
	{
		if( moveTarget != null )
		{
			//Vector3 targetOffset = Vector3.zero;
			if( Deck.instance.DeckState == CardState.Selected )
				targetOffset.y = currentHovered == this ? yHoverOffsetSelected : 0f;
			else if( Deck.instance.DeckState == CardState.Discarding )
				targetOffset.y = 0f;
			else
				targetOffset.y = currentHovered == this ? yHoverOffset : 0f;

			
			if( moveTarget.transform.name == "Player Hand" )
			{
				targetOffset.x = (handIndex * 48f) + 24f;
			}

			transform.position = Vector3.MoveTowards( transform.position, moveTarget.position + targetOffset, cardSpeed );
		}
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
