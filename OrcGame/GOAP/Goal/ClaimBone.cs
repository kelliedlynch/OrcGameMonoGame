using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using MonoGame.Extended.Collections;
using OrcGame.OgEntity.OgCreature;
using OrcGame.OgEntity.OgItem;
using OrcGame.GOAP.Core;

namespace OrcGame.GOAP.Goal;

public class ClaimBone : GoapGoal
{
    // private readonly Dictionary<string, dynamic> _bone = new() { { "Material", Material.Bone } };
    private static readonly Dictionary<string, dynamic> BoneProps = new() { { "Material", MaterialType.Bone } };
    // private static readonly SimulatedItem Bone = new(BoneProps);

    public override bool IsValid()
    {
        var ownsBone = Creature.Owned.Any(item => item.Material == MaterialType.Bone);

        if (ownsBone == false) return true;

        var hasBone = Creature.Carried.Any(item => item.Material == MaterialType.Bone);

        return !hasBone;
    }

    public override bool TriggerConditionsMet()
    {
        var itemManager = ItemManager.GetItemManager();
        var availableBone = itemManager.FindNearestItemWithProps(BoneProps);
        return (availableBone != null);
    }
    
    public override OperatorObjective GetObjective()
    {
        var carriedContainsBone = new QueryObjective()
        {
            Target = "Creature.Carried",
            QueryType = QueryType.ContainsAtLeast,
            Quantity = 1,
            PropsQuery = BoneProps
        };
        var ownedContainsBone = new QueryObjective()
        {
            Target = "Creature.Owned",
            QueryType = QueryType.ContainsAtLeast,
            Quantity = 1,
            PropsQuery = BoneProps
        };
    
        var compiledObjective = new OperatorObjective()
        {
            Operator = Operator.And,
            ObjectivesList = new HashSet<Objective>(){ carriedContainsBone, ownedContainsBone }
        };
    
        return compiledObjective;
    }

    public override GoalPriority GetPriority()
    {
        return GoalPriority.Want;
    }
    
    public ClaimBone(Creature creature) : base(creature)
    {
    }
}


