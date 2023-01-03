using System.Collections.Generic;
using System.Linq;
using MonoGame.Extended.Collections;
using OrcGame.Entity.Item;
using OrcGame.GOAP.Core;
namespace OrcGame.GOAP.Action;

public class PickUpItem : GoapAction
{
    private BaseItem _item;

    private Dictionary<string, object> FindRelevantConditionInObjective(Objective objective)
    {
        if (objective is QueryObjective obj1)
        {
            return obj1.PropsQuery.FirstOrDefault(item => item.Keys.Any(key => key == "Creature.Carried"));
        } else if (objective is OperatorObjective { Operator: not Operator.Not } obj2)
        {
            return obj2.ObjectivesList.Select(FindRelevantConditionInObjective).FirstOrDefault();
        }
        return null;
    }
    
    // IsValid is run only at the beginning of action planning, and only tests against the current state
    // of the world. TriggerConditionsMet is where we determine if the action is still doable after a 
    // change to the simulated state.
    // public override bool IsValid(Objective goal)
    // {
    //     var lookingFor = FindRelevantConditionInObjective(goal);
    //        
    //     // Find a suitable item in the world
    //     var itemManager = ItemManager.GetItemManager();
    //     _item = itemManager.FindNearestItemWithProps(lookingFor);
    //
    //     return _item is not null;
    // }

    public override bool IsValid(Dictionary<string, object> desiredState)
    {
        
        return true;
    }
    public override bool TriggerConditionsMet(Dictionary<string, object> worldState, Dictionary<string, object> goalState)
    {
        throw new System.NotImplementedException();
    }

    public override void GetTransform(Dictionary<string, object> state)
    {
        throw new System.NotImplementedException();
    }
}