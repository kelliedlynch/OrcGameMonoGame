using System;
using System.Collections.Generic;
using MonoGame.Extended.Collections;

namespace OrcGame.GOAP.Core
{
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

