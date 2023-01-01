using System.Collections;
using System.Collections.Generic;

namespace OrcGame.GOAP
{
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

				if (value is not ICollection cValue) continue;
				var collectionValue = new ArrayList();
				foreach (var item in cValue)
				{
					if (item is Entity.Entity eItem)
					{
						collectionValue.Add(SimulateEntity(eItem));
						continue;
					}
					collectionValue.Add(item);
				}
				propValueList.Add(prop.Name, collectionValue);
			}
			return propValueList;
		}
	}
}

