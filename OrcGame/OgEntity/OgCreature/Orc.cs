using System.Collections.Generic;
using OrcGame.GOAP.Action;
using OrcGame.GOAP.Core;
using OrcGame.GOAP.Goal;

namespace OrcGame.OgEntity.OgCreature;

public class Orc : Creature
{
    public Orc()
    {
        CreatureType = CreatureType.Humanoid;
        CreatureSubtype = CreatureSubtype.Orc;
        Goals = new HashSet<GoapGoal>()
        {
            new ClaimBone(this)
        };
        Actions = new HashSet<IGoapAction>()
        {
            new PickUpItem()
        };
    }
}