using System.Collections.Generic;
using System.Linq;
using OrcGame.Entity.Creature;
using OrcGame.GOAP.Core;

namespace OrcGame.GOAP;

public class Planner
{
    public static Path FindPathToGoal(BaseCreature creature, Objective objective, SimulatedState state)
    {
        // ENTRY POINT
        // Build step one (actually final step) of the path
        var path = new Path()
        {
            Action = null,
            Objective = objective,
            Branches = FindBranchingPaths(creature, objective, state)
        };
        return path;
    }
 
    private static HashSet<Path> FindBranchingPaths(BaseCreature creature, Objective objective, SimulatedState state)
    {
        var branches = new HashSet<Path>();        
        foreach (var action in creature.Actions)
        {
            
            // Can this action even be considered?
            var valid = action.IsValid(objective);
            if (!valid) continue;
            // Is this action relevant to the objective?
            var relevant = action.IsRelevant(objective);
            if (!relevant) continue;
            // Apply its transform and see if any objectives are met.
            var stateCopy = new SimulatedState(state);
            action.ApplyTransform(objective, stateCopy);
            var (anyObjectivesComplete, remainingObjective, stateAfterObjectivesSatisfied) = 
                GoapObjective.EvaluateObjective(objective, stateCopy, true);
            if (!anyObjectivesComplete) continue;
            // Are this action's prerequisites met? If not, add those to the remainingObjective
            // NOTE: Using stateAfterObjectivesSatisfied here because we don't want to consume any state resources
            // that will be needed to complete the objectives above. This might be a mistake; evaluate later
            var (_, objectiveWithTriggersAdded) = action.TriggerConditionsMet(remainingObjective, stateAfterObjectivesSatisfied);
            // Now we see if all objectives are satisfied after this action
            // NOTE: THIS FEELS A LITTLE SUPERFLUOUS. THERE IS PROBABLY A LESS RESOURCE-INTENSIVE CHECK WE CAN DO
            var (allObjectivesSatisfied, objectivesStillRemaining, stateThatHasProbablyNotChanged) =
                GoapObjective.EvaluateObjective(objectiveWithTriggersAdded, stateCopy);
            var thisBranch = new Path()
            {
                Action = action,
                Objective = remainingObjective,
                Branches = null
            };
            // If allObjectivesSatisfied, this action has completed the Path, and we add it with Branches == null.
            if (allObjectivesSatisfied)
            {
                branches.Add(thisBranch);
                continue;
            }
            // Otherwise, we need to find this path's branching paths
            var newBranches = FindBranchingPaths(creature, objectivesStillRemaining, stateThatHasProbablyNotChanged);
            if (!newBranches.Any()) continue;
            thisBranch.Branches = newBranches;
            branches.Add(thisBranch);
        }

        return branches;
    }

    public class Path
    {
        public IGoapAction Action;
        public Objective Objective;
        public HashSet<Path> Branches = new();
    }
}