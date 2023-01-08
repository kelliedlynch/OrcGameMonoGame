using System;
using System.Collections.Generic;
using OrcGame.OgEntity.OgCreature;

namespace OrcGame.GOAP.Core;
public abstract class GoapGoal
{
    protected GoapGoal(Creature c)
    {
        Creature = c;
    }

    public Creature Creature { get; set; }
    public abstract Objective GetObjective();
    public abstract bool IsValid();
    public abstract bool TriggerConditionsMet();

    public abstract GoalPriority GetPriority();

    // public abstract bool IsMet();
    public enum GoalPriority
    {
        Idle,
        Want,
        Work,
        Need,
        Emergency
    }
}

