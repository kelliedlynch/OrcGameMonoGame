using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using MonoGame.Extended.Collections;
using OrcGame.Entity.Creature;
using OrcGame.Entity.Item;

namespace OrcGame.GOAP.Core;
public static class GoapState
{
	private static readonly ItemManager ItemManager = ItemManager.GetItemManager();
	
	public static Dictionary<string, object> SimulateWorldStateFor(BaseCreature creature)
	{
		// var avail = new List<Dictionary<string, object>>(11000);
		// for (var i = 0; i < 11000; i++) avail[i] = GoapState.SimulateEntity(ItemManager.AvailableItems[i]);
		var avail = new List<Dictionary<string, object>>(11000);
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
		// var propsAndFields = new List<dynamic>();
		// var fields = entity.GetType().GetFields();
		// var props = entity.GetType().GetProperties();
		// propsAndFields.AddRange(fields);
		// propsAndFields.AddRange(props);
		//
		// var propValList = new Dictionary<string, object>();
		// foreach (var member in propsAndFields)
		// {
		// 	var value = member.GetValue(entity);
		// 	// NOTE: SKIPPING NULL VALUES MIGHT BREAK STUFF LATER, DO THIS BETTER
		// 	if (value == null) continue;
		// 	if (value.GetType().IsValueType || value.GetType() is string)
		// 	{
		// 		propValList.Add(member.Name, value);
		// 		continue;
		// 	}
		//
		// 	if (value is not IEnumerable cValue) continue;
		// 	var collectionValue = new List<Dictionary<string, dynamic>>();
		// 	foreach (var item in cValue)
		// 	{
		// 		if (item is not Entity.Entity eItem) continue;
		// 		collectionValue.Add(SimulateEntity(eItem));
		// 	}
		// 	propValList.Add(member.Name, collectionValue);
		// }
		
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
			var collectionValue = new List<Dictionary<string, dynamic>>();
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
			var collectionValue = new List<Dictionary<string, dynamic>>();
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
				case List<Dictionary<string, object>> list:
				{
					var cloneList = new List<Dictionary<string, object>>();
					foreach (var arrayItem in list)
					{
						cloneList.Add(new Dictionary<string, object>(arrayItem));
						// if (arrayItem != null)
						// {
						// 	cloneList.Add(CloneState(arrayItem));
						// }
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

		if (current[nestedTarget[^1]] is List<Dictionary<string, dynamic>> sub) relevantValue = sub;

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

