using System;
using System.Collections.Generic;
using OrcGame.OgEntity.OgCreature;
using OrcGame.GOAP.Core;

namespace OrcGame.GOAP;
public class Agent
{
	public void FindBestGoal(Creature creature, Dictionary<string, dynamic> state)
	{
		GoapGoal highestPriority = null;
		
		foreach (var goal in creature.Goals)
		{
			if (!goal.IsValid() || !goal.TriggerConditionsMet()) continue;
			// ReSharper disable once ConditionIsAlwaysTrueOrFalse
			if (highestPriority == null || highestPriority.GetPriority() < goal.GetPriority())
			{
				highestPriority = goal;
			}
		}
	}

	// private bool IsGoalReached(GoapGoal goal, Dictionary<string, dynamic> state)
	// {
	// 	var obj = goal.GetObjective();
	// 	return GoapObjective.EvaluateObjective(obj, state);
	// }
}

