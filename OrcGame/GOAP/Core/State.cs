using System;
using System.Collections.Generic;
using System.Linq;
using MonoGame.Extended.Collections;
using OrcGame.Entity.Creature;
using OrcGame.Entity.Item;
using Vector2 = System.Numerics.Vector2;

namespace OrcGame.GOAP.Core;

public class SimulatedState : ISimulated
{
	public SimulatedCreature Creature { get; }
	public Bag<SimulatedItem> AvailableItems { get; } = new();
	public HashSet<SimulatedItemGroup> GroupedAvailableItems { get; } = new();

	public SimulatedState(BaseCreature creature)
	{
		var itemManager = ItemManager.GetItemManager();
		Creature = new SimulatedCreature(creature);
		foreach (var item in itemManager.AvailableItems)
		{
			var simItem = new SimulatedItem(item);
			AvailableItems.Add(simItem);

			var addNewItem = true;
			foreach (var group in GroupedAvailableItems)
			{
				try
				{
					group.AddToGroup(simItem);
					addNewItem = false;
					break;
				}
				catch (NotGroupItemException e) {}
			}
			if (addNewItem) GroupedAvailableItems.Add(new SimulatedItemGroup(simItem));
		}
	}
	
	public SimulatedState(SimulatedState state)
	{
		Creature = new SimulatedCreature(state.Creature);
		foreach (var item in state.AvailableItems)
		{
			var simItem = new SimulatedItem(item);
			AvailableItems.Add(simItem);
		}

		foreach (var group in state.GroupedAvailableItems)
		{
			var simGroup = new SimulatedItemGroup(group);
			GroupedAvailableItems.Add(simGroup);
		}
	}

	public object GetValueForTarget(string target)
	{
		var targetParts = target.Split(".");
		if (!targetParts.Any()) throw new ArgumentException("Target string cannot be split");
		ISimulated currentSim = this;
		object value = null;
		foreach (var propName in targetParts)
		{
			var prop = currentSim.GetType().GetProperty(propName);
			if (prop == null) throw new KeyNotFoundException("State does not have specified target");
			value = prop.GetValue(currentSim);
		}

		return value;
	}
	
	public void SetValueForTarget(string target, object value)
	{
		var targetParts = target.Split(".");
		if (!targetParts.Any()) throw new ArgumentException("Target string cannot be split");
		ISimulated currentSim = this;
		
		foreach (var propName in targetParts)
		{
			var prop = currentSim.GetType().GetProperty(propName);
			if (prop == null) throw new KeyNotFoundException("State does not have specified target");
			if (propName == targetParts.Last()) prop.SetValue(currentSim, value);
		}
	}
}

public class SimulatedCreature : ISimulated
{
	public Vector2 Location { get; private set; }
	public CreatureType CreatureType { get; private set; }
	public CreatureSubtype CreatureSubtype { get; private set; }
	public HashSet<SimulatedItem> Owned { get; private set; } = new();
	public HashSet<SimulatedItem> Carried { get; private set; } = new();
	public HashSet<SimulatedItem> Tagged { get; private set; } = new();

	public SimulatedCreature(BaseCreature creature)
	{
		Location = creature.Location;
		CreatureType = creature.CreatureType;
		CreatureSubtype = creature.CreatureSubtype;
		foreach (var item in creature.Owned)
		{
			Owned.Add(new SimulatedItem(item));
		}

		foreach (var item in creature.Carried)
		{
			Carried.Add(new SimulatedItem(item));
		}
		
		foreach (var item in creature.Tagged)
		{
			Tagged.Add(new SimulatedItem(item));
		}

	}
	
	public SimulatedCreature(SimulatedCreature creature)
	{
		Location = creature.Location;
		CreatureType = creature.CreatureType;
		CreatureSubtype = creature.CreatureSubtype;
		Owned = new HashSet<SimulatedItem>(creature.Owned);
		Carried = new HashSet<SimulatedItem>(creature.Carried);
		Tagged = new HashSet<SimulatedItem>(creature.Tagged);
	}
}

public class BaseSimulatedItem: ISimulated
{
	public Material Material { get; protected set; } = Material.None;
}
public class SimulatedItem : BaseSimulatedItem
{
	public Vector2 Location { get; private set; }

	public SimulatedItem(BaseItem item)
	{
		Material = item.Material;
		Location = item.Location;
	}
	
	public SimulatedItem(SimulatedItem item)
	{
		Material = item.Material;
		Location = item.Location;
	}
	
	public SimulatedItem(SimulatedItemGroup group)
	{
		Material = group.Material;
		Location = group.Locations.Last(); 
	}

	public SimulatedItem(Dictionary<string, dynamic> properties)
	{
		foreach (var prop in properties)
		{
			var propInfo = typeof(SimulatedItem).GetProperty(prop.Key);
			if (propInfo == null) throw new KeyNotFoundException("Items cannot have that property");
			propInfo.SetValue(this, prop.Key);
		}
	}

}
public class SimulatedItemGroup : BaseSimulatedItem
{
	public int Quantity { get; private set; } = 1;
	public List<Vector2> Locations { get; private set; }= new();

	public SimulatedItemGroup(BaseItem item)
	{
		Material = item.Material;
		Locations.Add(item.Location);
	}
	public SimulatedItemGroup(SimulatedItem item)
	{
		Material = item.Material;
		Locations.Add(item.Location);
	}
	
	public SimulatedItemGroup(SimulatedItemGroup group)
	{
		Material = group.Material;
		Quantity = group.Quantity;
		Locations = group.Locations.ToList();
	}

	public void AddToGroup(SimulatedItem item)
	{
		var propsToCompare = typeof(BaseSimulatedItem).GetFields();
		if (propsToCompare.Any(field => field.GetValue(item) != field.GetValue(this))) throw new NotGroupItemException();
		Quantity++;
		Locations.Add(item.Location);
	}

	public void RemoveFromGroup(SimulatedItem item)
	{
		var propsToCompare = typeof(BaseSimulatedItem).GetFields();
		if (propsToCompare.Any(field => field.GetValue(item) != field.GetValue(this))) throw new NotGroupItemException();
		Quantity--;
		Locations.Remove(item.Location);
	}

	public SimulatedItemGroup PopGroupFromGroup(int qty)
	{
		var sub = Math.Min(qty, Quantity);
		Quantity -= sub;
		var returnGroup = new SimulatedItemGroup(this)
		{
			Quantity = sub
		};
		returnGroup.Locations = this.Locations.TakeLast(sub).ToList();
		Locations = Locations.Take(Quantity).ToList();
		return returnGroup;
	}

	public List<SimulatedItem> PopItemsFromGroup(int qty)
	{
		var sub = Math.Min(qty, Quantity);
		Quantity -= sub;
		var returnItems = new List<SimulatedItem>();
		for (var i = 0; i < sub; i++)
		{
			returnItems.Add(new SimulatedItem(this));
		}
		Locations = Locations.Take(Quantity).ToList();
		return returnItems;
	}
}

public interface ISimulated
{
	
}

public class NotGroupItemException : Exception
{
	
} 
