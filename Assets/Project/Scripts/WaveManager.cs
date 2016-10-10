using UnityEngine;
using System.Collections;

public class WaveManager : MonoBehaviour 
{
	public static WaveManager instance;
	void Awake(){ instance = this; }
	public float startingDelay = 2f;
	public float delayReduction = 0.1f;
	
	public static bool gameStarted = false;
	public static void StartGame()
	{
		gameStarted = true;
		instance.StartCoroutine("CreateWave", instance.startingDelay);
	}
	public int enemyCount = 8;
	public static int waveNumber = 0;
	IEnumerator CreateWave( float delay )
	{
		waveNumber++;
		StatsUI.instance.waveCounter.text = "Wave: " + waveNumber.ToString();
		delay = Mathf.Max( delay, 0.25f + Random.Range(0f, 0.5f) );

		if( waveNumber == 5 ) //Rare cards appear at 5!
		{
			Card.weights[ Card.cards["Card ++"].id ] = 2;
			Card.weights[ Card.cards["SlowAura"].id ] = 3;
			Card.weights[ Card.cards["Magic"].id ] = 2;
		}
			

		if( waveNumber > 3 )
			enemyCount++;
		if( waveNumber > 10 )
			enemyCount++;
		if( waveNumber > 30 )
			enemyCount++;

		Enemy newEnemy;

		for (int i = 0; i < enemyCount; i++) 
		{
			newEnemy = Enemy.Spawn( RandomEnemy() );
			newEnemy.MaxHealthValue = (int)newEnemy.MaxHealth + (newEnemy.HealthGainPerWave * waveNumber);
			newEnemy.Health = newEnemy.MaxHealthValue;
			yield return new WaitForSeconds( delay );
		}


		yield return new WaitForSeconds( delay );

		while( Enemy.alive.Length > 0 )
			yield return null;

		PlayerStats.instance.Gems += waveNumber;

		StartCoroutine("CreateWave", delay - delayReduction);
	}


	string RandomEnemy()
	{
		return ExtRandom<string>.Choice( new string[]{"Gahblin", "Knight", "Sage", "Troll", "Necro", "Skelly"} );
	}
}
