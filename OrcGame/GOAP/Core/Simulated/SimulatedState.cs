using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using OrcGame.OgEntity.OgCreature;
using OrcGame.OgEntity.OgItem;
using OrcGame.Utility;

namespace OrcGame.GOAP.Core;

public class SimulatedState : Simulated
{
    public SimulatedCreature Creature { get; private set;  }
    public HashSet<SimulatedItemGroup> GroupedAvailableItems { get; private set; } = new();

    public SimulatedState()
    {
        
    }
    public SimulatedState(Creature creature)
    {
        var itemManager = ItemManager.GetItemManager();
        // Creature = new SimulatedCreature(creature);
        Creature = Planner.CreaturePool.Request();
        Creature.InitCreature(creature);

        foreach (SimulatedItemGroup group in itemManager.GroupedAvailableItems)
        {
            GroupedAvailableItems.Add(new SimulatedItemGroup(group));
        }
    }

    public void InitState(Creature creature)
    {
        var itemManager = ItemManager.GetItemManager();
        Creature = new SimulatedCreature(creature);

        foreach (SimulatedItemGroup group in itemManager.GroupedAvailableItems)
        {
            GroupedAvailableItems.Add(new SimulatedItemGroup(group));
        }
    }

    public override void Reset()
    {
        Planner.CreaturePool.Dispose(Creature);
        foreach (var item in GroupedAvailableItems)
        {
            // item.Reset();
            Planner.GroupPool.Dispose(item);
        }
        
        GroupedAvailableItems.Clear();
    }
	
    public SimulatedState(SimulatedState state)
    {
        CopyPropertiesOf(state);
    }

    public void CopyPropertiesOf(SimulatedState state)
    {
        Creature = new SimulatedCreature(state.Creature);
        foreach (var group in state.GroupedAvailableItems)
        {
            this.GroupedAvailableItems.Add(new SimulatedItemGroup(group));
        }
    }

    public object GetValueForTarget(string target)
    {
        // TODO: this was written when we were still using dictionaries for the simulated state.
        // TODO: See if there's a better way to access this now.
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
	
    // public void SetValueForTarget(string target, object value)
    // {
    //     var targetParts = target.Split(".");
    //     if (!targetParts.Any()) throw new ArgumentException("Target string cannot be split");
    //     Simulated currentSim = this;
		  //
    //     foreach (var propName in targetParts)
    //     {
    //         var prop = currentSim.GetType().GetProperty(propName);
    //         if (prop == null) throw new KeyNotFoundException("State does not have specified target");
    //         if (propName == targetParts.Last()) prop.SetValue(currentSim, value);
    //     }
    // }
}