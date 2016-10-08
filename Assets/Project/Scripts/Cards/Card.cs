using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
public enum CardType{ Structure, Unit, Ability }
public class Card
{

	public static Dictionary<string, Card> cards;
	public static string[] cardNames;
	public static int[] weights;
	public static int[] GetWeights()
	{
		string debugWeights = "====== Card Weights ======\r\n";
		for (int i = 0; i < cardNames.Length; i++) 
		{
			debugWeights += cardNames[i] +"(id:"+cards[cardNames[i]].id+"): " + weights[i] + "\r\n";
		}

		if( Deck.instance.doDebug )
			Debug.Log(debugWeights);

		return weights;


	}
	//TEXT MESH IMG DISPLAY: <quad material=1 size=20 x=0.1 y=0.1 width=0.5 height=0.5 />
	//Must have multiple materials attached



	//Card stats
	public int id = -1;
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
		this.id = cards.Count;
		this.title = name;
		this.type = type;
		this.defaultWeight = defaultWeight;
	}


	#region Card Defs
	public static void BuildCardDatabase()
	{
		cards = new Dictionary<string, Card>();
		cardNames = new string[0];
		weights = new int[0];
		Card newCard;


		newCard = new Card("Blade", CardType.Structure, 4);
		newCard.cardPicture = null;
		newCard.description = "Chops\r\nEnemies";
		newCard.cost = 3;
		newCard.cardPlayStartAction = (string card) =>
		{
			Deck.instance.DeckState = CardState.Selected;
			Build.instance.StartBuild( Resources.Load("Structures/Bladetower") as GameObject, card, newCard.cardPlayEndedAction );
		};
		CardFinalize( newCard );

		newCard = new Card("Archer", CardType.Structure, 8);
		newCard.cardPicture = null;
		newCard.description = "Shoots\r\npointy\r\nobjects";
		newCard.cost = 1;
		newCard.cardPlayStartAction = (string card) =>
		{
			Deck.instance.DeckState = CardState.Selected;
			Build.instance.StartBuild( Resources.Load("Structures/Archer Tower") as GameObject, card, newCard.cardPlayEndedAction );
		};
		CardFinalize( newCard );

		newCard = new Card("Dart", CardType.Structure, 4);
		newCard.cardPicture = null;
		newCard.description = "Poisons\r\nfoes";
		newCard.cost = 4;
		newCard.cardPlayStartAction = (string card) =>
		{
			Deck.instance.DeckState = CardState.Selected;
			Build.instance.StartBuild( Resources.Load("Structures/DartTower") as GameObject, card, newCard.cardPlayEndedAction );
		};
		CardFinalize( newCard );

		newCard = new Card("SlowAura", CardType.Structure, 3);
		newCard.cardPicture = null;
		newCard.description = "Slows\r\nenemies\r\nin range";
		newCard.cost = 4;
		newCard.cardPlayStartAction = (string card) =>
		{
			Deck.instance.DeckState = CardState.Selected;
			Build.instance.StartBuild( Resources.Load("Structures/SlowAura") as GameObject, card, newCard.cardPlayEndedAction );
		};
		CardFinalize( newCard );

		newCard = new Card("Card ++", CardType.Ability, 1);
		newCard.cardPicture = null;
		newCard.description = "Another\r\ncard to\r\nchoose";
		newCard.cost = 7;
		newCard.cardPlayStartAction = (string card) =>
		{
			Deck.instance.handSize++;
			Debug.Log( cardNames[ Card.cards[card].id ] );
			weights[ Card.cards[card].id ] = 0;
			PlayerStats.Buy( Card.cards[card].cost );
			Deck.instance.DeckState = CardState.Discarding;
		};
		CardFinalize( newCard );

		newCard = new Card("Heal", CardType.Ability, 3);
		newCard.cardPicture = null;
		newCard.description = "Health\r\n+ 3";
		newCard.cost = 4;
		newCard.cardPlayStartAction = (string card) =>
		{
			PlayerStats.instance.Life += 3;
			PlayerStats.Buy( Card.cards[card].cost );
			Deck.instance.DeckState = CardState.Discarding;
		};
		CardFinalize( newCard );







		//ENEMIES - remove me TODO


		newCard = new Card("Gahblin", CardType.Unit, 10);
		newCard.cardPicture = null;
		newCard.description = "Tries to\r\nkill\r\nyou";
		newCard.cost = 1;
		newCard.cardPlayStartAction = (string card) =>
		{
			//GameObject.Instantiate( Resources.Load("Units/Gahblin") as GameObject, WaypointManager.instance.waypoints[0].position, Quaternion.identity );
			Enemy.Spawn("Gahblin");
			PlayerStats.Buy( Card.cards[card].cost );
			Deck.instance.DeckState = CardState.Discarding;
		};
		CardFinalize( newCard );

		newCard = new Card("Knight", CardType.Unit, 10);
		newCard.cardPicture = null;
		newCard.description = "\r\nTough";
		newCard.cost = 1;
		newCard.cardPlayStartAction = (string card) =>
		{
			//GameObject.Instantiate( Resources.Load("Units/Gahblin") as GameObject, WaypointManager.instance.waypoints[0].position, Quaternion.identity );
			Enemy.Spawn("Knight");
			PlayerStats.Buy( Card.cards[card].cost );
			Deck.instance.DeckState = CardState.Discarding;
		};
		CardFinalize( newCard );

		newCard = new Card("Sage", CardType.Unit, 10);
		newCard.cardPicture = null;
		newCard.description = "Heals\r\nfoes";
		newCard.cost = 1;
		newCard.cardPlayStartAction = (string card) =>
		{
			//GameObject.Instantiate( Resources.Load("Units/Gahblin") as GameObject, WaypointManager.instance.waypoints[0].position, Quaternion.identity );
			Enemy.Spawn("Sage");
			PlayerStats.Buy( Card.cards[card].cost );
			Deck.instance.DeckState = CardState.Discarding;
		};
		CardFinalize( newCard );

		newCard = new Card("Troll", CardType.Unit, 10);
		newCard.cardPicture = null;
		newCard.description = "Regens\r\nhis\r\nhealth";
		newCard.cost = 1;
		newCard.cardPlayStartAction = (string card) =>
		{
			//GameObject.Instantiate( Resources.Load("Units/Gahblin") as GameObject, WaypointManager.instance.waypoints[0].position, Quaternion.identity );
			Enemy.Spawn("Troll");
			PlayerStats.Buy( Card.cards[card].cost );
			Deck.instance.DeckState = CardState.Discarding;
		};
		CardFinalize( newCard );

		newCard = new Card("Necro", CardType.Unit, 5);
		newCard.cardPicture = null;
		newCard.description = "Summons\r\nSkellies";
		newCard.cost = 1;
		newCard.cardPlayStartAction = (string card) =>
		{
			//GameObject.Instantiate( Resources.Load("Units/Gahblin") as GameObject, WaypointManager.instance.waypoints[0].position, Quaternion.identity );
			Enemy.Spawn("Necro");
			PlayerStats.Buy( Card.cards[card].cost );
			Deck.instance.DeckState = CardState.Discarding;
		};
		CardFinalize( newCard );
	}

	private static void CardFinalize( Card newCard )
	{
		cards.Add( newCard.title, newCard );
		cardNames = ArrayTools.PushLast( cardNames, newCard.title );
		weights = ArrayTools.PushLast(weights, newCard.defaultWeight);
	}
	#endregion
}
