using System;
using System.Collections.Generic;
using System.Linq;
using MonoGame.Extended.Collections;
using OrcGame.Entity.Creature;
using OrcGame.GOAP.Core;

namespace OrcGame.GOAP;

public class Planner
{
    public Bag<Path> FindBranchingPaths(Path prevPath, Dictionary<string, dynamic> state)
    {
        var branchingPaths = new Bag<Path>();
        if (prevPath.Objective is OperatorObjective opObj)
        {
            switch (opObj.Operator)
            {
                case Operator.And:
                    foreach (var andObjective in opObj.ObjectivesList)
                    {
                        var branch = FindBranchingPaths(andObjective, state);
                        if (branch == null) return null;
                        branchingPaths.Add(branch);
                    }
                    break;
                case Operator.Or:
                    foreach (var orObjective in opObj.ObjectivesList)
                    {
                        var branch = FindBranchingPaths(orObjective, state);
                        if (branch == null) continue;
                        path.Branches.Add(branch);
                        break;
                    }
                    break;
                case Operator.Not:
                    foreach (var notObjective in opObj.ObjectivesList)
                    {
                        var branch = FindBranchingPaths(notObjective, state);
                        if (branch != null) return null;
                    }
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            
        }
        else if (prevPath.Objective is ValueObjective valueObjective)
        {
            var passed = GoapObjective.EvaluateValueObjective(valueObjective, state);
            
        }
    }

    private void BuildPathForValueObjective(BaseCreature creature, ValueObjective objective, Dictionary<string, dynamic> state)
    {
        foreach (var action in creature.Actions)
        {
            var valid = action.IsValid(objective);
            if (!valid) continue;
            var relevant = action.ApplyTransformIfRelevant(objective, state);
            if (!relevant.Item1) continue;
        }
    }

    public class Path
    {
        public GoapAction Action;
        public Objective Objective;
        public Bag<Path> Branches = new();
    }
}