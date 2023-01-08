using System.Collections.Generic;
using System.Numerics;
using OrcGame.OgEntity.OgItem;

namespace OrcGame.GOAP.Core;


public class SimulatedItem : Simulated
{
    public MaterialType Material { get; protected set; } = MaterialType.None;
    public Vector2 Location { get; private set; }

    public SimulatedItem(Item item)
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
        // Location = group.Locations.Last(); 
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