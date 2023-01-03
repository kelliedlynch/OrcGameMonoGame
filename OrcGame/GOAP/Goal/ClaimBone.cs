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
    public override bool IsValid(Dictionary<string, object> state)
    {
        throw new NotImplementedException();
    }

    public override bool TriggerConditionsMet(Dictionary<string, object> state)
    {
        throw new NotImplementedException();
    }

    public Dictionary<string, object> DesiredState()
    {
        return new Dictionary<string, object>() {
            { "Creature", new Dictionary<string, object>() {
                    { "Carried", new Bag<Dictionary<string, object>>() {
                        new Dictionary<string, object>(){ { "Material", Material.Bone } }
                    } },
                    { "Owned", new Bag<Dictionary<string, object>>() {
                        new Dictionary<string, object>(){ { "Material", Material.Bone } }
                    } },
                }}
        };
    } 
    public override OperatorObjective GetObjective(Dictionary<string, object> simulatedState)
    {
        if (simulatedState["Creature"] is not Dictionary<string, object> creature) { return null; }
        var ownedList = creature["Owned"] as Bag<Dictionary<string, object>>;
        var ownedQuery =
            from Dictionary<string, object> owned in ownedList
            where (Material)owned["Material"] == Material.Bone
            select owned;
        var carriedList = creature["Carried"] as Bag<Dictionary<string, object>>;
        var carriedQuery =
            from Dictionary<string, object> carried in carriedList
            where (Material)carried["Material"] == Material.Bone
            select carried;

        var carriedContainsBone = new QueryObjective()
        {
            Target = "Creature.Carried",
            QueryType = QueryType.ContainsAtLeast,
            Quantity = 1,
            PropsQuery = (IEnumerable<Dictionary<string, object>>)carriedQuery
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
            ObjectivesList = new Bag<Objective>(){ carriedContainsBone, ownedContainsBone }
        };

        return compiledObjective;
    }


    public ClaimBone(BaseCreature creature) : base(creature)
    {
    }
}


