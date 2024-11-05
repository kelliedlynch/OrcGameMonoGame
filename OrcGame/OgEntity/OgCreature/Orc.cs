using System.Collections.Generic;
using OrcGame.GOAP.Action;
using OrcGame.GOAP.Core;
using OrcGame.GOAP.Goal;
using OrcGame.OgEntity.OgItem;

namespace OrcGame.OgEntity.OgCreature;

public class Orc : Creature
{
    public Orc()
    {
        CreatureType = CreatureType.Humanoid;
        CreatureSubtype = CreatureSubtype.Orc;
        Goals = new HashSet<GoapGoal>()
        {
            new CarryOwnedItem(this, new Dictionary<string, dynamic>(){ { "Material", OgItem.MaterialType.Bone } })
        };
        Actions = new HashSet<IGoapAction>()
        {
            new OwnItem(),
            new PickUpItem()
        };
    }
}