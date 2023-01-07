using System;
using System.Collections.Generic;
using OrcGame.Entity.Item;
using MonoGame.Extended.Collections;
using OrcGame.GOAP.Core;

namespace OrcGame.Entity.Creature;
public class BaseCreature : Entity
{
	public CreatureType CreatureType { get; protected set; } = CreatureType.None;
	public CreatureSubtype CreatureSubtype { get; protected set; } = CreatureSubtype.None;
	public HashSet<BaseItem> Owned { get; protected set; } = new();
	public Bag<BaseItem> Carried { get; protected set; } = new();
	public Bag<BaseItem> Tagged { get; protected set; } = new();
	public void AddToOwned(BaseItem item)
	{
		if(Owned.Contains(item)) throw new ArgumentException("Creature already owns that item");
		Owned.Add(item);
	}
	
	public void RemoveFromOwned(BaseItem item)
	{
		if(!Owned.Contains(item)) throw new ArgumentException("Creature does not own that item");
		Owned.Add(item);
	}

	public void AddToTagged(BaseItem item)
	{
		if(Tagged.Contains(item)) throw new ArgumentException("Creature already tagged that item");
		Tagged.Add(item);
	}
	public void RemoveFromTagged(BaseItem item)
	{
		if(!Tagged.Contains(item)) throw new ArgumentException("Creature has not tagged that item");
		Tagged.Add(item);
	}
	public void AddToCarried(BaseItem item)
	{
		if(Carried.Contains(item)) throw new ArgumentException("Creature already carries that item");
		Carried.Add(item);
	}
	public void RemoveFromCarried(BaseItem item)
	{
		if(!Carried.Contains(item)) throw new ArgumentException("Creature is not carrying that item");
		Carried.Add(item);
	}
	public float WorkSpeed { get; protected set; }= 1.0f;

	public IdleState IdleState { get; protected set; } = IdleState.Idle;
	public HashSet<GoapGoal> Goals { get; protected set; } = new();
	public HashSet<IGoapAction> Actions { get; protected set; } = new();
}

public enum CreatureType
{
	None,
	Humanoid,
	Beast
}

public enum CreatureSubtype
{
	None,
	Orc,
	Dwarf
}

public enum IdleState
{
	Idle,
	Working,
	Playing,
	Surviving
}


