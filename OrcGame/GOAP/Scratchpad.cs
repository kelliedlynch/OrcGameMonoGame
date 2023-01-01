using System.Collections.Generic;

namespace OrcGame.GOAP;

public class Scratchpad
{
    public class Action
    {
        
    }

    public class Goal
    {
        
    }

    public class ClaimBone : Goal
    {
        public Dictionary<string, object> desiredState;

        public ClaimBone()
        {
            desiredState = new()
            {
                {"hasInInventoryWithProps", 2}
            };
        }
    }
}