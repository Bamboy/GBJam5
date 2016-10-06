using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(WaypointFollower))]
public class Enemy : MonoBehaviour 
{
	#region Static stuff
	public static Dictionary<string, Enemy> enemies;
	public static Enemy[] alive;

	public static void BuildEnemies()
	{
		alive = new Enemy[0];
		//Create a bunch of enemies, place instances into dictionary so we can copy them
		GameObject emptyEnemy = Resources.Load("Units/Enemy") as GameObject;
		enemies = new Dictionary<string, Enemy>();

		GameObject enemyObj = GameObject.Instantiate( emptyEnemy, WaypointManager.instance.transform ) as GameObject;
		Enemy newEnemy = enemyObj.GetComponent<Enemy>();
		newEnemy.SetType( "Gahblin" );
		newEnemy.MaxHealth = 10;
		newEnemy.Health = int.MaxValue;
		newEnemy.Speed = 10f;
		enemyObj.SetActive( false );
		enemies.Add( newEnemy.Type, newEnemy );

	}
	//Spawn an enemy at the start of the track.
	public static Enemy Spawn( string type )
	{
		if( enemies.ContainsKey( type ) )
		{
			GameObject obj = GameObject.Instantiate( enemies[ type ].gameObject, WaypointManager.instance.waypoints[0].position, Quaternion.identity, WaypointManager.instance.transform ) as GameObject;
			obj.SetActive( true );
			Enemy e = obj.GetComponent<Enemy>();
			e.Type = type;

			alive = ArrayTools.PushLast( alive, e );
			//TODO add some kind of array with all active enemies
			return e;
		}
		else
		{
			Debug.LogError("Enemy definition does not exist: "+type);
			return null;
		}
	}


	#endregion

	public void SetType( string name ) { type = name; gameObject.name = name; } //DONT USE - ONLY FOR MAKING ENEMIES
	private string type = "";
	public string Type
	{
		get{ return type; }
		set{
			if( value != type )
			{
				if( Enemy.enemies.ContainsKey( value ) )
				{
					//Copy our enemy stats
					Enemy newType = Enemy.enemies[ value ];
					type = value;
					Speed = newType.Speed;
					MaxHealth = newType.MaxHealth;
					Health = newType.MaxHealth;

					transform.position = WaypointManager.instance.waypoints[0].position;
				}
				else
					Debug.LogError("No card exists with name "+value);
			}
		}
	}

	public float Speed
	{
		get{ return mover.speed; }
		set{
			mover.speed = Mathf.Clamp( value, 2.5f, 100f );
		}
	}
	public int MaxHealth = 1;
	private int hp = 1;
	public int Health
	{
		get{ return hp; }
		set{
			hp = Mathf.Clamp( value, 0, MaxHealth );
			if( hp == 0 )
			{
				mover.speed = 0f;
				PlayerStats.instance.Gems += PlayerStats.instance.gemsPerDeath;
				Destroy( this.gameObject );
			}
		}
	}
	//TODO status resistance



	private WaypointFollower mover;
	void Awake()
	{
		mover = GetComponent<WaypointFollower>();
		statuses = new List<StatusEffect>(0);
		statusValues = new List<StatusEffect.EffectValues>(0);
	}

	void Update()
	{
		if( statuses.Count > 0 )
		{
			DoStatusEffects();
		}

		if( Input.GetKeyDown(KeyCode.L) )
			AddStatus( "Poison" );
	}
	void OnDestroy()
	{
		alive = ArrayTools.Remove(alive, this);
	}

	#region Status effects
	public List<StatusEffect> statuses;
	public List<StatusEffect.EffectValues> statusValues;
	void DoStatusEffects()
	{
		bool removing = false;
		for (int i = 0; i < statuses.Count; i++) 
		{
			StatusEffect.EffectValues valuesCopy = statusValues[i];

			if( statuses[i].onEvalulate( this, ref valuesCopy ) == false )
			{
				statuses[i].onRemove( this );
				valuesCopy.active = false;
				removing = true;
			}
			statusValues[i] = valuesCopy;
		}

		if( removing )
		{
			List<StatusEffect> statusCopy = new List<StatusEffect>( statuses );
			List<StatusEffect.EffectValues> valuesCopy = new List<StatusEffect.EffectValues>( statusValues );

			for (int j = 0; j < statusCopy.Count; j++) 
			{
				if( valuesCopy[j].active == false )
				{
					RemoveStatus( j );
				}
			}
		}
	}
	public void AddStatus( string effect )
	{
		if( StatusEffect.effects.ContainsKey( effect ) )
		{
			//Check if we have this effect
			if( HasStatus( effect ) )
			{
				if( StatusEffect.effects[effect].stacks == true )
				{
					PushStatus( effect );
				}
				else
				{
					//Refresh the existing effect rather than adding a new one. TODO add to duration instead?
					statusValues[ GetStatusIndex(effect) ] = StatusEffect.effects[effect].StartingValues();
				}
			}
			else
				PushStatus( effect );
		}
		else
			Debug.LogError("No status effect exists with name: " + effect);
	}
	void PushStatus( string effect ) //Forcefully adds the effect
	{
		statuses.Add( StatusEffect.effects[effect] );
		statusValues.Add( StatusEffect.effects[effect].onApply( this, effect ) );
	}
	void RemoveStatus( int index )
	{
		statuses.RemoveAt( index );
		statusValues.RemoveAt( index );
	}
	public bool HasStatus( string effect ) { return GetStatusIndex(effect) != -1; }
	public int GetStatusIndex( string effect )
	{
		for (int i = 0; i < statuses.Count; i++) 
		{
			if( statuses[i].name == effect )
			{
				return i;
			}
		}
		return -1;
	}
	#endregion
}
