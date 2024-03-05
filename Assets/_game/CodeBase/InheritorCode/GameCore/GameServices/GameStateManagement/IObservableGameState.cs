using System;
using System.Collections.Generic;

namespace InheritorCode.GameCore.GameServices.GameStateManagement
{
	public interface IObservableGameState
	{
		Action<HashSet<string>> OnChanged { get; set; }
	}
}