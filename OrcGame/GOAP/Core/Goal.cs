using System;
using System.Collections.Generic;
using OrcGame.Entity.Creature;

namespace OrcGame.GOAP.Core;
public abstract class GoapGoal
{
    protected GoapGoal(BaseCreature c)
    {
        Creature = c;
    }

    public BaseCreature Creature { get; set; }
    public abstract Objective GetObjective();
    public abstract bool IsValid(Dictionary<string, object> state);
    public abstract bool TriggerConditionsMet(Dictionary<string, object> state);
}

