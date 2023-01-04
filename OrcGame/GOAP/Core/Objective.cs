using System;
using System.Collections.Generic;
using System.Linq;
using MonoGame.Extended.Collections;

namespace OrcGame.GOAP.Core
{
    public static class GoapObjective
    {
	    // Return value: (bool didPass, Objective objectiveAfterProcessing, Dictionary<string, dynamic> stateAfterProcessing)
        public static (bool, Objective) EvaluateObjective(Objective obj, Dictionary<string, dynamic> state, bool returnTrueIfAny = false)
        {
        	return obj switch
        	{
        		QueryObjective objective => EvaluateQueryObjective(objective, state, returnTrueIfAny),
        		OperatorObjective objective => EvaluateOperatorObjective(objective, state, returnTrueIfAny),
        		ValueObjective objective => EvaluateValueObjective(objective, state),
        		_ => false
        	};
        }
    
        public static (bool, OperatorObjective) EvaluateOperatorObjective(OperatorObjective obj, Dictionary<string, dynamic> state, bool returnTrueIfAny = false)
        {
        	var allPassed = true;
            var anyPassed = false;
            var remainingObjectives = new Bag<Objective>();
        	foreach (var objective in obj.ObjectivesList)
        	{
        		var passed = EvaluateObjective(objective, state).Item1;
        		switch (obj.Operator)
        		{
        			case Operator.And:
        				if (!passed)
                        {
	                        if (!returnTrueIfAny) return (false, null);
	                        remainingObjectives.Add(objective);
	                        continue;
                        }

                        anyPassed = true;
        				break;
        			case Operator.Or:
        				if (passed)
        				{
	                        if (!returnTrueIfAny) return (true, null);
	                        anyPassed = true;
	                        continue;
                        } else if (returnTrueIfAny)
                        {
	                        remainingObjectives.Add(objective);
                        }
        				allPassed = false;
        				break;
        			case Operator.Not:
        				if (passed)
                        {
	                        if (!returnTrueIfAny) return (false, null);
	                        remainingObjectives.Add(objective);
	                        continue;
                        }

                        if (returnTrueIfAny) anyPassed = true;
        				break;
        			default:
        				return (false, null);
        		}
    
        		
        	}
            
            var returnBool = returnTrueIfAny ? anyPassed : allPassed;
            var returnObjective = new OperatorObjective()
            {
	            Conditional = obj.Conditional,
	            ObjectivesList = remainingObjectives,
	            Operator = obj.Operator,
	            Target = obj.Target
            };
            return (returnBool, returnObjective);
        }
    
        private static (bool, QueryObjective) EvaluateQueryObjective(QueryObjective obj, Dictionary<string, dynamic> state, bool returnTrueIfAny = false)
        {
	        var stateCopy = GoapState.CloneState(state);
        	Bag<Dictionary<string, dynamic>> relevant =
        		GoapState.ExtractRelevantValueFromState(obj.Target, stateCopy);
    
        	var found = FindGivenPropertiesInDictList(obj.PropsQuery, relevant).Item1;
            if (found == null) return (false, obj);
            var qtyFound = found.Count();
            var returnBool = false;

            if (obj.QueryType == QueryType.ContainsAtLeast)
	            returnBool = qtyFound >= obj.Quantity;
            if (obj.QueryType == QueryType.ContainsLessThan)
	            returnBool = qtyFound <= obj.Quantity;
            if (obj.QueryType == QueryType.ContainsExactly)
	            returnBool = qtyFound == obj.Quantity;
            if (obj.QueryType == QueryType.DoesNotContain)
	            returnBool = !found.Any();

            if (returnTrueIfAny)
            {
	            // NOTE: I AM TRYING TO MAKE THIS WORK WITH REFERENCE TYPES, BUT IT MIGHT NOT
	            
            }
	            
            return false;
        }
            
        // Returns: (items found or null, dictionary with found item removed)
        private static (Bag<Dictionary<string, dynamic>>, Bag<Dictionary<string, dynamic>>) FindGivenPropertiesInDictList(Dictionary<string, dynamic> props,
            IEnumerable<Dictionary<string, dynamic>> list)
        {
            var remainingInList = new Bag<Dictionary<string, dynamic>>();
            foreach (var item in list)
            {
	            remainingInList.Add(GoapState.CloneState(item));
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
        public static bool EvaluateValueObjective(ValueObjective obj, Dictionary<string, dynamic> state)
        {
            var objValue = obj.Value;
            var stateValue = GoapState.ExtractRelevantValueFromState(obj.Target, state);
            return obj.Conditional switch
            {
                Conditional.Equals => stateValue == objValue,
                Conditional.DoesNotEqual => stateValue != objValue,
                Conditional.IsGreaterThan => stateValue > objValue,
                Conditional.IsLessThan => stateValue < objValue,
                Conditional.IsGreaterThanOrEqualTo => stateValue >= objValue,
                Conditional.IsLessThanOrEqualTo => stateValue <= objValue,
                _ => throw new ArgumentOutOfRangeException()
            };
        }

        public static bool AreAnyObjectivesComplete(Objective objective, Dictionary<string, dynamic> state)
        {
	        
        }
    }
    
    public abstract record Objective
    {
        public string Target;
        public Conditional Conditional;
    }

    public record ValueObjective : Objective
    {
        public Type ValueType;
        public dynamic Value;
    }

    public record QueryObjective : Objective
    {
        public QueryType QueryType;
        public int Quantity;
        public Dictionary<string, object> PropsQuery;
    }

    public record OperatorObjective : Objective
    {
        public Operator Operator;
        public Bag<Objective> ObjectivesList;
    }

    public enum Operator
    {
        And,
        Or,
        Not
    }

    public enum Conditional
    {
        Equals,
        DoesNotEqual,
        IsGreaterThan,
        IsLessThan,
        IsGreaterThanOrEqualTo,
        IsLessThanOrEqualTo
    }

    public enum QueryType
    {
        ContainsAtLeast,
        ContainsLessThan,
        ContainsExactly,
        DoesNotContain
    }
}

