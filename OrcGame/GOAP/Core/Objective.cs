using System;
using System.Collections.Generic;
using System.Linq;
using MonoGame.Extended.Collections;
using OrcGame.GOAP.Core;

namespace OrcGame.GOAP.Core
{
    public static class GoapObjective
    {
	    // Return value: (bool didPass, Objective objectiveAfterProcessing, SimulatedState stateAfterProcessing)
        public static (bool, Objective, SimulatedState) EvaluateObjective(Objective obj, SimulatedState state, bool returnTrueIfAny = false)
        {
	        switch (obj)
	        {
		        case QueryObjective objective:
			        return EvaluateQueryObjective(objective, state, returnTrueIfAny);
		        case OperatorObjective objective:
			        var (opPassed, remainingObjective, alteredState) =
				        EvaluateOperatorObjective(objective, state, returnTrueIfAny);
			        return (opPassed, !returnTrueIfAny && opPassed ? null : remainingObjective, returnTrueIfAny ? alteredState : state);
		        case ValueObjective objective:
			        var valPassed = EvaluateValueObjective(objective, state);
			        return (valPassed, returnTrueIfAny && !valPassed ? objective : null, state);
		        default:
			        return (false, obj, state);
	        }
        }
    
        public static (bool, OperatorObjective, SimulatedState) EvaluateOperatorObjective(OperatorObjective obj, SimulatedState state, bool returnTrueIfAny = false)
        {
        	var allPassed = true;
            var anyPassed = false;
            var returnObjectives = new HashSet<Objective>();
            var returnState = new SimulatedState(state);
        	foreach (var objective in obj.ObjectivesList)
        	{
        		var (passed, returnObj, alteredState) = EvaluateObjective(objective, state, returnTrueIfAny);
        		switch (obj.Operator)
        		{
        			case Operator.And:
        				if (!passed)
                        {
	                        if (!returnTrueIfAny) return (false, obj, state);
	                        if (returnObj != null) returnObjectives.Add(returnObj);
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
	                        if (returnObj != null) returnObjectives.Add(returnObj);
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
            var returnObjective = obj with { ObjectivesList = returnObjectives };
            return (returnBool, returnObjective, returnState);
        }
    
        private static (bool, QueryObjective, SimulatedState) EvaluateQueryObjective(
	        QueryObjective obj, SimulatedState state, bool returnTrueIfAny = false)
        {
	        var stateCopy = new SimulatedState(state);
        	dynamic relevant = state.GetValueForTarget(obj.Target);
            switch (relevant)
            {
	            case HashSet<SimulatedItem> items:
		            relevant = items;
		            break;
	            case HashSet<SimulatedItemGroup> groups:
		            relevant = groups;
		            break;
	            case HashSet<SimulatedCreature> creatures:
		            relevant = creatures;
		            break;
	            default:
		            throw new ArgumentException("SimulatedState value is not HashSet of ISimulated");
            }
            var foundItems = FindMembersWithProperties(obj.PropsQuery, relevant, obj.Quantity);
            var qtyFound = 0;

            foreach (var simulated in foundItems)
            {
	            if (simulated is not SimulatedItemGroup item)
	            {
		            qtyFound = foundItems.Count;
		            break;
	            }

	            qtyFound += item.Quantity;
            }

	        var returnBool = obj.QueryType switch
            {
	            QueryType.ContainsAtLeast => qtyFound >= obj.Quantity,
	            QueryType.ContainsLessThan => qtyFound <= obj.Quantity,
	            QueryType.ContainsExactly => qtyFound == obj.Quantity,
	            QueryType.DoesNotContain => qtyFound == 0,
	            _ => throw new ArgumentOutOfRangeException()
            };

            QueryObjective returnObj = null; 
            if (returnTrueIfAny)
            {
	            var qtyRemaining = obj.Quantity - qtyFound;
	            if (qtyRemaining > 0)
	            {
		            returnObj = obj with { Quantity = qtyRemaining };
		            
	            }
            }
	            
            return (returnBool, returnObj, stateCopy);
        }
            
        // Returns: (items found, remaining items with found items removed)
        private static HashSet<Simulated> FindMembersWithProperties(
	        Dictionary<string, dynamic> props, dynamic list, int qtySeeking = 1)
        {
            var foundItems = new HashSet<Simulated>();
            
            foreach (Simulated item in list)
            {
	            var iType = item.GetType();
	            
	            if (props.Keys.All(key => iType.GetProperties().Any(prop => prop.Name == key )) &&
	                props.Keys.All(key => (dynamic)iType.GetProperty(key)!.GetValue(item) == props[key]))
	            {
		            if (item is SimulatedItemGroup group)
		            {
			            var popped = group.PopGroupFromGroup(qtySeeking);
			            qtySeeking -= popped.Quantity;
			            foundItems.Add(popped);
		            }
		            else
		            {
			            foundItems.Add(item);
			            qtySeeking -= 1;
		            }

		            if (qtySeeking <= 0) break;
	            }
            }

            return foundItems;
        }
        public static bool EvaluateValueObjective(ValueObjective obj, SimulatedState state)
        {
            var objValue = obj.Value;
            object stateValue = null;
            try
            {
	            stateValue = state.GetValueForTarget(obj.Target);
            }
            catch (KeyNotFoundException k)
            {
	            return false;
            }

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
    }

    public record ValueObjective : Objective
    {
        public Type ValueType;
        public Conditional Conditional;
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
        public HashSet<Objective> ObjectivesList;
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

