using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum HealthCap{ Low = 6, Med = 12, High = 30 };

[RequireComponent(typeof(WaypointFollower))]
public class Enemy : MonoBehaviour 
{
	#region Static stuff
	public static Dictionary<string, Enemy> enemies;
	public static Enemy[] alive;

	public static void BuildEnemies()
	{
		float baseSpeed = 10f;

		alive = new Enemy[0];
		//Create a bunch of enemies, place instances into dictionary so we can copy them
		//GameObject emptyEnemy = Resources.Load("Units/Enemy") as GameObject;
		enemies = new Dictionary<string, Enemy>();

		GameObject enemyObj = GameObject.Instantiate( Resources.Load("Units/Gahblin"), WaypointManager.instance.transform ) as GameObject;
		Enemy newEnemy = enemyObj.GetComponent<Enemy>();
		newEnemy.SetType( "Gahblin" );
		newEnemy.MaxHealth = HealthCap.Med;
		newEnemy.Health = int.MaxValue;
		newEnemy.Speed = baseSpeed;
		enemyObj.SetActive( false );
		enemies.Add( newEnemy.Type, newEnemy );

		enemyObj = GameObject.Instantiate( Resources.Load("Units/Knight"), WaypointManager.instance.transform ) as GameObject;
		newEnemy = enemyObj.GetComponent<Enemy>();
		newEnemy.SetType( "Knight" );
		newEnemy.MaxHealth = HealthCap.High;
		newEnemy.Health = int.MaxValue;
		newEnemy.Speed = baseSpeed / 2f;
		enemyObj.SetActive( false );
		enemies.Add( newEnemy.Type, newEnemy );

		enemyObj = GameObject.Instantiate( Resources.Load("Units/Sage"), WaypointManager.instance.transform ) as GameObject;
		newEnemy = enemyObj.GetComponent<Enemy>();
		newEnemy.SetType( "Sage" );
		newEnemy.MaxHealth = HealthCap.Med;
		newEnemy.Health = int.MaxValue;
		newEnemy.Speed = baseSpeed / 2f;
		enemyObj.SetActive( false );
		enemies.Add( newEnemy.Type, newEnemy );

		enemyObj = GameObject.Instantiate( Resources.Load("Units/Troll"), WaypointManager.instance.transform ) as GameObject;
		newEnemy = enemyObj.GetComponent<Enemy>();
		newEnemy.SetType( "Troll" );
		newEnemy.MaxHealth = HealthCap.Med;
		newEnemy.Health = int.MaxValue;
		newEnemy.Speed = baseSpeed;
		enemyObj.SetActive( false );
		enemies.Add( newEnemy.Type, newEnemy );

		enemyObj = GameObject.Instantiate( Resources.Load("Units/Necro"), WaypointManager.instance.transform ) as GameObject;
		newEnemy = enemyObj.GetComponent<Enemy>();
		newEnemy.SetType( "Necro" );
		newEnemy.MaxHealth = HealthCap.High;
		newEnemy.Health = int.MaxValue;
		newEnemy.Speed = baseSpeed / 2f;
		enemyObj.SetActive( false );
		enemies.Add( newEnemy.Type, newEnemy );

		enemyObj = GameObject.Instantiate( Resources.Load("Units/Skelly"), WaypointManager.instance.transform ) as GameObject;
		newEnemy = enemyObj.GetComponent<Enemy>();
		newEnemy.SetType( "Skelly" );
		newEnemy.MaxHealth = HealthCap.Low;
		newEnemy.Health = int.MaxValue;
		newEnemy.Speed = baseSpeed;
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
	SpriteRenderer healthBar;
	public Vector3 centerOffset = new Vector3( 0, 8, 0 );
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
					Health = (int)newType.MaxHealth;

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
	private HealthCap mxHP = HealthCap.Low;
	public HealthCap MaxHealth{
		get{ return mxHP; }
		set{
			switch( value )
			{
			case HealthCap.High:
				healthBarSet = Deck.instance.healthBarHigh;
				break;
			case HealthCap.Med:
				healthBarSet = Deck.instance.healthBarMed;
				break;
			case HealthCap.Low:
				healthBarSet = Deck.instance.healthBarLow;
				break;
			default:
				Debug.LogError("wtf " + value);
				healthBarSet = Deck.instance.healthBarMed;
				break;
			}
			mxHP = value;
		}
	}

	private int hp = 1;
	public int Health
	{
		get{ return hp; }
		set{
			hp = Mathf.Clamp( value, 0, (int)MaxHealth );
			if( hp == 0 )
			{
				mover.speed = 0f;
				PlayerStats.instance.Gems += PlayerStats.instance.gemsPerDeath;
				Destroy( this.gameObject );
			}
			else
			{
				//Update health bar.
				float percent = (float)hp / (float)((int)MaxHealth);
				healthBar.sprite = healthBarSet[ Mathf.FloorToInt( percent * (healthBarSet.Length - 1) ) ];
			}
		}
	}
	private Sprite[] healthBarSet;
	//TODO status resistance

	private WaypointFollower mover;
	void Awake()
	{
		mover = GetComponent<WaypointFollower>();
		statuses = new List<StatusEffect>(0);
		statusValues = new List<StatusEffect.EffectValues>(0);

		Transform t = transform.FindChild("Healthbar");

		if( t == null )
		{
			GameObject obj = new GameObject();
			obj.transform.position = this.transform.position + new Vector3(0, -1, 0);
			obj.transform.SetParent( this.transform );
			obj.name = "Healthbar";
			healthBar = obj.AddComponent<SpriteRenderer>();
			healthBar.sortingLayerName = "Cards";
			healthBar.sortingOrder = -900;
			obj.AddComponent<YSort>();
		}
		else
		{
			healthBar = t.GetComponent<SpriteRenderer>();
		}
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

			if( statuses[i].onEvalulate( this, ref valuesCopy ) == false || valuesCopy.active == false )
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
	public void RemoveStatus( string effect )
	{
		foreach( StatusEffect.EffectValues ev in statusValues )
		{
			if( ev.source == effect )
			{
				ev.active = false; //This will cause the effect to be removed next frame
			}
		}
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

	void OnDrawGizmos()
	{
		Gizmos.color = Color.blue;
		Gizmos.DrawSphere( transform.position + centerOffset, 4f );
	}

	public static Enemy[] GetEnemiesInRange( Vector3 position, float range )
	{
		Enemy[] result = new Enemy[0];

		foreach (Enemy enemy in Enemy.alive) 
		{

			if( Vector3.Distance(position, enemy.transform.position + enemy.centerOffset) < range )
			{
				result = ArrayTools.Push(result, enemy);
			}

		}

		return result;
	}
}
