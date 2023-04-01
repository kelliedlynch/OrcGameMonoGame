using System;
using System.Collections.Generic;
using OrcGame.GOAP;
using OrcGame.OgEntity.OgItem;
using OrcGame.GOAP.Core;

namespace OrcGame.OgEntity.OgCreature;
public class Creature : Entity
{
	public CreatureType CreatureType { get; protected set; } = CreatureType.None;
	public CreatureSubtype CreatureSubtype { get; protected set; } = CreatureSubtype.None;
	public HashSet<Item> Owned { get; protected set; } = new();
	public HashSet<Item> Carried { get; protected set; } = new();
	public HashSet<Item> Tagged { get; protected set; } = new();
	public void AddToOwned(Item item)
	{
		if(Owned.Contains(item)) throw new ArgumentException("Creature already owns that item");
		Owned.Add(item);
	}
	
	public void RemoveFromOwned(Item item)
	{
		if(!Owned.Contains(item)) throw new ArgumentException("Creature does not own that item");
		Owned.Add(item);
	}

	public void AddToTagged(Item item)
	{
		if(Tagged.Contains(item)) throw new ArgumentException("Creature already tagged that item");
		Tagged.Add(item);
	}
	public void RemoveFromTagged(Item item)
	{
		if(!Tagged.Contains(item)) throw new ArgumentException("Creature has not tagged that item");
		Tagged.Add(item);
	}
	public void AddToCarried(Item item)
	{
		if(Carried.Contains(item)) throw new ArgumentException("Creature already carries that item");
		Carried.Add(item);
	}
	public void RemoveFromCarried(Item item)
	{
		if(!Carried.Contains(item)) throw new ArgumentException("Creature is not carrying that item");
		Carried.Add(item);
	}
	public float WorkSpeed { get; protected set; }= 1.0f;

	public IdleState IdleState { get; protected set; } = IdleState.Idle;
	public HashSet<GoapGoal> Goals { get; protected set; } = new();
	public HashSet<IGoapAction> Actions { get; protected set; } = new();
	public GoapGoal CurrentGoal { get; set; }
	public Branch CurrentPlan { get; set; }
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


