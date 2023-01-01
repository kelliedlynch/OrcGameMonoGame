using System;
using OrcGame.Entity;

namespace OrcGame.GOAP
{

	public abstract class Goal : GoapBase
	{
        public abstract OperatorObjective GetObjective();
    }

    public abstract class GoapBase
    {
        protected Creature _creature;
        public Creature Creature
        {
            get => _creature;
        }

        public abstract bool IsValid();
        public abstract bool TriggerConditionsMet();
    }


	//interface IGoap
	//{
 //       Creature Creature
 //       {
 //           get;
 //       }

 //       //protected abstract Creature _creature;
 //       //public Creature Creature
 //       //{
 //       //    get => _creature;
 //       //}

 //       public abstract bool IsValid();
	//	public abstract bool TriggerConditionsMet();
		
	//}
}

