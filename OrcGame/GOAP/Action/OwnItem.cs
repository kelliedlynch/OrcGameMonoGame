using System;
using System.Collections.Generic;
using System.Linq;
using OrcGame.GOAP.Core;

namespace OrcGame.GOAP.Action;

public class OwnItem : IGoapAction
{
    // IsValid is run only at the beginning of action planning, and only tests against the current state
    // of the world. IsRelevant is where we determine if the action is still doable after a 
    // change to the simulated state.
    public bool IsValid(Objective objective)
    {
        return true;
    }

    public int GetCost()
    {
        return 0;
    }

    public bool IsRelevant(Objective objective)
    {
        return GoapObjective.ObjectiveContainsRelevantCondition("Creature.Owned", objective);
    }


    public (bool, Objective) TriggerConditionsMet(Objective objective, SimulatedState currentState)
    {
        Dictionary<string, dynamic> lookingFor = GoapObjective.GetRelevantValueFromObjective("Creature.Owned", objective);
        if (lookingFor == null) return (false, objective);
        SimulatedItem found = null;
        foreach (var group in currentState.GroupedAvailableItems)
        {
            var gType = group.GetType();
            if (lookingFor.Keys.All(key => gType.GetProperties().Any(propInfo => propInfo.Name == key)) &&
                lookingFor.Keys.All(key => gType.GetProperty(key)!.GetValue(group) == lookingFor[key]))
            {
                found = group.PopItemsFromGroup(1).First();
                break;
            }
        }

        if (found == null)
        {
            // If relevant item isn't available, add that to the objectives
            OperatorObjective newObjective;
            var availableItem = Planner.QuObjPool.Request();
            availableItem.Target = "GroupedAvailableItems";
            availableItem.QueryType = QueryType.ContainsAtLeast;
            availableItem.Quantity = 1;
            availableItem.PropsQuery = lookingFor;
        
            if (objective is OperatorObjective { Operator: Operator.And } obj)
            {
                obj.ObjectivesList.Add(availableItem);
                newObjective = obj;
            }
            else
            {
                newObjective = Planner.OpObjPool.Request();
                newObjective.Operator = Operator.And;
                newObjective.ObjectivesList = new HashSet<Objective>() { objective, availableItem };
            }

            return (false, newObjective);
        }
        else
        {
            return (true, objective);
        }
    }

    public void ApplyTransform(Objective objective, SimulatedState state)
    {
        Dictionary<string, dynamic> lookingFor = GoapObjective.GetRelevantValueFromObjective("Creature.Owned", objective);
        if (lookingFor == null) throw new FormatException("No relevant conditions in objective");
        SimulatedItem found = null;
        foreach (var group in state.GroupedAvailableItems)
        {
            var gType = group.GetType();
            if (lookingFor.Keys.All(key => gType.GetProperties().Any(propInfo => propInfo.Name == key)) &&
                lookingFor.Keys.All(key => (dynamic)gType.GetProperty(key)!.GetValue(group) == lookingFor[key]))
            {
                found = group.PopItemsFromGroup(1).First();
                break;
            }
        }

        if (found == null)
            throw new MissingMemberException("No relevant item available in State.GroupedAvailableItems");
        state.Creature.Owned.Add(new SimulatedItem(found));
    }
}