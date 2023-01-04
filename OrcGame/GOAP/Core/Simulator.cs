using System.Collections;
using System.Collections.Generic;
using MonoGame.Extended.Collections;
using OrcGame.Entity.Creature;
using OrcGame.Entity.Item;

namespace OrcGame.GOAP.Core;
public static class GoapSimulator
{
	private static readonly ItemManager ItemManager = ItemManager.GetItemManager();
	
	public static Dictionary<string, object> SimulateWorldStateFor(BaseCreature creature)
	{
		var avail = new Bag<Dictionary<string, object>>();
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
		return propValueList;
	}

	public static Dictionary<string, dynamic> CloneState(Dictionary<string, dynamic> state)
	{
		var clone = new Dictionary<string, dynamic>();
		foreach (var item in state)
		{
			switch (item.Value)
			{
				case Bag<Dictionary<string, dynamic>> list:
				{
					var cloneList = new Bag<Dictionary<string, dynamic>>();
					foreach (var arrayItem in list)
					{
						if (arrayItem != null)
						{
							cloneList.Add(CloneState(arrayItem));
						}
					}

					clone[item.Key] = cloneList;
					break;
				}
				case Dictionary<string, dynamic> dict:
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
}

