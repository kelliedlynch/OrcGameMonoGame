using System.Collections.Generic;
using System.Linq;
using MonoGame.Extended.Collections;
using OrcGame.GOAP.Core;
namespace OrcGame.GOAP.Action;

public class PickUpItem : GoapAction
{
    private Dictionary<string, dynamic> _found;
    private Dictionary<string, dynamic> FindRelevantConditionInObjective(Objective objective)
    {
        if (objective is QueryObjective obj1)
        {
            return obj1.Target == "Creature.Carried" ? obj1.PropsQuery : null;
        } else if (objective is OperatorObjective { Operator: not Operator.Not } obj2)
        {
            return obj2.ObjectivesList.Select(FindRelevantConditionInObjective).FirstOrDefault();
        }
        return null;
    }
    
    // IsValid is run only at the beginning of action planning, and only tests against the current state
    // of the world. IsRelevant is where we determine if the action is still doable after a 
    // change to the simulated state.
    public override bool IsValid(Objective objective)
    {
        return true;
    }

    public override (bool, Dictionary<string, dynamic>) ApplyTransformIfRelevant(Objective objective, Dictionary<string, dynamic> oldState)
    {
        // Does the objective want an item in Creature.Carried?
        var lookingFor = FindRelevantConditionInObjective(objective);
        // Is there a matching item in Creature.Tagged?
        var tagged = oldState["Creature"]["Tagged"] as Bag<Dictionary<string, dynamic>>;
        var found =
            (from item in tagged
                where lookingFor.Keys.All(key => item.ContainsKey(key) && item[key] == lookingFor[key])
                select item).FirstOrDefault();
        // If matching item found, remove it from the state and return
        if (found == null) return (false, oldState);
        _found = found;
        var state = GoapState.CloneState(oldState);
        return ApplyTransform(state);
    }

    public override bool TriggerConditionsMet(Dictionary<string, dynamic> worldState, Dictionary<string, dynamic> goalState)
    {
        throw new System.NotImplementedException();
    }

    public override (bool, Dictionary<string, dynamic>) ApplyTransform(Dictionary<string, dynamic> state)
    {
        // NOTE: is mutating the state necessary? I'm pretty sure since Dictionary is reference typed,
        // it will work without
        if (state["Creature"]["Tagged"] is not Bag<Dictionary<string, dynamic>> newTagged) return (false, state);
        foreach (var entry in newTagged)
        {
            if (_found.Keys.Any(key => _found[key] != entry[key])) continue;
            newTagged.Remove(entry);
            state["Creature"]["Carried"].Add(entry);
            break;
        }

        return (true, state);
    }
}