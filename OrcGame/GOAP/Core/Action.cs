using System.Collections.Generic;
using OrcGame.Entity.Creature;

namespace OrcGame.GOAP.Core;
public abstract class GoapAction 
{
    public BaseCreature Creature { get; set; }
    public abstract bool IsValid(Dictionary<string, object> desiredState);
    public abstract bool TriggerConditionsMet(Dictionary<string, object> worldState, Dictionary<string, object> goalState);
    public abstract void GetTransform(Dictionary<string, object> state);
}


