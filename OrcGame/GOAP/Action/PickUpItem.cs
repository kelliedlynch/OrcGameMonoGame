using System.Collections.Generic;
using System.Linq;
using MonoGame.Extended.Collections;
using OrcGame.Entity.Item;
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

    public override bool IsRelevant(Objective objective)
    {
        return GoapObjective.ObjectiveContainsRelevantCondition("Creature.Carried", objective);
    }


    public override (bool, Objective, Dictionary<string, dynamic>) TriggerConditionsMet(Objective objective, Dictionary<string, dynamic> currentState)
    {
        Dictionary<string, dynamic> lookingFor = GoapObjective.GetRelevantValueFromObjective("Creature.Carried", objective);
        if (lookingFor == null) return (false, objective, currentState);
        List<Dictionary<string, dynamic>> availableItems = GoapState.GetValueForKey("AvailableItems", currentState);
        var found = 
            availableItems.FirstOrDefault(item => 
                lookingFor.Keys.All(key => item.ContainsKey(key) && item[key] == lookingFor[key]));

        if (found == null)
        {
            // If relevant item isn't available, add that to the objectives
            Objective newObjective;
            var availableBone = new QueryObjective()
            {
                Target = "AvailableItems",
                QueryType = QueryType.ContainsAtLeast,
                Quantity = 1,
                PropsQuery = lookingFor
            };
            if (objective is OperatorObjective obj && obj.Operator == Operator.And)
            {
                obj.ObjectivesList.Add(availableBone);
                newObjective = obj;
            }
            else
            {
                newObjective = new OperatorObjective()
                {
                    Operator = Operator.And,
                    ObjectivesList = new List<Objective>() { objective, availableBone }
                };
            }

            return (false, newObjective, currentState);
        }
        else
        {
            // If relevant item IS available, this action will "consume" it, so remove it from the state
            var stateCopy = GoapState.CloneState(currentState);
            GoapState.SetValueForKey("AvailableItems", availableItems.Remove(found), stateCopy);
            return (true, objective, stateCopy);
        }
    }

    public override Dictionary<string, dynamic> ApplyTransform(Dictionary<string, dynamic> state)
    {
        // NOTE: WOULD IT MAKE MORE SENSE FOR THIS TO RETURN AN UPDATED OBJECTIVE?
        var stateCopy = GoapState.CloneState(state);
        List<Dictionary<string, dynamic>> currentInventory = GoapState.GetValueForKey("Creature.Carried", stateCopy);
        currentInventory.Add(new Dictionary<string, dynamic>() {{"Material", Material.Bone}});
        GoapState.SetValueForKey("Creature.Carried", currentInventory, stateCopy);
        return stateCopy;
    }
}