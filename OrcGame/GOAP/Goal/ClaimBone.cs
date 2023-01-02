using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using OrcGame.Entity;

namespace OrcGame.GOAP
{
    public class ClaimBone : Goal
    {
        public ClaimBone(Creature creature)
        {
            _creature = creature;
        }

        public override bool IsValid()
        {
            throw new NotImplementedException();
        }

        public override bool TriggerConditionsMet()
        {
            throw new NotImplementedException();
        }

        public override OperatorObjective GetObjective(Dictionary<string, object> simulatedState)
        {
            var ownedList = simulatedState["Owned"] as ArrayList;
            var ownedQuery =
                from Dictionary<string, object> owned in ownedList
                where (Material)owned["Material"] == Material.Bone
                select owned;
            var carriedList = simulatedState["Carried"] as ArrayList;
            var carriedQuery =
                from Dictionary<string, object> carried in carriedList
                where (Material)carried["Material"] == Material.Bone
                select carried;

            var carriedContainsBone = new QueryObjective()
            {
                Target = "Creature.Carried",
                QueryType = QueryType.ContainsAtLeast,
                Quantity = 1,
                PropsQuery = (IEnumerable<Dictionary<string, object>>)ownedQuery
            };
            var ownedContainsBone = new QueryObjective()
            {
                Target = "Creature.Owned",
                QueryType = QueryType.ContainsAtLeast,
                Quantity = 1,
                PropsQuery = (IEnumerable<Dictionary<string, object>>)ownedQuery
            };

            var compiledObjective = new OperatorObjective()
            {
                Operator = Operator.And,
                ObjectivesList = new List<Objective>(){ carriedContainsBone, ownedContainsBone }
            };

            return compiledObjective;
        }


    }

}

