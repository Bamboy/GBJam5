using UnityEngine;
using System.Collections;

public class PlayerStats : MonoBehaviour 
{
	public static PlayerStats instance;
	void Awake() { instance = this; }


	public int gemsPerDeath = 1;


	private int money = 10;
	public int Gems
	{
		get{
			return money;
		}
		set{
			money = value;
		}
	}

	public static bool CanAfford( int cost )
	{ return instance.Gems >= cost; }
	public static bool Buy( int cost )
	{
		if( CanAfford( cost ) )
		{
			instance.Gems -= cost;
			return true;
		}
		else
			return false;
	}

	private int lives = 1;
	public int Life
	{
		get{
			return lives;
		}
		set{
			//Update life counter TODO
			if( value < lives )
			{
				//Camera shake coroutine. TODO
			}

			lives = Mathf.Clamp( value, 0, 999 );
			if( lives == 0 )
			{
				Debug.LogError("Game over! (Do this)");
			}
		}
	}

}
