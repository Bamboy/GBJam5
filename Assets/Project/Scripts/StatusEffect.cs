﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class StatusEffect
{
	public static Dictionary<string, StatusEffect> effects;
	public static void BuildEffects()
	{
		effects = new Dictionary<string, StatusEffect>();
		StatusEffect newEffect;



		newEffect = new StatusEffect("Slow", Mathf.Infinity, 8f);
		newEffect.onApply = (Enemy owner, string effectName) => {
			//TODO attach some kind of particle system
			owner.Speed -= newEffect.power;
			return newEffect.StartingValues();
		};
		newEffect.onEvalulate = (Enemy owner, ref EffectValues values) => { return true; };
		newEffect.onRemove = (Enemy owner) => { 
			//TODO remove some kind of particle system 
			owner.Speed += newEffect.power;
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
