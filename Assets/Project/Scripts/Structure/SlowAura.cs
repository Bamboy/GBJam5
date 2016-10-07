using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SlowAura : Structure
{
	public static SlowAura[] all;
	public override void Start () 
	{
		if( all == null )
			all = new SlowAura[0];

		all = ArrayTools.Push( all, this );

		base.Start();
		onTick = (Structure owner) => 
		{

			return;

		};
	}

	public static void Evalulate()
	{
		if( all == null )
			return;
		
		Enemy[] afflicted = new Enemy[0];
		foreach( SlowAura aura in all )
		{
			afflicted = ArrayTools.Concat( afflicted, aura.GetTargets() );
		}

		Dictionary<Enemy, bool> dic = new Dictionary<Enemy, bool>();

		for (int i = 0; i < Enemy.alive.Length; i++) 
		{
			dic.Add( Enemy.alive[i], false );
		}

		foreach(Enemy enemy in afflicted) 
		{
			if( dic.ContainsKey( enemy ) && dic[enemy] == false )
			{
				dic[enemy] = true;
			}
		}

		foreach (KeyValuePair<Enemy, bool> pair in dic) 
		{
			if( pair.Value == true )
			{
				if( pair.Key.HasStatus("Slow") == false )
					pair.Key.AddStatus("Slow");
			}
			else
			{
				if( pair.Key.HasStatus("Slow") == true )
					pair.Key.RemoveStatus("Slow");
			}
		}

	}

	public Enemy[] GetTargets()
	{
		Enemy[] result = new Enemy[0];

		for (int i = 0; i < Enemy.alive.Length; i++) 
		{
			float dist = Vector3.Distance( center, Enemy.alive[i].transform.position );
			if( dist < areaOfInfluence )
			{
				result = ArrayTools.Push( result, Enemy.alive[i] );

			}
		}
		return result;
	}


	public override void Update () 
	{
		base.Update();
	}



	public override void OnDestroy()
	{
		base.OnDestroy();

		all = ArrayTools.Remove(all, this );
	}
}
