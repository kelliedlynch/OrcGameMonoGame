using System;
using System.Collections.Generic;
using System.Linq;
using MonoGame.Extended.Collections;
using OrcGame.Entity.Creature;
using OrcGame.GOAP.Core;

namespace OrcGame.GOAP;
public class Agent
{
	public void FindBestGoal(BaseCreature creature, Dictionary<string, dynamic> state)
	{
		GoapGoal highestPriority;
		
		foreach (var goal in creature.Goals)
		{
			if (!goal.IsValid() || !goal.TriggerConditionsMet()) continue;
			
		}
	}

	private bool IsGoalReached(GoapGoal goal, Dictionary<string, dynamic> state)
	{
		var obj = goal.GetObjective();
		return ParseObjective(obj, state);
	}

	private bool ParseObjective(Objective obj, Dictionary<string, dynamic> state)
	{
		return obj switch
		{
			QueryObjective objective => ParseQueryObjective(objective, state),
			OperatorObjective objective => ParseOperatorObjective(objective, state),
			ValueObjective objective => ParseValueObjective(objective, state),
			_ => false
		};
	}

	private bool ParseOperatorObjective(OperatorObjective obj, Dictionary<string, dynamic> state)
	{
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

	private bool ParseQueryObjective(QueryObjective obj, Dictionary<string, dynamic> state)
	{
		Bag<Dictionary<string, dynamic>> relevant =
			ExtractRelevantValueFromState(obj.Target, typeof(Bag<Dictionary<string, dynamic>>), state);

		var found = FindGivenPropertiesInDictList(obj.PropsQuery, relevant).Item1;
		if (found == null) { return false;}

		return obj.QueryType switch
		{
			QueryType.ContainsAtLeast => found.Count() >= obj.Quantity,
			QueryType.ContainsLessThan => found.Count() <= obj.Quantity,
			QueryType.ContainsExactly => found.Count() == obj.Quantity,
			QueryType.DoesNotContain => !found.Any(),
			_ => false
		};
	}

	// Returns: (items found or null, dictionary with found item removed)
	private (Bag<Dictionary<string, dynamic>>, Bag<Dictionary<string, dynamic>>) FindGivenPropertiesInDictList(Dictionary<string, dynamic> props,
		IEnumerable<Dictionary<string, dynamic>> list)
	{
		var remainingInList = new Bag<Dictionary<string, dynamic>>();
		foreach (var item in list)
		{
			remainingInList.Add(GoapSimulator.CloneState(item));
		}

		var foundItems = new Bag<Dictionary<string, dynamic>>();
		foreach (var item in remainingInList)
		{
			if (props.Keys.All(item.ContainsKey))
			{
				foundItems.Add(item);
			}
		}

		if (foundItems.Count != 0)
		{
			remainingInList.RemoveAll(foundItems);
		}

		return (foundItems, remainingInList);
	}

	private bool ParseValueObjective(ValueObjective obj, Dictionary<string, dynamic> state)
	{
		var objValue = obj.Value;
		var stateValue = state[obj.Target] is bool && (bool)state[obj.Target];
		return stateValue == objValue;
	}

	private dynamic ExtractRelevantValueFromState(string target, Type type, Dictionary<string, dynamic> state)
	{
		dynamic relevantValue = null;
		var nestedTarget = target.Split(".");
		var current = state;
		for (var i=0; i < nestedTarget.Count() - 1; i++)
		{
			if (current[nestedTarget[i]] is Dictionary<string, dynamic> cur)
			{
				current = cur;
			}
		}

		if (current[nestedTarget[^1]] is Bag<Dictionary<string, dynamic>> sub)
		{
			relevantValue = sub;
		}

		return relevantValue;
	}
}

