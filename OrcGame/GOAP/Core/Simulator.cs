using System.Collections;
using System.Collections.Generic;
using MonoGame.Extended.Collections;

namespace OrcGame.GOAP.Core;
public static class GoapSimulator
{
	public static Dictionary<string, object> SimulateEntity(Entity.Entity entity)
	{
		var propValueList = new Dictionary<string, object>();
		foreach (var prop in entity.GetType().GetFields())
		{

			var value = prop.GetValue(entity);

			if (prop.FieldType.IsValueType)
			{
                propValueList.Add(prop.Name, value);
				continue;
            }

			if (value is not IEnumerable cValue) continue;
			var collectionValue = new Bag<Dictionary<string, object>>();
			foreach (var item in cValue)
			{
				if (item is Entity.Entity eItem)
				{
					collectionValue.Add(SimulateEntity(eItem));
					continue;
				}
				// collectionValue.Add(item);
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
						if (arrayItem != null)
						{
							cloneList.Add(CloneState(arrayItem));
						}
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
}

