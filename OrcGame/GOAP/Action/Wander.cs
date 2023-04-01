using OrcGame.GOAP.Core;
using OrcGame.OgEntity.OgCreature;

namespace OrcGame.GOAP.Action;

public class Wander : IGoapAction
{
    public bool IsValid(Objective objective)
    {
        return true;
    }

    public int GetCost()
    {
        return 1;
    }

    public bool IsRelevant(Objective objective)
    {
        return GoapObjective.ObjectiveContainsRelevantCondition("Creature.IdleState", objective);
    }

    public void ApplyTransform(Objective objective, SimulatedState state)
    {
        state.Creature.IdleState = IdleState.Playing;
    }

    public (bool, Objective) TriggerConditionsMet(Objective objective, SimulatedState state)
    {
        return (state.Creature.IdleState == IdleState.Idle, objective);
    }
}