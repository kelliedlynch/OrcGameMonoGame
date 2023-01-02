using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.InteropServices.JavaScript;
using OrcGame.GOAP.Core;
namespace OrcGame.GOAP.Action;

public class PickUpItem : GoapAction
{
    public override bool IsValid(Dictionary<string, object> inputState)
    {
        
        if (inputState.ContainsKey("Carried") && inputState["Carried"] is ArrayList { Count: > 0 })
        {
            var state = GoapSimulator.CloneState(inputState);
            var carried = state["Carried"] as ArrayList;
            // If any item in the list doesn't exist in an available state at validation
            // time, this will (eventually) return false. We might want to change that later, if we want
            // creature actions to be able to change item availability.
            var item = carried![^1];
            // Find a suitable item in the world
            
        }

        return true;
    }

    public override bool TriggerConditionsMet(Dictionary<string, object> state)
    {
        throw new System.NotImplementedException();
    }

    public override void GetTransform(Dictionary<string, object> state)
    {
        throw new System.NotImplementedException();
    }
}