using System.Collections.Generic;
using System.Numerics;
using OrcGame.GOAP.Action;
using OrcGame.GOAP.Core;
using OrcGame.GOAP.Goal;

namespace OrcGame.Entity.Creature;

public class Orc : BaseCreature
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