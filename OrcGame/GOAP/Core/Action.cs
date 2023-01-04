using System.Collections.Generic;
using System.Linq;
using OrcGame.Entity.Creature;

namespace OrcGame.GOAP.Core;
public abstract class GoapAction 
{
    public BaseCreature Creature { get; set; }

    public abstract bool IsValid(Objective objective);
    public abstract (bool, Dictionary<string, dynamic>) ApplyTransformIfRelevant(Objective objective, Dictionary<string, dynamic> state);
    public abstract bool TriggerConditionsMet(Dictionary<string, object> worldState, Dictionary<string, object> goalState);
    public abstract (bool, Dictionary<string, dynamic>) ApplyTransform(Dictionary<string, object> state);
    
    protected bool ObjectiveContainsRelevantCondition(string target, Objective objective)
    {
        if (objective is OperatorObjective opObjective)
        {
            return opObjective.ObjectivesList.Select(
                subObjective => ObjectiveContainsRelevantCondition(target, subObjective)).FirstOrDefault();
        }
        else
        {
            return objective.Target == target;
        }
    }
}


