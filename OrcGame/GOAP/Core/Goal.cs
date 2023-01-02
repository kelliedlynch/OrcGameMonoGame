using System;
using System.Collections.Generic;
using OrcGame.Entity.Creature;

namespace OrcGame.GOAP.Core;
public abstract class GoapGoal : GoapBase
{
    public abstract Objective GetObjective(Dictionary<string, object> simulatedState);
}

public abstract class GoapBase
{
    
    
    protected BaseCreature _creature;
    public BaseCreature Creature
    {
        get => _creature;
    }

    public abstract bool IsValid(Dictionary<string, object> state);
    public abstract bool TriggerConditionsMet(Dictionary<string, object> state);
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


