using System;
using System.Collections.Generic;
using OrcGame.OgEntity.OgItem;

namespace OrcGame.OgEntity.OgCreature;
public class Creature : Entity
{
	public CreatureType CreatureType { get; protected set; } = CreatureType.None;
	public CreatureSubtype CreatureSubtype { get; protected set; } = CreatureSubtype.None;

	public float WorkSpeed { get; protected set; }= 1.0f;

	public IdleState IdleState { get; protected set; } = IdleState.Idle;



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


