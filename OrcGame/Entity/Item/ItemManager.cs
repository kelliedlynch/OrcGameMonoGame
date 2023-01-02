using System;
using System.Collections.Generic;
using System.Linq;
using MonoGame.Extended.Collections;

namespace OrcGame.Entity.Item;

public sealed class ItemManager
{
    private static readonly Lazy<ItemManager> Instance = new Lazy<ItemManager>(() => new ItemManager());
    private readonly Bag<BaseItem> _availableItems = new();
    private readonly Bag<BaseItem> _unavailableItems = new();

    public static ItemManager GetItemManager() { return Instance.Value; }

    public BaseItem FindNearestItemWithProps(Dictionary<string, object> props)
    {
        // TODO: Make this actually find the nearest item, instead of a random one
        foreach (var item in _availableItems)
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
        _availableItems.Add(item);
    }

    public void RemoveItemFromWorld(BaseItem item)
    {
        if (_availableItems.Contains(item))
        {
            _availableItems.Remove(item);
        } else if (_unavailableItems.Contains(item))
        {
            _unavailableItems.Remove(item);
        }
    }
}