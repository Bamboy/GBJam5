using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class StatusEffect
{
	public static Dictionary<string, StatusEffect> effects;
	public static void BuildEffects()
	{
		effects = new Dictionary<string, StatusEffect>();
		StatusEffect newEffect;

		newEffect = new StatusEffect("Poison", 0.25f, 10f); // Power is total damage to deal. 1 damage is dealt every duration.
		newEffect.onApply = (Enemy owner, string effectName) => {
			//TODO attach some kind of particle system
			Debug.Log("buff applied"); 
			return newEffect.StartingValues();
		};
		newEffect.onEvalulate = (Enemy owner, ref EffectValues values) => 
		{
			if( Time.time >= values.endTime )
			{
				//Deal one damage and reduce power by one
				owner.Health -= 1;
				values.power -= 1f;
				Debug.Log("dealt damage- hp: "+ owner.Health);
				if( values.power > 0f )
				{
					values.endTime = Time.time + StatusEffect.effects[ values.source ].duration;
					return true;
				}
				else
					return false;
			}
				
			return true;
		};
		newEffect.onRemove = (Enemy owner) => { 
			//TODO remove some kind of particle system 
			Debug.Log("buff removed"); 
		};
		effects.Add( newEffect.name, newEffect );

	}


	public StatusEffect( string name, float duration, float power )
	{
		this.name = name;
		this.duration = duration;
		this.power = power;
	}
	public string name;
	public float duration;
	public float power;
	public bool stacks = true; //Does this effect stack with others of the same kind?

	public delegate EffectValues OnApplied( Enemy owner, string effectName );
	public delegate bool Evalulate( Enemy owner, ref EffectValues values ); //Returns false if his buff should go away, otherwise true
	public delegate void OnRemoved( Enemy owner );

	public OnApplied onApply = (Enemy owner, string effectName) => { 
		Debug.Log("buff applied"); 
		return StatusEffect.effects[ effectName ].StartingValues();
	};
	public Evalulate onEvalulate = (Enemy owner, ref EffectValues values) => { 
		Debug.Log("this does nothing"); 
		return false; 
	};
	public OnRemoved onRemove = (Enemy owner) => { 
		Debug.Log("buff removed"); 
	};



	public EffectValues StartingValues()
	{
		EffectValues values = new EffectValues();
		values.source = name;
		values.endTime = Time.time + duration;
		values.power = power;
		return values;
	}

	public class EffectValues
	{
		public string source;
		public bool active = true; //Used to remove the effect
		public float endTime; //When to remove the buff, using Time.time
		public float power;  //The power of this effect. Different statuses use this differently.
	}

}
