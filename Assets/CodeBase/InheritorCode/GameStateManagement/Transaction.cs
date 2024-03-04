using System;
using System.Reflection;

namespace GameStateManagement
{
	public sealed class Transaction<TState> : IDisposable where TState : ICommitableGameState
	{
		private readonly PropertyInfo[] _properties;
		private readonly Action<TState>[] _onComplete;
		private readonly TState _snapshot;
		private readonly bool _isNestedTransaction;

		private bool _isAborted = false;

		public TState State { get; }

		public Transaction(TState state, params Action<TState>[] onComplete)
		{
			State = state;
			_properties = typeof(TState).GetProperties();
			_onComplete = onComplete;
			_snapshot = CreateSnapshot(state);

			if (state.IsInTransaction)
				_isNestedTransaction = true;
			else
				State.BeginTransaction();
		}

		public void AbortTransaction() =>
			_isAborted = true;

		public void Dispose()
		{
			if (_isAborted)
				RestoreSnapshot();
			else
				Commit();
		}

		private void Commit()
		{
			if (_isNestedTransaction)
				return;

			State.EndTransaction();
			State.CommitChanges();

			foreach (Action<TState> action in _onComplete)
				action?.Invoke(State);
		}

		private TState CreateSnapshot(TState state)
		{
			var snapshot = Activator.CreateInstance<TState>();

			snapshot.BeginTransaction();

			foreach (PropertyInfo property in _properties)
				property.SetValue(snapshot, property.GetValue(state));

			snapshot.EndTransaction();

			return snapshot;
		}

		private void RestoreSnapshot()
		{
			foreach (PropertyInfo property in _properties)
				property.SetValue(State, property.GetValue(_snapshot));
		}
	}
}