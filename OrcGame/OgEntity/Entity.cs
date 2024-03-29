﻿using System.Numerics;

namespace OrcGame.OgEntity
{
	public class Entity 
	{
		public Vector2 Location { get; protected set; } = Vector2.Zero;
		public string EntityName { get; protected set; } = "Generic Entity";
		public string InstanceName { get; protected set; } = "Generic Entity Instance";
		
		// private ReturnToPoolDelegate _returnAction;

		// void IPoolable.Initialize(ReturnToPoolDelegate returnAction)
		// {
		// 	// copy the instance reference of the return function so we can call it later
		// 	_returnAction = returnAction;
		// }
		//
		// public void Return()
		// {
		// 	// check if this instance has already been returned
		// 	if (_returnAction != null)
		// 	{
		// 		// not yet returned, return it now
		// 		_returnAction.Invoke(this);
		// 		// set the delegate instance reference to null, so we don't accidentally return it again
		// 		_returnAction = null;
		// 	}
		// }
		//
		// public IPoolable NextNode { get; set; }
		// public IPoolable PreviousNode { get; set; }
	}
	
}

