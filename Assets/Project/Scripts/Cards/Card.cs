using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class Card
{
	public enum CardType{ Structure, Unit, Ability }
	public static Dictionary<string, Card> cards;
	public static Dictionary<string, float> cardDrawWeights;
	public static void BuildCardDatabase()
	{
		//blah blah
	}




	//Card stats
	public string title = "A card!";
	public string description = "Still a card";
	public string longDescription = "";

	public CardType type = CardType.Ability;
	public Sprite cardPicture;







	public delegate bool CanPlay(); //Evalulate if this card can be played (IE player has enough money)
	public CanPlay cardIsPlayable = () => { return false; };
	
	public delegate void OnPlayEvent();
	public delegate bool OnPlaying(); //Return true if we should keep playing on the next frame.
	public OnPlayEvent cardPlayStartAction = () => { Debug.Log("This card has no behaviour assigned"); };
	public OnPlaying cardPlayingAction = () => { return true; };
	public OnPlayEvent cardPlayEndedAction = () => { return; };

}
