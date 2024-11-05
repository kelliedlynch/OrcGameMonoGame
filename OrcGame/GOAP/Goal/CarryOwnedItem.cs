using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using OrcGame.OgEntity.OgCreature;
using OrcGame.OgEntity.OgItem;
using OrcGame.GOAP.Core;

namespace OrcGame.GOAP.Goal;

public class CarryOwnedItem : GoapGoal
{
    private readonly ItemManager _itemManager = ItemManager.GetItemManager();
    // private readonly Dictionary<string, dynamic> _bone = new() { { "Material", Material.Bone } };
    private Dictionary<string, dynamic> _itemProps;
    // private static readonly SimulatedItem Bone = new(BoneProps);
    
    public override bool IsValid()
    {
        // var ownsBone = false;
        
        var ownsBone = Creature.Owned.Any(item => item.Material == MaterialType.Bone);

        // foreach (var ownedItem in Creature.Owned)
        // {
        //     foreach (KeyValuePair<string, dynamic> prop in ItemProps)
        //     {
        //         ownedItem.GetType().GetProperty(prop.Key)?.GetValue(ownedItem);
        //     }
        // }

        if (ownsBone == false) return true;

        var hasBone = Creature.Carried.Any(item => item.Material == MaterialType.Bone);

        return !hasBone;
    }

    public override bool TriggerConditionsMet()
    {
        var availableBone = _itemManager.FindNearestItemWithProps(_itemProps);
        return (availableBone != null);
    }
    
    public override OperatorObjective GetObjective()
    {
        var carriedContainsBone = new QueryObjective()
        {
            Target = "Creature.Carried",
            QueryType = QueryType.ContainsAtLeast,
            Quantity = 1,
            PropsQuery = _itemProps
        };
        var ownedContainsBone = new QueryObjective()
        {
            Target = "Creature.Owned",
            QueryType = QueryType.ContainsAtLeast,
            Quantity = 1,
            PropsQuery = _itemProps
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
    
    public CarryOwnedItem(Creature creature, Dictionary<string, dynamic> itemProperties) : base(creature)
    {
        _itemProps = itemProperties;
    }

}


