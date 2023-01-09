using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Numerics;
using OrcGame.OgEntity.OgItem;

namespace OrcGame.GOAP.Core;

public class SimulatedItemGroup : SimulatedGroup
{
    public Vector2 Location { get; protected set; } = Vector2.Zero;
    public string EntityName { get; protected set; } = "Generic Entity";
    public string InstanceName { get; protected set; } = "Generic Entity Instance";
    public MaterialType Material { get; protected set; } = MaterialType.None;
    public float Weight { get; protected set; } = 0;
    // public List<Vector2> Locations { get; private set; }= new();

    public SimulatedItemGroup()
    {
        
    }

    public void InitGroup(dynamic item)
    {
        Material = item.Material;
        Weight = item.Weight;
        EntityName = item.EntityName;
        InstanceName = item.InstanceName;
    }
    public SimulatedItemGroup(Item item)
    {
        Material = item.Material;
        Weight = item.Weight;
        EntityName = item.EntityName;
        InstanceName = item.InstanceName;
    }
    public SimulatedItemGroup(SimulatedItem item)
    {
        Material = item.Material;
        Weight = item.Weight;
        EntityName = item.EntityName;
        InstanceName = item.InstanceName;   }
	
    public SimulatedItemGroup(SimulatedItemGroup group)
    {
        Material = group.Material;
        Weight = group.Weight;
        EntityName = group.EntityName;
        InstanceName = group.InstanceName;
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
    
    public bool IsGroupMember(SimulatedItem item)
    {
        if (InstanceName == item.InstanceName && Material == item.Material && Math.Abs(Weight - item.Weight) < 0.001f &&
            EntityName == item.EntityName)
            return true;
        return false;
    }
    
    public int Quantity { get; protected set; } = 1;
    public void AddToGroup(SimulatedItem item)
    {
        // if (!IsGroupMember(item)) throw new NotGroupItemException();
        Quantity++;
        // Locations.Add(item.Location);
    }

    public void RemoveFromGroup(SimulatedItem item)
    {
        // if (!IsGroupMember(item)) throw new NotGroupItemException();
        Quantity--;
        // Locations.Remove(item.Location);
    }

    public override void Reset()
    {
        
    }
}