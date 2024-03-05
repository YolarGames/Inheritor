using System;
using System.Collections.Generic;
using UnityEngine;

namespace InheritorCode.GameCore.GameServices.GameStateManagement
{
	[Serializable]
	public sealed class GameState : ICommitableGameState, IObservableGameState
	{
		private readonly HashSet<string> _changedProperties = new();
		public Action<HashSet<string>> OnChanged { get; set; } = _ => { };
		public bool IsInTransaction { get; private set; } = false;

		[SerializeField] private int _coins;
		[SerializeField] private int _exp;

		public int Coins
		{
			get => _coins;
			set
			{
				if (!CanProcessValueChange(_coins, value))
					return;

				_coins = value;
				SetPropertyChanged(nameof(Coins));
			}
		}
		public int Exp
		{
			get => _exp;
			set
			{
				if (!CanProcessValueChange(_exp, value))
					return;

				_exp = value;
				SetPropertyChanged(nameof(Exp));
			}
		}

		public void BeginTransaction() =>
			IsInTransaction = true;

		public void EndTransaction() =>
			IsInTransaction = false;

		public void CommitChanges()
		{
			OnChanged?.Invoke(_changedProperties);
			_changedProperties.Clear();
		}

		private void SetPropertyChanged(string propertyName) =>
			_changedProperties.Add(propertyName);

		private bool CanProcessValueChange<T>(T v1, T v2)
		{
			if (IsInTransaction && IsValueDifferent())
				return true;

			Debug.LogWarning("GameState: Can't change value outside of transaction or value is the same.");
			return false;

			bool IsValueDifferent() =>
				!EqualityComparer<T>.Default.Equals(v1, v2);
		}
	}
}