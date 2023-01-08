using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using MonoGame.Extended.Collections;
using OrcGame.OgEntity.OgCreature;
using OrcGame.OgEntity.OgItem;
using Vector2 = System.Numerics.Vector2;

namespace OrcGame.GOAP.Core;

public class Simulated
{
	public bool IsGroupMember(Simulated item, SimulatedGroup group)
	{
		var groupProps = group.GetType().GetProperties();
		return false;
	}
}

public class SimulatedGroup : Simulated
{
	public int Quantity { get; protected set; } = 1;
	public void AddToGroup(Simulated item)
	{
		var propsToCompare = typeof(Simulated).GetFields();
		if (propsToCompare.Any(field => field.GetValue(item) != field.GetValue(this))) throw new NotGroupItemException();
		Quantity++;
		// Locations.Add(item.Location);
	}

	public void RemoveFromGroup(Simulated item)
	{
		var propsToCompare = typeof(Simulated).GetFields();
		if (propsToCompare.Any(field => field.GetValue(item) != field.GetValue(this))) throw new NotGroupItemException();
		Quantity--;
		// Locations.Remove(item.Location);
	}
	
}

public class NotGroupItemException : Exception
{
	
} 
