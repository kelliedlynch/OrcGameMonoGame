using System;
using System.Collections.Generic;
using OrcGame.Entity.Creature;
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

    public override bool IsValid(Dictionary<string, object> state)
    {
        return true;
    }

    public override bool TriggerConditionsMet(Dictionary<string, object> state)
    {
        if (state["Creature"] is not Dictionary<string, object> creature) return false;
        if (creature["IdleState"] is not IdleState cState) return false;
        return cState == IdleState.Idle;
    }

    public EntertainSelf(BaseCreature creature) : base(creature)
    {
    }
}


