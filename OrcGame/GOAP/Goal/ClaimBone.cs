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


    public override OperatorObjective GetObjective()
    {
        var desired = new Dictionary<string, dynamic>() {
            { "Creature", new Dictionary<string, dynamic>() {
                { "Carried", new Bag<Dictionary<string, dynamic>>() {
                    new Dictionary<string, dynamic>(){ { "Material", Material.Bone } }
                } },
                { "Owned", new Bag<Dictionary<string, dynamic>>() {
                    new Dictionary<string, dynamic>(){ { "Material", Material.Bone } }
                } },
            }}
        };
        if (desired["Creature"] is not Dictionary<string, dynamic> creature) return null;
        if (desired["Creature"]["Carried"] is not Bag<Dictionary<string, dynamic>>) return null;
        var carriedContainsBone = new QueryObjective()
        {
            Target = "Creature.Carried",
            QueryType = QueryType.ContainsAtLeast,
            Quantity = 1,
            PropsQuery = creature["Carried"][0] as Dictionary<string, dynamic>
        };
        var ownedContainsBone = new QueryObjective()
        {
            Target = "Creature.Owned",
            QueryType = QueryType.ContainsAtLeast,
            Quantity = 1,
            PropsQuery = creature["Owned"][0] as Dictionary<string, dynamic>
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


