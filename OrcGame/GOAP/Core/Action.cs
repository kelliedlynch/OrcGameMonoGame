using System.Collections.Generic;
using OrcGame.Entity.Creature;

namespace OrcGame.GOAP.Core;
public abstract class GoapAction 
{
    public BaseCreature Creature { get; set; }

    public abstract (bool, Dictionary<string, dynamic>) IsValid(Objective objective, Dictionary<string, dynamic> state);
    public abstract bool TriggerConditionsMet(Dictionary<string, object> worldState, Dictionary<string, object> goalState);
    public abstract (bool, Dictionary<string, dynamic>) ApplyTransform(Dictionary<string, object> state);
}


