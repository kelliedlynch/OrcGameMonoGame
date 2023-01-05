using System.Collections.Generic;
using System.Linq;
using MonoGame.Extended.Collections;
using OrcGame.GOAP.Core;
namespace OrcGame.GOAP.Action;

public class PickUpItem : GoapAction
{
    // IsValid is run only at the beginning of action planning, and only tests against the current state
    // of the world. IsRelevant is where we determine if the action is still doable after a 
    // change to the simulated state.
    public override bool IsValid(Objective objective)
    {
        return true;
    }

    public bool IsRelevant(Objective objective)
    {
        return GoapObjective.ObjectiveContainsRelevantCondition("Creature.Carried", objective);
    }

    public override (bool, Dictionary<string, dynamic>) ApplyTransform(Objective objective,
        Dictionary<string, dynamic> oldState)
    {

    }

    public override (bool, Objective) TriggerConditionsMet(Objective objective, Dictionary<string, dynamic> currentState)
    {
        var lookingFor = GoapObjective.GetRelevantValueFromObjective("Creature.Carried", objective);
        var availableItems = GoapState.GetValueForKey("AvailableItems", currentState);
        
        
        
        // var found = FindRelevantItemIfExists(lookingFor, currentState);
        if (found == null) return (false, objective);

    }

    public override (bool, Dictionary<string, dynamic>) ApplyTransform(Dictionary<string, dynamic> state)
    {
        // NOTE: is mutating the state necessary? I'm pretty sure since Dictionary is reference typed,
        // it will work without
        // if (state["Creature"]["Tagged"] is not Bag<Dictionary<string, dynamic>> newTagged) return (false, state);
        foreach (var entry in state["Creature"]["Tagged"])
        {
            state["Creature"]["Carried"].Add(entry);
            if (_found.Keys.Any(key => _found[key] != entry[key])) continue;
            state["Creature"]["Tagged"].Remove(entry);
            break;
        }

        return (true, state);
    }
}