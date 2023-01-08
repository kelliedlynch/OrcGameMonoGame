using System;
using System.Collections.Generic;
using System.Linq;
using OrcGame.OgEntity.OgCreature;
using OrcGame.OgEntity.OgItem;

namespace OrcGame.GOAP.Core;

public class SimulatedState : Simulated
{
    public SimulatedCreature Creature { get; }
    public HashSet<SimulatedItemGroup> GroupedAvailableItems { get; } = new();

    public SimulatedState(Creature creature)
    {
        var itemManager = ItemManager.GetItemManager();
        Creature = new SimulatedCreature(creature);

        foreach (SimulatedItemGroup group in itemManager.GroupedAvailableItems)
        {
            GroupedAvailableItems.Add(new SimulatedItemGroup(group));
        }
    }
	
    public SimulatedState(SimulatedState state)
    {
        Creature = new SimulatedCreature(state.Creature);

        foreach (var group in state.GroupedAvailableItems)
        {
            GroupedAvailableItems.Add(new SimulatedItemGroup(group));
        }
    }

    public object GetValueForTarget(string target)
    {
        var targetParts = target.Split(".");
        if (!targetParts.Any()) throw new ArgumentException("Target string cannot be split");
        Simulated currentSim = this;
        object value = null;
        foreach (var propName in targetParts)
        {
            if (currentSim == null) break;
            var prop = currentSim.GetType().GetProperty(propName);
            if (prop == null) throw new KeyNotFoundException("State does not have specified target");
            value = prop.GetValue(currentSim);
            currentSim = value as Simulated;
        }

        return value;
    }
	
    public void SetValueForTarget(string target, object value)
    {
        var targetParts = target.Split(".");
        if (!targetParts.Any()) throw new ArgumentException("Target string cannot be split");
        Simulated currentSim = this;
		
        foreach (var propName in targetParts)
        {
            var prop = currentSim.GetType().GetProperty(propName);
            if (prop == null) throw new KeyNotFoundException("State does not have specified target");
            if (propName == targetParts.Last()) prop.SetValue(currentSim, value);
        }
    }
}