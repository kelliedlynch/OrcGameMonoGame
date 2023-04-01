using System.Collections.Generic;
using System.Numerics;
using OrcGame.OgEntity.OgCreature;

namespace OrcGame.GOAP.Core;

public class SimulatedCreature : Core.Simulated
{
    public Vector2 Location { get; private set; }
    public CreatureType CreatureType { get; private set; }
    public CreatureSubtype CreatureSubtype { get; private set; }
    public IdleState IdleState { get; set; }
    public HashSet<SimulatedItem> Owned { get; private set; } = new();
    public HashSet<SimulatedItem> Carried { get; private set; } = new();
    public HashSet<SimulatedItem> Tagged { get; private set; } = new();

    public SimulatedCreature()
    {
        
    }

    public void InitCreature(Creature creature)
    {
        Location = creature.Location;
        CreatureType = creature.CreatureType;
        CreatureSubtype = creature.CreatureSubtype;
        IdleState = creature.IdleState;
        foreach (var item in creature.Owned)
        {
            Owned.Add(new SimulatedItem(item));
        }

        foreach (var item in creature.Carried)
        {
            Carried.Add(new SimulatedItem(item));
        }
		
        foreach (var item in creature.Tagged)
        {
            Tagged.Add(new SimulatedItem(item));
        }
    }
    public SimulatedCreature(Creature creature)
    {
        Location = creature.Location;
        CreatureType = creature.CreatureType;
        CreatureSubtype = creature.CreatureSubtype;
        IdleState = creature.IdleState;
        foreach (var item in creature.Owned)
        {
            Owned.Add(new SimulatedItem(item));
        }

        foreach (var item in creature.Carried)
        {
            Carried.Add(new SimulatedItem(item));
        }
		
        foreach (var item in creature.Tagged)
        {
            Tagged.Add(new SimulatedItem(item));
        }

    }
	
    public SimulatedCreature(SimulatedCreature creature)
    {
        Location = creature.Location;
        CreatureType = creature.CreatureType;
        CreatureSubtype = creature.CreatureSubtype;
        IdleState = creature.IdleState;
        Owned = new HashSet<SimulatedItem>(creature.Owned);
        Carried = new HashSet<SimulatedItem>(creature.Carried);
        Tagged = new HashSet<SimulatedItem>(creature.Tagged);
    }

    public override void Reset()
    {
        foreach (var item in Owned)
        {
            item.Reset();
            Planner.ItemPool.Dispose(item);
        }
        foreach (var item in Tagged)
        {
            item.Reset();
            Planner.ItemPool.Dispose(item);
        }
        foreach (var item in Carried)
        {
            item.Reset();
            Planner.ItemPool.Dispose(item);
        }
        
        Owned.Clear();
        Carried.Clear();
        Tagged.Clear();
    }
}