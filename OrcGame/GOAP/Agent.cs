using System;
using OrcGame.OgEntity.OgCreature;
using OrcGame.GOAP.Core;

namespace OrcGame.GOAP;
public class Agent
{
	public void FindBestGoal(Creature creature)
	{
		var emergenciesOnly = (creature.IdleState != IdleState.Idle && creature.CurrentPlan != null);
		GoapGoal highestPriorityGoal = null;
		var highestPriority = GoapGoal.GoalPriority.Idle;
		foreach (var goal in creature.Goals)
		{
			var thisPriority = goal.GetPriority();
			if (emergenciesOnly && thisPriority < GoapGoal.GoalPriority.Emergency) continue;
			if (!goal.IsValid() || !goal.TriggerConditionsMet()) continue;
			if (highestPriorityGoal == null || highestPriority < thisPriority)
			{
				highestPriority = thisPriority;
				highestPriorityGoal = goal;
			}
		}

		if (highestPriorityGoal == null) throw new AgentFailureException("Agent failed to find a valid goal");
		creature.CurrentGoal = highestPriorityGoal;
		var plan = Planner.FindPathToGoal(creature, highestPriorityGoal.GetObjective());
		var cheapestPlan = Planner.FindCheapestPlan(plan);
	}

	
	
	// private bool IsGoalReached(GoapGoal goal, Dictionary<string, dynamic> state)
	// {
	// 	var obj = goal.GetObjective();
	// 	return GoapObjective.EvaluateObjective(obj, state);
	// }
}

public class AgentFailureException : Exception
{
	public AgentFailureException(string message) : base(message)
	{
	}
}