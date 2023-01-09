using System;
using OrcGame.Utility;
// using System.Linq;
// using MonoGame.Extended.Collections;

namespace OrcGame.GOAP.Core;

public abstract class Simulated : IPoolable
{
	public abstract void Reset();
}

public abstract class SimulatedGroup : Simulated
{

	
}

public class NotGroupItemException : Exception
{
	
} 
