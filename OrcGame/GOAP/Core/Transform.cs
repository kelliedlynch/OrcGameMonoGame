using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using MonoGame.Extended.Collections;

namespace OrcGame.GOAP.Core;

public abstract class Transform
{
    public string Target;

    public abstract Dictionary<string, object> Apply(Dictionary<string, object> state);
}

public class MathTransform : Transform
{
    public MathOperator Operator;
    public float Value;
    
    public override Dictionary<string, object> Apply(Dictionary<string, object> inputState)
    {
        var state = GoapSimulator.CloneState(inputState);
        // TODO: do more type checking and error handling
        var intConvert = state[Target] is int;
        var stateVal = (float)state[Target];
        switch (Operator)
        {
            case MathOperator.Plus:
                stateVal += Value;
                break;
            case MathOperator.Minus:
                stateVal -= Value;
                break;
            case MathOperator.Multiply:
                stateVal *= Value;
                break;
            case MathOperator.Divide:
                stateVal /= Value;
                break;
            default:
                throw new ArgumentException("Invalid Operator");
        }

        state[Target] = intConvert ? (int)stateVal : stateVal;
        return state;
    }
}

public class AddPropertyTransform : Transform
{
    public Type Type;
    public object Value;
    public override Dictionary<string, object> Apply(Dictionary<string, object> state)
    {
        throw new NotImplementedException();
    }
}

public class AddListItemTransform : Transform
{
    // Item is a Dictionary of properties that represent the item to be added,
    // and does not necessarily contain all properties, only relevant ones.
    public Dictionary<string, object> AddItem;
    public int Qty;
    
    public override Dictionary<string, object> Apply(Dictionary<string, object> inputState)
    {
        var state = GoapSimulator.CloneState(inputState);
        var list = state[Target] as Bag<Dictionary<string, object>>;
        Debug.Assert(list != null, nameof(list) + " != null");
        for (var qtyAdded = 0; qtyAdded < Qty; qtyAdded++)
        {
            list.Add(AddItem);
        }
        return state;
    }
}

public class RemoveListItemTransform : Transform
{
    public Dictionary<string, object> RemoveItem;
    public int Qty;
    
    public override Dictionary<string, object> Apply(Dictionary<string, object> inputState)
    {
        var state = GoapSimulator.CloneState(inputState);
        var list = (Bag<Dictionary<string, object>>)state[Target];
        var qtyRemoved = 0;
        foreach (var item in list)
        {
            if (RemoveItem.Keys.Any() && RemoveItem.Keys.All(key => item.ContainsKey(key)))
            {
                list.Remove(item);
                qtyRemoved++;
            }

            if (qtyRemoved < Qty) continue;
            // is this necessary, or is this all reference type stuff?
            state[Target] = list;
            return state;
        }
        return inputState;
    }
}

public enum MathOperator
{
    Plus,
    Minus,
    Multiply,
    Divide
}
