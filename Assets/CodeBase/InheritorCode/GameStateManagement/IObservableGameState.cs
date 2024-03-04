using System;
using System.Collections.Generic;

namespace GameStateManagement
{
	public interface IObservableGameState
	{
		Action<HashSet<string>> OnChanged { get; set; }
	}
}