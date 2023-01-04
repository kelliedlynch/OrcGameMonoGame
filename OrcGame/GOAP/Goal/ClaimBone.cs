using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using MonoGame.Extended.Collections;
using OrcGame.Entity.Creature;
using OrcGame.Entity.Item;
using OrcGame.GOAP.Core;

namespace OrcGame.GOAP.Goal;

public class ClaimBone : GoapGoal
{
    private readonly Dictionary<string, dynamic> _bone = new() { { "Material", Material.Bone } };

    public override bool IsValid()
    {
        var ownsBone = Creature.Owned.Any(item => item.Material == Material.Bone);

        if (ownsBone == false) return true;

        var hasBone = Creature.Carried.Any(item => item.Material == Material.Bone);

        return !hasBone;
    }
    // public override bool IsValid(Dictionary<string, dynamic> state)
    // {
    //     if (!state.ContainsKey("Creature") 
    //         || !state["Creature"].ContainsKey("Owned")
    //         || !state["Creature"].ContainsKey("Carried")) return false;
    //     var ownsBone = false;
    //     var hasBone = false;
    //     foreach (var item in (Bag<Dictionary<string, dynamic>>)state["Creature"]["Owned"])
    //     {
    //         ownsBone = item.ContainsKey("Material") && item["Material"] == Material.Bone;
    //         if (ownsBone == true) break;
    //     }
    //     foreach (var item in (Bag<Dictionary<string, dynamic>>)state["Creature"]["Carried"])
    //     {
    //         hasBone = item.ContainsKey("Material") && item["Material"] == Material.Bone;
    //         if (hasBone == true) break;
    //     }
    //
    //     return !(ownsBone && hasBone);
    // }

    public override bool TriggerConditionsMet()
    {
        var itemManager = ItemManager.GetItemManager();
        var availableBone = itemManager.FindNearestItemWithProps(_bone);
        return (availableBone != null);
    }
    
    public override OperatorObjective GetObjective()
    {
        var carriedContainsBone = new QueryObjective()
        {
            Target = "Creature.Carried",
            QueryType = QueryType.ContainsAtLeast,
            Quantity = 1,
            PropsQuery = _bone
        };
        var ownedContainsBone = new QueryObjective()
        {
            Target = "Creature.Owned",
            QueryType = QueryType.ContainsAtLeast,
            Quantity = 1,
            PropsQuery = _bone
        };
    
        var compiledObjective = new OperatorObjective()
        {
            Operator = Operator.And,
            ObjectivesList = new Bag<Objective>(){ carriedContainsBone, ownedContainsBone }
        };
    
        return compiledObjective;
    }

    public override GoalPriority GetPriority()
    {
        return GoalPriority.Want;
    }
    
    public ClaimBone(BaseCreature creature) : base(creature)
    {
    }
}


