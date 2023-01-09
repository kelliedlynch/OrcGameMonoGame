using System.Collections.Generic;
using System.Linq;
// using MonoGame.Extended.Collections;
using OrcGame.OgEntity.OgCreature;
using OrcGame.GOAP.Core;
using OrcGame.Utility;

namespace OrcGame.GOAP;

public static class Planner
{
    public static ObjectPool<SimulatedState> StatePool = new(5);
    public static ObjectPool<SimulatedCreature> CreaturePool = new(5);
    public static ObjectPool<SimulatedItemGroup> GroupPool = new(50);
    public static ObjectPool<SimulatedItem> ItemPool = new(11000);
    public static ObjectPool<Branch> BranchPool = new(20);

    // public static SimulatedState GetSimulatedState(Creature creature)
    // {
    //     SimulatedState newState;
    //     if (SimulatedStates.Any())
    //     {
    //         newState = SimulatedStates.Dequeue();
    //         newState.InitState(creature);
    //     }
    //     else
    //     {
    //         newState = new SimulatedState(creature);
    //         SimulatedStates.Enqueue(newState);
    //     }
    //
    //     return newState;
    // }
    //
    // public static void DisposeSimulatedState(SimulatedState state)
    // {
    //     state.Reset();
    //     SimulatedStates.Enqueue(state);
    // }
    
    public static Branch FindPathToGoal(Creature creature, Objective objective, SimulatedState state)
    {
        var path = BranchPool.Request();
        path.Action = null;
        path.Objective = objective;
        path.Branches = FindBranchingPaths(creature, objective, state);
        
        // var path = new Branch()
        // {
        //     Action = null,
        //     Objective = objective,
        //     Branches = FindBranchingPaths(creature, objective, state)
        // };
        return path;
    }
 
    private static HashSet<Branch> FindBranchingPaths(Creature creature, Objective objective, SimulatedState state)
    {
        var branches = new HashSet<Branch>();        
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
            var (allObjectivesSatisfied, objectivesStillRemaining, stateAfterTriggersMet) =
                GoapObjective.EvaluateObjective(objectiveWithTriggersAdded, stateCopy);

            var thisBranch = BranchPool.Request();
            thisBranch.Action = action;
            thisBranch.Objective = remainingObjective;
            thisBranch.Branches = null;
            // var thisBranch = new Branch()
            // {
            //     Action = action,
            //     Objective = remainingObjective,
            //     Branches = null
            // };
            // If allObjectivesSatisfied, this action has completed the Path, and we add it with Branches == null.
            if (allObjectivesSatisfied)
            {
                branches.Add(thisBranch);
                continue;
            }
            // Otherwise, we need to find this path's branching paths
            var newBranches = FindBranchingPaths(creature, objectivesStillRemaining, stateAfterTriggersMet);
            if (!newBranches.Any()) continue;
            thisBranch.Branches = newBranches;
            branches.Add(thisBranch);
        }

        return branches;
    }

    public class Branch : IPoolable
    {
        public IGoapAction Action;
        public Objective Objective;
        public HashSet<Branch> Branches = new();
        public void Reset()
        {
            if (Branches == null) return;
            foreach (var branch in Branches)
            {
                BranchPool.Dispose(branch);
            }
            Branches.Clear();
        }
    }
}