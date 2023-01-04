using System.Collections.Generic;
using OrcGame.Entity.Item;
using MonoGame.Extended.Collections;
using OrcGame.GOAP.Core;

namespace OrcGame.Entity.Creature;
public class BaseCreature : Entity
{
	public CreatureType CreatureType;
	public CreatureSubtype CreatureSubtype;
	public Bag<BaseItem> Owned = new();
	public Bag<BaseItem> Tagged = new();
	public Bag<BaseItem> Carried = new();
	public float WorkSpeed = 1.0f;

	public IdleState IdleState = IdleState.Idle;
	public Bag<GoapGoal> Goals = new();
	public Bag<GoapAction> Actions = new();
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


