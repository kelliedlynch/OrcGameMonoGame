using System;
using OrcGame.Entity;

namespace OrcGame.GOAP
{
	public class Agent
	{
		public void FindBestGoal(Creature creature)
		{

		}

		public void BuildPlan(Goal goal)
		{
			var obj = goal.GetObjective();

		}

		public void ParseOperatorObjective(OperatorObjective obj)
		{

		}

		public void ParseQueryObjective(QueryObjective obj)
		{

		}

		public bool ParseBoolValueObjective(BoolValueObjective obj)
		{
			var targetString = obj.Target;


			if (obj.Operator is Operator.Equals)
			{

			}
			return true;
		}
	}
}

