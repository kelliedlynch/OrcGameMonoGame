using System;
using System.Collections.Generic;
using OrcGame.Entity.Creature;
using OrcGame.GOAP.Core;

namespace OrcGame.GOAP.Goal;
public class EntertainSelf : GoapGoal
{
    public override Objective GetObjective(Dictionary<string, object> simulatedState)
    {
        throw new NotImplementedException();
    }

    public override bool IsValid(Dictionary<string, object> state)
    {
        throw new NotImplementedException();
    }

    public override bool TriggerConditionsMet(Dictionary<string, object> state)
    {
        throw new NotImplementedException();
    }

    public EntertainSelf(BaseCreature creature) : base(creature)
    {
    }
}


