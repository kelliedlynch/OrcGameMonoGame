using System;
using System.Collections.Generic;
using System.Linq;

namespace OrcGame.OgEntity.OgItem;

public sealed class ItemManager
{
    private static readonly Lazy<ItemManager> Instance = new Lazy<ItemManager>(() => new ItemManager());
    public HashSet<Item> WorldItems { get; } = new();
    public HashSet<Item> AvailableItems => WorldItems.Where(x => !x.IsTagged && !x.IsOwned).ToHashSet();
    public HashSet<Item> UnavailableItems => WorldItems.Where(x => x.IsTagged || x.IsOwned).ToHashSet();
    public HashSet<Item> ItemsOnGround => WorldItems.Where(x => !x.IsCarried).ToHashSet();

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
        WorldItems.Add(item);
    }

    public void RemoveItemFromWorld(Item item)
    {
        WorldItems.Remove(item);
    }
}