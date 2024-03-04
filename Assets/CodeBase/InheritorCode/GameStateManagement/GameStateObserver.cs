using System;
using System.Collections.Generic;

namespace GameStateManagement
{
	public sealed class GameStateObserver<TState> : IDisposable where TState : IObservableGameState
	{
		private readonly Action<TState> _onChanged;
		private readonly HashSet<string> _trackedProperties;
		private TState _observable;

		public GameStateObserver(TState observable, Action<TState> onChanged, params string[] properties)
		{
			_observable = observable;
			_observable.OnChanged += CompareProperties;
			_onChanged = onChanged;
			_trackedProperties = new HashSet<string>(properties);
		}

		public void Dispose()
		{
			_trackedProperties.Clear();
			_observable.OnChanged -= CompareProperties;
		}

		private void CompareProperties(HashSet<string> changedProperties)
		{
			foreach (string property in changedProperties)
				if (_trackedProperties.Contains(property))
				{
					_onChanged?.Invoke(_observable);
					return;
				}
		}
	}
}