using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using MonoGame.Extended.Collections;
using OrcGame.Entity.Creature;
using OrcGame.Entity.Item;
using OrcGame.GOAP.Core;

namespace OrcGame.GOAP.Goal;

public class ClaimBone : GoapGoal
{
    public ClaimBone(BaseCreature creature)
    {
        _creature = creature;
    }

    public override bool IsValid(Dictionary<string, object> state)
    {
        throw new NotImplementedException();
    }

    public override bool TriggerConditionsMet(Dictionary<string, object> state)
    {
        throw new NotImplementedException();
    }

    public override OperatorObjective GetObjective(Dictionary<string, object> simulatedState)
    {
        var ownedList = simulatedState["Owned"] as Bag<Dictionary<string, object>>;
        var ownedQuery =
            from Dictionary<string, object> owned in ownedList
            where (Material)owned["Material"] == Material.Bone
            select owned;
        var carriedList = simulatedState["Carried"] as Bag<Dictionary<string, object>>;
        var carriedQuery =
            from Dictionary<string, object> carried in carriedList
            where (Material)carried["Material"] == Material.Bone
            select carried;

        var carriedContainsBone = new QueryObjective()
        {
            Target = "Creature.Carried",
            QueryType = QueryType.ContainsAtLeast,
            Quantity = 1,
            PropsQuery = (IEnumerable<Dictionary<string, object>>)ownedQuery
        };
        var ownedContainsBone = new QueryObjective()
        {
            Target = "Creature.Owned",
            QueryType = QueryType.ContainsAtLeast,
            Quantity = 1,
            PropsQuery = (IEnumerable<Dictionary<string, object>>)ownedQuery
        };

        var compiledObjective = new OperatorObjective()
        {
            Operator = Operator.And,
            ObjectivesList = new List<Objective>(){ carriedContainsBone, ownedContainsBone }
        };

        return compiledObjective;
    }


}


