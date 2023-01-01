using System;
using System.Collections.Generic;

namespace OrcGame.GOAP
{
    public abstract record Objective
    {
        public string Target;
        public Operator Operator;
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
        public int Quantity;
        public IEnumerable<Entity.Entity> PropsQuery;
    }

    public record OperatorObjective : Objective
    {
        public List<Objective> ObjectivesList;
    }

    public enum Operator
    {
        ContainsAtLeast,
        ContainsLessThan,
        Equals,
        DoesNotEqual,
        IsGreaterThan,
        IsLessThan,
        IsGreaterThanOrEqualTo,
        IsLessThanOrEqualTo,
        AND,
        OR,
        NOT
    }
}

