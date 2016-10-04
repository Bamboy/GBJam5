using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
public enum CardType{ Structure, Unit, Ability }
public class Card
{

	public static Dictionary<string, Card> cards;
	public static Dictionary<string, int> cardDrawWeights;


	//TEXT MESH IMG DISPLAY: <quad material=1 size=20 x=0.1 y=0.1 width=0.5 height=0.5 />
	//Must have multiple materials attached



	//Card stats
	public string title = "A card!";
	public string description = "Still a card";
	public string longDescription = "";

	public CardType type = CardType.Ability;
	public Sprite cardPicture;

	public int defaultWeight = 0;
	public int cost = 1;


	public delegate bool CanPlay( string card ); //Evalulate if this card can be played (IE player has enough money)
	public delegate void OnPlayEvent( string card );
	public delegate bool OnPlaying( string card ); //Return true if we should keep playing on the next frame.

	public CanPlay cardIsPlayable = (string card) => { return PlayerStats.CanAfford( Card.cards[card].cost ); };
	public OnPlayEvent cardPlayStartAction = (string card) => { Debug.Log("This card has no behaviour assigned"); };
	public OnPlaying cardPlayingAction = (string card) => { return true; };
	public OnPlayEvent cardPlayEndedAction = (string card) => { PlayerStats.Buy( Card.cards[card].cost ); Deck.instance.DeckState = CardState.Discarding; };

	public Card(string name, CardType type, int defaultWeight = 0)
	{
		this.title = name;
		this.type = type;
		this.defaultWeight = defaultWeight;
	}


	#region Card Defs
	public static void BuildCardDatabase()
	{
		cards = new Dictionary<string, Card>();
		cardDrawWeights = new Dictionary<string, int>();
		Card newCard;


		newCard = new Card("Blade", CardType.Structure, 3);
		newCard.cardPicture = null;
		newCard.description = "Chops\r\nEnemies";
		newCard.cost = 1;
		newCard.cardPlayStartAction = (string card) =>
		{
			Deck.instance.DeckState = CardState.Selected;
			Build.instance.StartBuild( Resources.Load("Structures/Bladetower") as GameObject, card, newCard.cardPlayEndedAction );
		};
		cards.Add( newCard.title, newCard );
		cardDrawWeights.Add( newCard.title, newCard.defaultWeight );

		newCard = new Card("Gahblin", CardType.Unit, 3);
		newCard.cardPicture = null;
		newCard.description = "Tries to\r\nkill\r\nyou";
		newCard.cost = 1;
		newCard.cardPlayStartAction = (string card) =>
		{
			GameObject.Instantiate( Resources.Load("Units/Gahblin") as GameObject, WaypointManager.instance.waypoints[0].position, Quaternion.identity );
			PlayerStats.Buy( Card.cards[card].cost );
			Deck.instance.DeckState = CardState.Discarding;
		};
		cards.Add( newCard.title, newCard );
		cardDrawWeights.Add( newCard.title, newCard.defaultWeight );

		newCard = new Card("Card++", CardType.Ability, 1);
		newCard.cardPicture = null;
		newCard.description = "Another\r\ncard to\r\nchoose";
		newCard.cost = 1;
		newCard.cardPlayStartAction = (string card) =>
		{
			Deck.instance.handSize++;
			PlayerStats.Buy( Card.cards[card].cost );
			Deck.instance.DeckState = CardState.Discarding;
		};
		cards.Add( newCard.title, newCard );
		cardDrawWeights.Add( newCard.title, newCard.defaultWeight );
	}
	#endregion
}
