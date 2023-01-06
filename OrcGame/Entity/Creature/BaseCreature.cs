using System;
using System.Collections.Generic;
using OrcGame.Entity.Item;
using MonoGame.Extended.Collections;
using OrcGame.GOAP.Core;

namespace OrcGame.Entity.Creature;
public class BaseCreature : Entity
{
	public CreatureType CreatureType;
	public CreatureSubtype CreatureSubtype;
	public List<BaseItem> Owned { get; } = new();

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
	public List<BaseItem> Tagged { get; } = new();
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
	public List<BaseItem> Carried { get; } = new();
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
	public float WorkSpeed = 1.0f;

	public IdleState IdleState = IdleState.Idle;
	public List<GoapGoal> Goals = new();
	public List<GoapAction> Actions = new();
}

public enum CreatureType
{
	Humanoid,
	Beast
}

public enum CreatureSubtype
{
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


