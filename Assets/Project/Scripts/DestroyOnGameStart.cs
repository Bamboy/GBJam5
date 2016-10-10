using UnityEngine;
using System.Collections;

public class DestroyOnGameStart : MonoBehaviour 
{
	void Update () 
	{
		if( WaveManager.gameStarted == true )
			Destroy(this.gameObject);
	}
}
