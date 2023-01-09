using System.Collections.Generic;
using System.Numerics;
using OrcGame.OgEntity.OgItem;

namespace OrcGame.GOAP.Core;


public class SimulatedItem : Simulated
{
    public Vector2 Location { get; protected set; } = Vector2.Zero;
    public string EntityName { get; protected set; } = "Generic Entity";
    public string InstanceName { get; protected set; } = "Generic Entity Instance";
    public MaterialType Material { get; protected set; } = MaterialType.None;
    public float Weight { get; protected set; } = 0;

    public SimulatedItem()
    {
        
    }

    public void InitItem(Item item)
    {
        Material = item.Material;
        Weight = item.Weight;
        EntityName = item.EntityName;
        InstanceName = item.InstanceName;
        Location = item.Location;
    }
    public SimulatedItem(Item item)
    {
        Material = item.Material;
        Weight = item.Weight;
        EntityName = item.EntityName;
        InstanceName = item.InstanceName;
        Location = item.Location;
    }
	
    public SimulatedItem(SimulatedItem item)
    {
        Material = item.Material;
        Weight = item.Weight;
        EntityName = item.EntityName;
        InstanceName = item.InstanceName;
        Location = item.Location;
    }
	
    public SimulatedItem(SimulatedItemGroup group)
    {
        Material = group.Material;
        Weight = group.Weight;
        EntityName = group.EntityName;
        InstanceName = group.InstanceName;
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

    public override void Reset()
    {
        
    }
}