using System.Collections.Generic;
using MonoGame.Extended.Collections;
using OrcGame.GOAP.Core;

namespace OrcGame.GOAP.Action;

public class TagItem : GoapAction
{
    public override bool IsValid(Objective objective)
    {
        return true;
    }

    public override (bool, Dictionary<string, dynamic>) IsRelevant(Objective objective,
        Dictionary<string, dynamic> oldState)
    {
        if (!ObjectiveContainsRelevantCondition("Creature.Tagged", objective)) return (false, oldState);
        var available = (Bag<Dictionary<string, dynamic>>)oldState["AvailableItems"];
    }

    public override bool TriggerConditionsMet(Dictionary<string, object> worldState, Dictionary<string, object> goalState)
    {
        throw new System.NotImplementedException();
    }

    public override (bool, Dictionary<string, dynamic>) ApplyTransform(Dictionary<string, object> state)
    {
        throw new System.NotImplementedException();
    }
}