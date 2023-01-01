using System.Collections.Generic;

namespace OrcGame.Entity
{

	public class Creature : Entity
	{

		public CreatureType CreatureType;
		public CreatureSubtype CreatureSubtype;
		public List<Item> Owned = new();
		public List<Item> Tagged = new();
		public List<Item> Carried = new();

		public IdleState IdleState;
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
}

