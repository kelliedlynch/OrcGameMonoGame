using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Microsoft.Xna.Framework;
using MonoGame.Extended.Collections;
using OrcGame.Entity.Creature;
using OrcGame.Entity.Item;
using Vector2 = System.Numerics.Vector2;

namespace OrcGame.GOAP.Core;
public static class GoapState
{
	private static readonly ItemManager ItemManager = ItemManager.GetItemManager();
	
	public static Dictionary<string, object> SimulateWorldStateFor(BaseCreature creature)
	{
		var avail = new HashSet<Dictionary<string, object>>(11000);
		foreach (var item in ItemManager.AvailableItems)
		{
			avail.Add(SimulateEntity(item));
		}
		var state = new Dictionary<string, object>()
		{
			{ "Creature", SimulateEntity(creature) },
			{ "AvailableItems", avail }
		};
		return state;
	}
	public static Dictionary<string, object> SimulateEntity(Entity.Entity entity)
	{
		var propValueList = new Dictionary<string, object>();
		foreach (var prop in entity.GetType().GetFields())
		{
			var value = prop.GetValue(entity);
  
			if (prop.FieldType.IsValueType || prop.FieldType == typeof(string))
			{
                propValueList.Add(prop.Name, value);
				continue;
            }
  
			if (value is not IEnumerable cValue) continue;
			var collectionValue = new Bag<Dictionary<string, dynamic>>();
			foreach (var item in cValue)
			{
				if (item is not Entity.Entity eItem) continue;
				collectionValue.Add(SimulateEntity(eItem));
			}
			propValueList.Add(prop.Name, collectionValue);
		}
		foreach (var prop in entity.GetType().GetProperties())
		{
			var value = prop.GetValue(entity);
  
			if (prop.PropertyType.IsValueType || prop.PropertyType == typeof(string))
			{
				propValueList.Add(prop.Name, value);
				continue;
			}
  
			if (value is not IEnumerable cValue) continue;
			var collectionValue = new Bag<Dictionary<string, dynamic>>();
			foreach (var item in cValue)
			{
				if (item is not Entity.Entity eItem) continue;
				collectionValue.Add(SimulateEntity(eItem));
			}
			propValueList.Add(prop.Name, collectionValue);
		}
		return propValueList;
	}

	public static Dictionary<string, object> CloneState(Dictionary<string, object> state)
	{
		var clone = new Dictionary<string, object>();
		foreach (var item in state)
		{
			switch (item.Value)
			{
				case Bag<Dictionary<string, object>> list:
				{
					var cloneList = new Bag<Dictionary<string, object>>();
					foreach (var arrayItem in list)
					{
						cloneList.Add(new Dictionary<string, object>(arrayItem));
						// if (arrayItem != null)
						// {
						// 	cloneList.Add(CloneState(arrayItem));
						// }
					}
					clone[item.Key] = cloneList;
					// clone[item.Key] = list.MemberwiseClone();
					break;
				}
				case HashSet<Dictionary<string, object>> hashSet:
				{
					var cloneList = new HashSet<Dictionary<string, object>>();
					foreach (var setItem in hashSet)
					{
						cloneList.Add(new Dictionary<string, object>(setItem));
					}
					clone[item.Key] = cloneList;
					break;
				}
				case Dictionary<string, object> dict:
				{
					var cloneDict = CloneState(dict);
					clone[item.Key] = cloneDict;
					break;
				}
				default:
					clone[item.Key] = item.Value;
					break;
			}
		}
		
		return clone;
	}
	
	public static dynamic GetValueForKey(string target, Dictionary<string, dynamic> state)
	{
		dynamic relevantValue = null;
		var nestedTarget = target.Split(".");
		var current = state;
		for (var i=0; i < nestedTarget.Length - 1; i++)
		{
			if (current[nestedTarget[i]] is Dictionary<string, dynamic> cur) current = cur;
		}

		if (current[nestedTarget[^1]] is Bag<Dictionary<string, dynamic>> sub) relevantValue = sub;

		return relevantValue;
	}
	
	public static void SetValueForKey(string target, dynamic value, Dictionary<string, dynamic> state)
	{
		var targetAsArray = target.Split(".");
		var targetRoot = targetAsArray.First();
		var targetStep = "";
		
		if (targetAsArray.Length > 1)
		{
			var targetPrefix = targetAsArray.Skip(1).ToArray();
			targetStep = targetPrefix!.First();
			foreach (var element in targetPrefix.Skip(1))
			{
				targetStep += "." + element;
			}

			SetValueForKey(targetStep, value, state[targetRoot]);
			value = state[targetRoot];
		}

		state[targetRoot] = value;
	}
}

public static class GoapSimulator
{
	public static SimulatedState SimulateWorldStateFor(BaseCreature creature)
	{
		var state = new SimulatedState();
	}
}

public class SimulatedState
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
}

public class SimulatedCreature
{
	public Vector2 Location;
	public CreatureType CreatureType;
	public CreatureSubtype CreatureSubtype;
	public Bag<SimulatedItemGroup> Owned;
	public Bag<SimulatedItemGroup> Carried;
	public Bag<SimulatedItemGroup> Tagged;

	public SimulatedCreature(BaseCreature creature)
	{
		foreach (var field in typeof(SimulatedCreature).GetFields())
		{
			field.SetValue(this, field.GetValue(creature));
		}
	}
}


public class BaseSimulatedItem
{
	public Material Material = Material.None;
}
public class SimulatedItem : BaseSimulatedItem
{
	public Vector2 Location = Vector2.Zero;

	public SimulatedItem(BaseItem item)
	{
		Material = item.Material;
		Location = item.Location;
	}
}
public class SimulatedItemGroup : BaseSimulatedItem
{
	public int Quantity;
	public List<Vector2> Locations = new();

	public SimulatedItemGroup(BaseItem item)
	{
		Material = item.Material;
		Quantity = 1;
		Locations.Add(item.Location);
	}
	public SimulatedItemGroup(SimulatedItem item)
	{
		Material = item.Material;
		Quantity = 1;
		Locations.Add(item.Location);
	}

	public void AddToGroup(SimulatedItem item)
	{
		var propsToCompare = typeof(BaseSimulatedItem).GetFields();
		if (propsToCompare.Any(field => field.GetValue(item) != field.GetValue(this))) throw new NotGroupItemException();
		Quantity++;
		Locations.Add(item.Location);
	}
}

public class NotGroupItemException : Exception
{
	
} 
