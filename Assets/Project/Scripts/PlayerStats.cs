﻿using UnityEngine;
using System.Collections;

public class PlayerStats : MonoBehaviour 
{
	public static PlayerStats instance;
	void Awake() { instance = this; }
	void Start()
	{
		Gems = 10;
		Life = 5;
	}

	public int gemsPerDeath = 1;


	private int money = 8;
	public int Gems
	{
		get{
			return money;
		}
		set{
			money = value;
			StatsUI.instance.SetGems( value );
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
				StatsUI.instance.SetLife( value, true );
				CameraController.instance.Shake( 0.5f, 4f );
			}
			else
				StatsUI.instance.SetLife( value, false );

			lives = Mathf.Clamp( value, 0, 999 );
			if( lives == 0 )
			{
				GameOver();
			}
		}
	}


	public static void GameOver()
	{
		StatsUI.instance.GameOverAnim();

		Build.instance.raycatcher.transform.position = new Vector3(0,0,-130);
		Build.instance.raycatcher.tag = "Untagged";
	}
}
