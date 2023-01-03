﻿using System.Collections.Generic;
using System.Linq;
using OrcGame.Entity.Creature;
using OrcGame.GOAP.Core;

namespace OrcGame.GOAP;
public class Agent
{
	private Dictionary<string, object> _simulatedState;
	public void FindBestGoal(BaseCreature creature)
	{

	}

	public void BuildPlan(GoapGoal goal)
	{
		_simulatedState = GoapSimulator.SimulateEntity(goal.Creature);
		
	}

	public bool IsGoalReached(GoapGoal goal, Dictionary<string, object> state = null)
	{
		state ??= _simulatedState;
		var obj = goal.GetObjective(state);
		return ParseObjective(obj, state);
	}

	private bool ParseObjective(Objective obj, Dictionary<string, object> state = null)
	{
		state ??= _simulatedState;
		return obj switch
		{
			QueryObjective objective => ParseQueryObjective(objective, state),
			OperatorObjective objective => ParseOperatorObjective(objective, state),
			BoolValueObjective objective => ParseBoolValueObjective(objective, state),
			_ => false
		};
	}

	private bool ParseOperatorObjective(OperatorObjective obj, Dictionary<string, object> state = null)
	{
		state ??= _simulatedState;
		var allPassed = true;
		foreach (var objective in obj.ObjectivesList)
		{
			var passed = ParseObjective(objective, state);
			switch (obj.Operator)
			{
				case Operator.And:
					if (!passed)
					{
						return false;
					}
					break;
				case Operator.Or:
					if (passed)
					{
						return true;
					}
					allPassed = false;
					break;
				case Operator.Not:
					if (passed)
					{
						return false;
					}
					break;
				default:
					return false;
			}

			
		}
		return allPassed;
	}

	private bool ParseQueryObjective(QueryObjective obj, Dictionary<string, object> state = null)
	{
		state ??= _simulatedState;
		var found = obj.PropsQuery;
		
		switch (obj.QueryType)
		{
			case QueryType.ContainsAtLeast:
				return found.Count() >= obj.Quantity;
			case QueryType.ContainsLessThan:
				return found.Count() <= obj.Quantity;
			case QueryType.ContainsExactly:
				return found.Count() == obj.Quantity;
			case QueryType.DoesNotContain:
				return !found.Any();
			default:
				return false;
		}
	}

	private bool ParseBoolValueObjective(BoolValueObjective obj, Dictionary<string, object> state = null)
	{
		state ??= _simulatedState;
		var objValue = obj.Value;
		var stateValue = state[obj.Target] is bool && (bool)state[obj.Target];
		return stateValue == objValue;
	}
}

