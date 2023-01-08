using System;
using System.Collections.Generic;
using OrcGame.OgEntity.OgItem;

namespace OrcGame.GOAP.Core;

public class SimulatedItemGroup : SimulatedGroup
{
    public MaterialType Material { get; protected set; } = MaterialType.None;
    
    // public List<Vector2> Locations { get; private set; }= new();

    public SimulatedItemGroup(Item item)
    {
        Material = item.Material;
        // Locations.Add(item.Location);
    }
    public SimulatedItemGroup(SimulatedItem item)
    {
        Material = item.Material;
        // Locations.Add(item.Location);
    }
	
    public SimulatedItemGroup(SimulatedItemGroup group)
    {
        Material = group.Material;
        Quantity = group.Quantity;
        // Locations = group.Locations.ToList();
    }

    public SimulatedItemGroup PopGroupFromGroup(int qty)
    {
        var sub = Math.Min(qty, Quantity);
        Quantity -= sub;

        var returnGroup = new SimulatedItemGroup(this)
        {
            Quantity = sub
        };
        return returnGroup;
    }
    
    public HashSet<SimulatedItem> PopItemsFromGroup(int qty)
    {
        var sub = Math.Min(qty, Quantity);
        Quantity -= sub;
        var returnItems = new HashSet<SimulatedItem>();
        for (var i = 0; i < sub; i++)
        {
            returnItems.Add(new SimulatedItem(this));
        }
        return returnItems;
    }

}