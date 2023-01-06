using System.Collections.Generic;
using System.Linq;
using OrcGame.Entity.Creature;

namespace OrcGame.GOAP.Core;
public abstract class GoapAction 
{
    // public BaseCreature Creature { get; set; }

    public abstract bool IsRelevant(Objective objective);
    public abstract bool IsValid(Objective objective);
    public abstract Dictionary<string, dynamic> ApplyTransform(Dictionary<string, dynamic> state);
    public abstract (bool, Objective, Dictionary<string, dynamic>) TriggerConditionsMet(Objective objective, Dictionary<string, object> worldState);
    // public abstract (bool, Dictionary<string, dynamic>) ApplyTransform(Dictionary<string, object> state);
    

}


