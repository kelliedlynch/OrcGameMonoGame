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

        public override OperatorObjective GetObjective()
        {
            var ownedQuery =
                from owned in Creature.Owned
                where owned.Material == Material.Bone
                select owned;
            var carriedQuery =
                from item in Creature.Carried
                where item.Material == Material.Bone
                select item;

            var objective1 = new QueryObjective()
            {
                Target = "Creature.Carried",
                Operator = Operator.ContainsAtLeast,
                Quantity = 1,
                PropsQuery = (IEnumerable<Entity.Entity>)ownedQuery
            };
            var objective2 = new QueryObjective()
            {
                Target = "Creature.Owned",
                Operator = Operator.ContainsAtLeast,
                Quantity = 1,
                PropsQuery = (IEnumerable<Entity.Entity>)ownedQuery
            };

            var compiledObjective = new OperatorObjective()
            {
                Operator = Operator.AND,
                ObjectivesList = { objective1, objective2 }
            };

            return compiledObjective;
        }


    }

}

