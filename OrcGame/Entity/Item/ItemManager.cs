using System;
using System.Collections.Generic;
using System.Linq;
using MonoGame.Extended.Collections;
using OrcGame.Entity.Creature;

namespace OrcGame.Entity.Item;

public sealed class ItemManager
{
    private static readonly Lazy<ItemManager> Instance = new Lazy<ItemManager>(() => new ItemManager());
    public Bag<BaseItem> AvailableItems { get; } = new();

    public Bag<BaseItem> UnavailableItems { get; } = new();

    public static ItemManager GetItemManager() { return Instance.Value; }

    public BaseItem FindNearestItemWithProps(Dictionary<string, object> props)
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

    public void AddItemToWorld(BaseItem item)
    {
        AvailableItems.Add(item);
    }

    public void RemoveItemFromWorld(BaseItem item)
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