using System.Collections.Generic;
using System.Linq;
using MonoGame.Extended.Collections;
using OrcGame.Entity.Creature;
using OrcGame.GOAP.Core;

namespace OrcGame.GOAP;

public class Planner
{
    public Path FindPathToGoal(BaseCreature creature, Objective objective, Dictionary<string, dynamic> state)
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
    // public Path BuildPathForObjective(BaseCreature creature, Objective objective, Dictionary<string, dynamic> state)
    // {
    //     var branchingPaths = new Bag<Path>();
    //     if (objective is OperatorObjective opObj)
    //     {
    //         switch (opObj.Operator)
    //         {
    //             case Operator.And:
    //                 var branchesA = new Bag<Path>();
    //                 var stateCopyA = GoapState.CloneState(state);
    //                 foreach (var andObjective in opObj.ObjectivesList)
    //                 {
    //                     // Check if the objective is already complete
    //                     var (passed, _, stateAfterObjectivePassed) = GoapObjective.EvaluateObjective(andObjective, state);
    //                     // If complete, set the transformed state for further objectives
    //                     if (passed)
    //                     {
    //                         stateCopyA = stateAfterObjectivePassed;
    //                         continue;
    //                     }
    //                     // If not, find a path that completes it.
    //                     stateAfterObjectivePassed = GoapState.CloneState(stateCopyA);
    //                     var branch = BuildPathForObjective(creature, andObjective, stateAfterObjectivePassed);
    //                     // If no path exists, then completing this objective first doesn't work
    //                     if (branch == null) continue;
    //                     // Otherwise, completing this objective first is valid, and we should try to complete the
    //                     // remaining objectives
    //                     var remainingObjectives = new Bag<Objective>();
    //                     foreach (var o in opObj.ObjectivesList)
    //                     {
    //                         if (o != andObjective) remainingObjectives.Add(o);
    //                     }
    //
    //                     var newObjective = remainingObjectives.FirstOrDefault();
    //                     if (remainingObjectives.Count() > 1)
    //                     {
    //                         newObjective = new OperatorObjective()
    //                         {
    //                             Target = opObj.Target,
    //                             Operator = opObj.Operator,
    //                             ObjectivesList = remainingObjectives
    //                         };
    //                     }
    //
    //                     // var nextPath = BuildPathForObjective(creature, newObjective, stateAfterObjectivePassed);
    //                     branchesA.Add(BuildPathForObjective(creature, newObjective, stateAfterObjectivePassed));
    //                 }
    //                 // If we made it all the way through this loop without returning, all objectives have passed,
    //                 // and we need to return the path that gets us there.
    //                 return new Path()
    //                 {
    //                     Action = null,
    //                     Objective = objective,
    //                     Branches = branchesA
    //                 };
    //             case Operator.Or:
    //                 var branchesO = new Bag<Path>();
    //                 // var stateCopyO = GoapState.CloneState(state);
    //                 foreach (var orObjective in opObj.ObjectivesList)
    //                 {
    //                     // Check if the objective is already complete
    //                     var (passed, _, _) = GoapObjective.EvaluateObjective(orObjective, state);
    //                     // If complete, objective is satisfied, and we should return this finished Path
    //                     if (passed) return new Path();
    //                     // If not complete, find a path that completes it.
    //                     var stateCopyO = GoapState.CloneState(state);
    //                     var branch = BuildPathForObjective(creature, orObjective, stateCopyO);
    //                     // If no path exists, then completing this objective doesn't work
    //                     if (branch == null) continue;
    //                     // Otherwise, completing this objective is a valid option, and the path to do so
    //                     // should be added to branches
    //                     branchesO.Add(branch);
    //                 }
    //                 // If we made it here and there are no branches available, this objective fails
    //                 if (!branchesO.Any()) return null;
    //                 // Otherwise, return a path containing our options for completing this objective
    //                 return new Path()
    //                 {
    //                     Action = null,
    //                     Objective = opObj,
    //                     Branches = branchesO
    //                 };
    //                 break;
    //             case Operator.Not:
    //                 // NOTE: DO WE EVEN NEED NOT OPERATORS? WE HAVE NEGATIVE CONDITIONALS, SO I THINK WE CAN REPRESENT
    //                 // ANYTHING WE NEED WITH THOSE. TRY TO GET BY WITHOUT USING THEM, AND EVALUATE LATER, BECAUSE THEY
    //                 // ARE VERY CONFUSING
    //                 throw new NotImplementedException(
    //                     "Can you do this without a NOT operator? I don't want to code it.");
    //             default:
    //                 throw new ArgumentOutOfRangeException();
    //         }
    //         
    //     }
    //     else if (objective is ValueObjective valueObjective)
    //     {
    //         var passed = GoapObjective.EvaluateValueObjective(valueObjective, state);
    //         if (passed) return new Path();
    //         var branches = FindBranchingPaths(creature, valueObjective, state);
    //     }
    // }

    private Bag<Path> FindBranchingPaths(BaseCreature creature, Objective objective, Dictionary<string, dynamic> stateBeforeActions)
    {
        var branches = new Bag<Path>();        
        foreach (var action in creature.Actions)
        {
            // Can this action even be considered?
            var valid = action.IsValid(objective);
            if (!valid) continue;
            // Is this action relevant to the objective? Apply its transform and see if any objectives are met.
            var (isRelevant, stateAfterAction) = action.ApplyTransform(objective, stateBeforeActions);
            if (!isRelevant) continue;
            var (anyObjectivesComplete, remainingObjective, stateAfterObjectivesSatisfied) = 
                GoapObjective.EvaluateObjective(objective, stateAfterAction, true);
            if (!anyObjectivesComplete) continue;
            // Are this action's prerequisites met? If not, add those to the remainingObjective
            // NOTE: Using stateAfterObjectivesSatisfied here because we don't want to consume any state resources
            // that will be needed to complete the objectives above. This might be a mistake; evaluate later
            var (triggersMet, objectiveWithTriggersAdded) = action.TriggerConditionsMet(remainingObjective, stateAfterObjectivesSatisfied);
            if (!triggersMet) remainingObjective = objectiveWithTriggersAdded;
            // If remainingObjective is null here, this action has completed the Path.
            var thisBranch = new Path()
            {
                Action = action,
                Objective = remainingObjective,
                Branches = null
            };
            if (remainingObjective == null)
            {
                branches.Add(thisBranch);
                continue;
            }
            // Otherwise, we need to find this path's branching paths
            thisBranch.Branches = FindBranchingPaths(creature, remainingObjective, stateAfterObjectivesSatisfied);
            branches.Add(thisBranch);
            
            // // Is there a path to this new objective?
            // var pathToNewObjective = BuildPathForObjective(creature, remainingObjective, stateAfterObjectivesSatisfied);
            // if (pathToNewObjective == null) continue;
            // // There is a path to the new objective. We should add this to the branches list
            // branches.Add(pathToNewObjective);
        }

        return branches.Any() ? branches : null;
    }

    public class Path
    {
        public GoapAction Action;
        public Objective Objective;
        public Bag<Path> Branches = new();
    }
}