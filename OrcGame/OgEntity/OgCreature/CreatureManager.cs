using System;
using System.Collections.Generic;
using System.Linq;

namespace OrcGame.OgEntity.OgCreature;

public class CreatureManager
{
    private static readonly Lazy<CreatureManager> Instance = new Lazy<CreatureManager>(() => new CreatureManager());
    public HashSet<Creature> WorldCreatures { get; } = new();
    public HashSet<Creature> IdleCreatures => WorldCreatures.Where(x => x.IdleState == IdleState.Idle).ToHashSet();


    public static CreatureManager GetCreatureManager() { return Instance.Value; }

    public void AddCreatureToWorld(Creature creature)
    {
        WorldCreatures.Add(creature);
    }
    
    public void RemoveCreatureFromWorld(Creature creature)
    {
        WorldCreatures.Remove(creature);
    }
}