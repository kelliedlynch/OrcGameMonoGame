using System;
using System.Collections.Generic;
using System.Linq;
using OrcGame.GOAP.Core;

namespace OrcGame.OgEntity.OgItem;

public sealed class ItemManager
{
    private static readonly Lazy<ItemManager> Instance = new Lazy<ItemManager>(() => new ItemManager());
    public HashSet<Item> AvailableItems { get; } = new();
    public HashSet<SimulatedItemGroup> GroupedAvailableItems { get; private set; } = new();
    public HashSet<Item> UnavailableItems { get; } = new();

    public static ItemManager GetItemManager() { return Instance.Value; }

    public Item FindNearestItemWithProps(Dictionary<string, object> props)
    {
        // TODO: Make this actually find the nearest item, instead of a random one
        foreach (var item in AvailableItems)
        {
            if (props.Keys.Any(key => item.GetType().GetField(key) == null)) { continue; }

            if (props.Keys.All(key => item.GetType().GetField(key)!.GetValue(item) == props[key]))
            {
                return item;
            }
        }

        return null;
    }

    public void AddItemToWorld(Item item)
    {
        AvailableItems.Add(item);

        // if (!GroupedAvailableItems.Any())
        // {
        //     GroupedAvailableItems.Add(new SimulatedItemGroup(item));
        //     return;
        // }
        var sim = new SimulatedItem(item);
        foreach (var group in GroupedAvailableItems)
        {
            if (group.IsGroupMember(sim))
            {
                group.AddToGroup(sim);
                return;
            }
        }
        GroupedAvailableItems.Add(new SimulatedItemGroup(item));
    }

    public void RemoveItemFromWorld(Item item)
    {
        if (AvailableItems.Contains(item))
        {
            AvailableItems.Remove(item);
        } else if (UnavailableItems.Contains(item))
        {
            UnavailableItems.Remove(item);
        }
    }
}