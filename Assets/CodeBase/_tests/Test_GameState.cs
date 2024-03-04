using GameStateManagement;
using NUnit.Framework;

namespace CodeBase._tests
{
	public class Test_GameState
	{
		[Test]
		public void _1_CanInitGameState()
		{
			var gameStateService = GameStateService.Instance;

			using (Transaction<GameState> transaction = gameStateService.StartTransaction())
			{
				transaction.State.Coins = 10;
				transaction.State.Exp = 0;
			}

			Assert.That(gameStateService.State.Coins, Is.EqualTo(10));
			Assert.That(gameStateService.State.Exp, Is.EqualTo(0));
		}

		// [Test]
		// public void _2_CanObserveGameStateChanges()
		// {
		// 	GameStateService gameStateService = GameStateService.Get();
		// 	gameStateService.Init(10, 0);
		// 	GameState gameState = gameStateService.State;
		//
		// 	var stateObserverCalled = false;
		//
		// 	var observer = new PropertyObserver<GameState>(observable: gameState,
		// 		onChanged: Validate,
		// 		nameof(gameState.Coins));
		//
		// 	ShopService.Get().UseCoins(2);
		//
		// 	observer.Dispose();
		//
		// 	Assert.That(stateObserverCalled, "Observer not called");
		// 	return;
		//
		// 	void Validate(GameState state)
		// 	{
		// 		stateObserverCalled = true;
		// 		Assert.That(state.Coins, Is.EqualTo(8));
		// 	}
		// }
		//
		// [Test]
		// public void _3_CanObserveConsistentGameStateChanges()
		// {
		// 	GameStateService gameStateService = GameStateService.Get();
		// 	gameStateService.Init(10, 0);
		// 	GameState gameState = gameStateService.State;
		//
		// 	var stateObserverCalled = false;
		//
		// 	var observer = new PropertyObserver<GameState>(
		// 		observable: gameState,
		// 		onChanged: Validate,
		// 		nameof(gameState.Coins), nameof(gameState.Stars));
		//
		// 	ShopService shopService = ShopService.Get();
		// 	shopService.BuyStars(1, 1);
		//
		// 	observer.Dispose();
		//
		// 	Assert.That(stateObserverCalled, "Observer not called");
		// 	return;
		//
		// 	void Validate(GameState state)
		// 	{
		// 		stateObserverCalled = true;
		// 		Assert.That(state.Coins, Is.EqualTo(9));
		// 		Assert.That(state.Stars, Is.EqualTo(1));
		// 	}
		// }
		//
		// [Test]
		// public void _4_CanObserveConsistentGameStateChanges_Vector3()
		// {
		// 	GameStateService gameStateService = GameStateService.Get();
		// 	gameStateService.Init(10, 0, Vector3.up);
		// 	GameState gameState = gameStateService.State;
		//
		// 	var stateObserverCalled = false;
		//
		// 	var observer = new PropertyObserver<GameState>(
		// 		observable: gameState,
		// 		onChanged: ValidateState,
		// 		nameof(gameState.Coins), nameof(gameState.Stars), nameof(gameState.Vector3));
		//
		// 	using (Transaction<GameState> stateTransaction = gameStateService.StartGameStateTransaction())
		// 	{
		// 		stateTransaction.State.Coins -= 1;
		// 		stateTransaction.State.Stars += 1;
		// 		stateTransaction.State.Vector3 = Vector3.down;
		// 	}
		//
		// 	observer.Dispose();
		//
		// 	Assert.That(stateObserverCalled, "Observer not called");
		// 	return;
		//
		// 	void ValidateState(GameState state)
		// 	{
		// 		stateObserverCalled = true;
		// 		Assert.That(state.Coins, Is.EqualTo(9));
		// 		Assert.That(state.Stars, Is.EqualTo(1));
		// 		Assert.That(state.Vector3, Is.EqualTo(Vector3.down));
		// 	}
		// }
		//
		// [Test]
		// public void _5_CanObserveConsistentGameStateChanges_CustomData()
		// {
		// 	GameStateService gameStateService = GameStateService.Get();
		// 	gameStateService.Init(10, 0, Vector3.up, new CustomGameStatePropertyClass { Value = "June" });
		// 	GameState gameState = gameStateService.State;
		//
		// 	var stateObserverCalled = false;
		//
		// 	var observer = new PropertyObserver<GameState>(observable: gameState,
		// 		onChanged: ValidateState,
		// 		nameof(gameState.Coins), nameof(gameState.Stars),
		// 		nameof(gameState.Vector3), nameof(gameState.CustomClass));
		//
		// 	using (Transaction<GameState> stateTransaction = gameStateService.StartGameStateTransaction())
		// 	{
		// 		stateTransaction.State.Coins -= 3;
		// 		stateTransaction.State.Stars += 3;
		//
		// 		stateTransaction.State.Vector3 = Vector3.down;
		// 		stateTransaction.State.CustomClass.Value = "June's Journey";
		// 	}
		//
		// 	observer.Dispose();
		//
		// 	Assert.That(stateObserverCalled, "Observer not called");
		// 	return;
		//
		// 	void ValidateState(GameState state)
		// 	{
		// 		stateObserverCalled = true;
		// 		Assert.That(state.Stars, Is.EqualTo(3));
		// 		Assert.That(state.Coins, Is.EqualTo(7));
		// 		Assert.That(state.Vector3, Is.EqualTo(Vector3.down));
		// 		Assert.That(state.CustomClass.Value, Is.EqualTo("June's Journey"));
		// 	}
		// }
		//
		// [Test]
		// public void _6_CanObserveConsistentGameStateChanges_NoChangesOutOfTransactionScope()
		// {
		// 	GameStateService gameStateService = GameStateService.Get();
		// 	gameStateService.Init(10, 0);
		// 	GameState gameState = gameStateService.State;
		//
		// 	var stateObserverCalled = false;
		//
		// 	var observer = new PropertyObserver<GameState>(
		// 		observable: gameState,
		// 		onChanged: Validate,
		// 		nameof(gameState.Coins));
		//
		// 	using (Transaction<GameState> transaction = gameStateService.StartGameStateTransaction())
		// 	{
		// 		transaction.State.Coins -= 3;
		// 	}
		//
		// 	gameState.Coins -= 6;
		//
		// 	observer.Dispose();
		//
		// 	Assert.That(stateObserverCalled, "Observer not called");
		// 	return;
		//
		// 	void Validate(GameState state)
		// 	{
		// 		stateObserverCalled = true;
		// 		Assert.That(state.Coins, Is.EqualTo(7));
		// 	}
		// }
		//
		// [Test]
		// public void _7_CanObserveConsistentGameStateChanges_NestedTransactions()
		// {
		// 	GameStateService gameStateService = GameStateService.Get();
		// 	gameStateService.Init(10, 0);
		// 	GameState gameState = gameStateService.State;
		//
		// 	var stateObserverCalled = false;
		//
		// 	var observer = new PropertyObserver<GameState>(
		// 		observable: gameState,
		// 		onChanged: Validate,
		// 		nameof(gameState.Coins), nameof(gameState.Stars));
		//
		// 	using (Transaction<GameState> transaction = gameStateService.StartGameStateTransaction())
		// 	{
		// 		transaction.State.Coins -= 3;
		//
		// 		ShopService.Get().BuyStars(1, 2);
		// 		ShopService.Get().BuyStars(1, 2);
		// 	}
		//
		// 	observer.Dispose();
		//
		// 	Assert.That(stateObserverCalled, "Observer not called");
		// 	return;
		//
		// 	void Validate(GameState state)
		// 	{
		// 		stateObserverCalled = true;
		// 		Assert.That(state.Coins, Is.EqualTo(3));
		// 		Assert.That(state.Stars, Is.EqualTo(2));
		// 	}
		// }
		//
		// [Test]
		// public void _8_CanObserveConsistentGameStateChanges_AbortTransaction()
		// {
		// 	GameStateService gameStateService = GameStateService.Get();
		// 	gameStateService.Init(10, 0);
		// 	GameState gameState = gameStateService.State;
		//
		// 	var stateObserverCalled = false;
		// 	var somethingWrong = true;
		//
		// 	var observer = new PropertyObserver<GameState>(observable: gameState,
		// 		onChanged: Validate,
		// 		nameof(gameState.Coins), nameof(gameState.Stars));
		//
		// 	using (Transaction<GameState> transaction1 = gameStateService.StartGameStateTransaction())
		// 	{
		// 		transaction1.State.Coins -= 3;
		// 		ShopService.Get().BuyStars(1, 2);
		//
		// 		using (Transaction<GameState> transaction2 = gameStateService.StartGameStateTransaction())
		// 		{
		// 			ShopService.Get().BuyStars(1, 2);
		// 			ShopService.Get().BuyStars(1, 2);
		//
		// 			if (somethingWrong)
		// 				transaction2.AbortTransaction();
		// 		}
		// 	}
		//
		// 	observer.Dispose();
		//
		// 	Assert.That(stateObserverCalled, "Observer not called");
		// 	return;
		//
		// 	void Validate(GameState state)
		// 	{
		// 		stateObserverCalled = true;
		// 		Assert.That(state.Coins, Is.EqualTo(5));
		// 		Assert.That(state.Stars, Is.EqualTo(1));
		// 	}
		// }
		//
		// [Test]
		// public void _9_CanObserveConsistentGameStateChanges_ValidateOnes()
		// {
		// 	GameStateService gameStateService = GameStateService.Get();
		// 	gameStateService.Init(10, 0);
		// 	GameState gameState = gameStateService.State;
		//
		// 	var stateObserverCalled = false;
		// 	var validationCounts = 0;
		//
		// 	var observer = new PropertyObserver<GameState>(observable: gameState,
		// 		onChanged: Validate,
		// 		nameof(gameState.Coins), nameof(gameState.Stars));
		//
		// 	using (Transaction<GameState> transaction1 = gameStateService.StartGameStateTransaction())
		// 	{
		// 		transaction1.State.Coins -= 3;
		//
		// 		using (Transaction<GameState> transaction2 = gameStateService.StartGameStateTransaction())
		// 		{
		// 			ShopService.Get().BuyStars(1, 2);
		// 			ShopService.Get().BuyStars(1, 2);
		// 		}
		//
		// 		using (Transaction<GameState> transaction3 = gameStateService.StartGameStateTransaction())
		// 		{
		// 			ShopService.Get().BuyStars(1, 2);
		// 		}
		// 	}
		//
		// 	observer.Dispose();
		//
		// 	Assert.That(stateObserverCalled, "Observer not called");
		// 	return;
		//
		// 	void Validate(GameState state)
		// 	{
		// 		stateObserverCalled = true;
		// 		validationCounts++;
		//
		// 		Assert.That(state.Coins, Is.EqualTo(1));
		// 		Assert.That(state.Stars, Is.EqualTo(3));
		// 		Assert.That(validationCounts, Is.EqualTo(1));
		// 	}
		// }
	}
}