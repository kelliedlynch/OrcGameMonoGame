using System.Collections.Generic;
using System.Linq;
using OrcGame.Entity.Creature;

namespace OrcGame.GOAP.Core;
public interface IGoapAction 
{
    // public BaseCreature Creature { get; set; }

    public abstract bool IsRelevant(Objective objective);
    public abstract bool IsValid(Objective objective);
    public abstract void ApplyTransform(Objective objective, SimulatedState state);
    public abstract (bool, Objective) TriggerConditionsMet(Objective objective, SimulatedState worldState);
    // public abstract (bool, Dictionary<string, dynamic>) ApplyTransform(Dictionary<string, object> state);
    

}


