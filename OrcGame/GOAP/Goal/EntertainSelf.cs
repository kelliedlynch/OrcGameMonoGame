using System;
using System.Collections.Generic;
using OrcGame.OgEntity.OgCreature;
using OrcGame.GOAP.Core;

namespace OrcGame.GOAP.Goal;
public class EntertainSelf : GoapGoal
{
    public override Objective GetObjective()
    {
        var avoidCondition = new Dictionary<string, dynamic>() {
            { "Creature", new Dictionary<string, dynamic>() {
                { "IdleState", IdleState.Idle }
            }}
        };
        return new ValueObjective()
        {
            Target = "Creature.IdleState",
            Conditional = Conditional.DoesNotEqual,
            ValueType = typeof(IdleState),
            Value = IdleState.Idle
        };
    }

    public override GoalPriority GetPriority()
    {
        return GoalPriority.Idle;
    }

    public override bool IsValid()
    {
        return true;
    }

    public override bool TriggerConditionsMet()
    {
        return Creature.IdleState == IdleState.Idle;
    }

    public EntertainSelf(Creature creature) : base(creature)
    {
    }
}


