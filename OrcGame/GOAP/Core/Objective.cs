using System.Collections.Generic;

namespace OrcGame.GOAP.Core
{
    public abstract record Objective
    {
        public string Target;
        public Conditional Conditional;
    }

    public abstract record StringValueObjective : Objective
    {
        public string Value;
    }

    public abstract record IntValueObjective : Objective
    {
        public int Value;
    }

    public abstract record FloatValueObjective : Objective
    {
        public float Value;
    }

    public abstract record BoolValueObjective : Objective
    {
        public bool Value;
    }

    public record QueryObjective : Objective
    {
        public QueryType QueryType;
        public int Quantity;
        public IEnumerable<Dictionary<string, object>> PropsQuery;
    }

    public record OperatorObjective : Objective
    {
        public Operator Operator;
        public List<Objective> ObjectivesList;
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

