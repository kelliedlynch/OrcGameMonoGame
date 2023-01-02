using System.Collections.Generic;
namespace OrcGame.GOAP.Core;
public abstract class GoapAction : GoapBase
{
    
    public abstract void GetTransform(Dictionary<string, object> state);
}


