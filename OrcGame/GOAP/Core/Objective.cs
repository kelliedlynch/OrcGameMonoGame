using System;
using System.Collections.Generic;
using System.Linq;
using MonoGame.Extended.Collections;

namespace OrcGame.GOAP.Core
{
    public static class GoapObjective
    {
	    // Return value: (bool didPass, Objective objectiveAfterProcessing, Dictionary<string, dynamic> stateAfterProcessing)
        public static (bool, Objective, Dictionary<string, dynamic>) EvaluateObjective(Objective obj, Dictionary<string, dynamic> state, bool returnTrueIfAny = false)
        {
	        switch (obj)
	        {
		        case QueryObjective objective:
			        return EvaluateQueryObjective(objective, state, returnTrueIfAny);
		        case OperatorObjective objective:
			        var (opPassed, remainingObjective, alteredState) =
				        EvaluateOperatorObjective(objective, state, returnTrueIfAny);
			        return (opPassed, returnTrueIfAny && !opPassed ? remainingObjective : null, returnTrueIfAny ? alteredState : state);
		        case ValueObjective objective:
			        var valPassed = EvaluateValueObjective(objective, state);
			        return (valPassed, returnTrueIfAny && !valPassed ? objective : null, state);
		        default:
			        return (false, obj, state);
	        }
        }
    
        public static (bool, OperatorObjective, Dictionary<string, dynamic>) EvaluateOperatorObjective(OperatorObjective obj, Dictionary<string, dynamic> state, bool returnTrueIfAny = false)
        {
        	var allPassed = true;
            var anyPassed = false;
            var returnObjectives = new Bag<Objective>();
            var returnState = returnTrueIfAny ? GoapState.CloneState(state) : state;
        	foreach (var objective in obj.ObjectivesList)
        	{
        		var (passed, returnObj, alteredState) = EvaluateObjective(objective, state);
        		switch (obj.Operator)
        		{
        			case Operator.And:
        				if (!passed)
                        {
	                        if (!returnTrueIfAny) return (false, obj, state);
	                        returnObjectives.Add(returnObj);
	                        returnState = alteredState;
	                        continue;
                        }

                        anyPassed = true;
        				break;
        			case Operator.Or:
        				if (passed)
        				{
	                        if (!returnTrueIfAny) return (true, obj, state);
	                        anyPassed = true;
	                        continue;
                        } else if (returnTrueIfAny)
                        {
	                        returnObjectives.Add(returnObj);
	                        returnState = alteredState;
                        }
        				allPassed = false;
        				break;
        			case Operator.Not:
        				if (passed)
                        {
	                        if (!returnTrueIfAny) return (false, obj, state);
	                        returnObjectives.Add(returnObj);
	                        continue;
                        }

                        if (returnTrueIfAny) anyPassed = true;
                        break;
        			default:
        				return (false, obj, state);
        		}
    
        		
        	}
            
            var returnBool = returnTrueIfAny ? anyPassed : allPassed;
            var returnObjective = new OperatorObjective()
            {
	            Conditional = obj.Conditional,
	            ObjectivesList = returnObjectives,
	            Operator = obj.Operator,
	            Target = obj.Target
            };
            return (returnBool, returnObjective, returnState);
        }
    
        private static (bool, QueryObjective, Dictionary<string, dynamic>) EvaluateQueryObjective(
	        QueryObjective obj, Dictionary<string, dynamic> state, bool returnTrueIfAny = false)
        {
	        var stateCopy = GoapState.CloneState(state);
        	Bag<Dictionary<string, dynamic>> relevant = GoapState.GetValueForKey(obj.Target, stateCopy);

            var (foundItems, remainingItems) = FindGivenPropertiesInDictList(obj.PropsQuery, obj.Quantity, relevant);
            var qtyFound = foundItems.Count;
            // if (qtyFound == 0) return (false, obj, state);
            var returnBool = false;

            if (obj.QueryType == QueryType.ContainsAtLeast)
	            returnBool = qtyFound >= obj.Quantity;
            if (obj.QueryType == QueryType.ContainsLessThan)
	            returnBool = qtyFound <= obj.Quantity;
            if (obj.QueryType == QueryType.ContainsExactly)
	            returnBool = qtyFound == obj.Quantity;
            if (obj.QueryType == QueryType.DoesNotContain)
	            returnBool = qtyFound == 0;

            QueryObjective returnObj = null; 
            if (returnTrueIfAny)
            {
	            var qtyRemaining = obj.Quantity - qtyFound;
	            if (qtyRemaining > 0)
	            {
		            returnObj = new QueryObjective()
		            {
			            Conditional = obj.Conditional,
			            PropsQuery = obj.PropsQuery,
			            Quantity = qtyRemaining,
			            QueryType = obj.QueryType,
			            Target = obj.Target
		            };
		            
	            }
	            GoapState.SetValueForKey(obj.Target, remainingItems, stateCopy);
            }
	            
            return (returnBool, returnObj, stateCopy);
        }
            
        // Returns: (items found, remaining items with found items removed)
        private static (Bag<Dictionary<string, dynamic>>, Bag<Dictionary<string, dynamic>>) FindGivenPropertiesInDictList(
	        Dictionary<string, dynamic> props, int qtySeeking, Bag<Dictionary<string, dynamic>> list)
        {
            var remainingInList = new Bag<Dictionary<string, dynamic>>();
            // foreach (var item in list)
            // {
	           //  remainingInList.Add(GoapState.CloneState(item));
            // }

            var foundItems = new Bag<Dictionary<string, dynamic>>();
            foreach (var item in list)
            {
	            if (qtySeeking > 0 && props.Keys.All(item.ContainsKey))
	            {
		            foundItems.Add(GoapState.CloneState(item));
		            qtySeeking -= 1;
	            }
	            else
	            {
		            remainingInList.Add(GoapState.CloneState(item));
	            }
            }

            return (foundItems, remainingInList);
        }
        public static bool EvaluateValueObjective(ValueObjective obj, Dictionary<string, dynamic> state)
        {
            var objValue = obj.Value;
            var stateValue = GoapState.GetValueForKey(obj.Target, state);
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
        
        public static bool ObjectiveContainsRelevantCondition(string target, Objective objective)
        {
	        if (objective is OperatorObjective opObjective)
	        {
		        return opObjective.ObjectivesList.Select(
			        subObjective => ObjectiveContainsRelevantCondition(target, subObjective)).FirstOrDefault();
	        }
	        else
	        {
		        return objective.Target == target;
	        }
        }
    
        public static dynamic GetRelevantValueFromObjective(string target, Objective objective)
        {
	        if (objective is OperatorObjective opObjective)
	        {
		        return opObjective.ObjectivesList.Select(
			        subObjective => GetRelevantValueFromObjective(target, subObjective)).FirstOrDefault();
	        }
	        else if (objective is ValueObjective valObjective)
	        {
		        return valObjective.Target == target ? valObjective.Value : null;
	        }
	        else if (objective is QueryObjective quObjective)
	        {
		        return quObjective.Target == target ? quObjective.PropsQuery : null;
	        }

	        return null;
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

