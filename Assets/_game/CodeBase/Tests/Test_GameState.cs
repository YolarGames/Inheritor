using InheritorCode.GameCore.GameServices.GameStateManagement;
using NUnit.Framework;

namespace Tests
{
	public class Test_GameState
	{
		[Test]
		public void _1_CanInit()
		{
			var gameStateService = new GameStateService(null);
			gameStateService.Init();

			using (Transaction<GameState> transaction = gameStateService.StartTransaction())
			{
				transaction.State.Coins = 10;
				transaction.State.Exp = 0;
			}

			Assert.That(gameStateService.State.Coins, Is.EqualTo(10));
			Assert.That(gameStateService.State.Exp, Is.EqualTo(0));
		}

		[Test]
		public void _2_CanObserveChanges()
		{
			var stateObserverCalled = false;
			var gameStateService = new GameStateService(null);
			gameStateService.Init();

			using (Transaction<GameState> transaction = gameStateService.StartTransaction())
				transaction.State.Coins = 10;

			GameStateObserver<GameState> observer =
				gameStateService.CreateObserver(Validate, nameof(gameStateService.State.Coins));

			using (Transaction<GameState> transaction = gameStateService.StartTransaction())
				transaction.State.Coins -= 2;

			observer.Dispose();

			Assert.That(stateObserverCalled, "Observer not called");

			return;

			void Validate(GameState state)
			{
				stateObserverCalled = true;
				Assert.That(state.Coins, Is.EqualTo(8));
			}
		}

		[Test]
		public void _3_CanObserveConsistentChanges()
		{
			var stateObserverCalled = false;
			var gameStateService = new GameStateService(null);
			gameStateService.Init();

			using (Transaction<GameState> transaction = gameStateService.StartTransaction())
			{
				transaction.State.Coins = 10;
				transaction.State.Exp = 0;
			}

			GameStateObserver<GameState> observer = gameStateService.CreateObserver(Validate,
				nameof(gameStateService.State.Coins), nameof(gameStateService.State.Exp));

			using (Transaction<GameState> transaction = gameStateService.StartTransaction())
			{
				transaction.State.Coins -= 1;
				transaction.State.Exp += 1;
			}

			observer.Dispose();

			Assert.That(stateObserverCalled, "Observer not called");
			return;

			void Validate(GameState state)
			{
				stateObserverCalled = true;
				Assert.That(state.Coins, Is.EqualTo(9));
				Assert.That(state.Exp, Is.EqualTo(1));
			}
		}

		[Test]
		public void _4_HaveNoChangesOutOfTransactionScope()
		{
			var gameStateService = new GameStateService(null);
			gameStateService.Init();

			using (Transaction<GameState> transaction = gameStateService.StartTransaction())
				transaction.State.Coins = 10;

			var stateObserverCalled = false;

			GameStateObserver<GameState> observer =
				gameStateService.CreateObserver(Validate, nameof(gameStateService.State.Coins));

			using (Transaction<GameState> transaction = gameStateService.StartTransaction())
				transaction.State.Coins -= 3;

			gameStateService.State.Coins -= 6;

			observer.Dispose();

			Assert.That(stateObserverCalled, "Observer not called");
			return;

			void Validate(GameState state)
			{
				stateObserverCalled = true;
				Assert.That(state.Coins, Is.EqualTo(7));
			}
		}

		[Test]
		public void _5_CanDoNestedTransactions()
		{
			var gameStateService = new GameStateService(null);
			gameStateService.Init();

			using (Transaction<GameState> transaction = gameStateService.StartTransaction())
			{
				transaction.State.Coins = 10;
				transaction.State.Exp = 0;
			}

			var stateObserverCalled = false;

			GameStateObserver<GameState> observer =
				gameStateService.CreateObserver(Validate,
					nameof(gameStateService.State.Coins),
					nameof(gameStateService.State.Exp));

			using (Transaction<GameState> transaction = gameStateService.StartTransaction())
			{
				transaction.State.Coins -= 3;

				using (Transaction<GameState> transaction2 = gameStateService.StartTransaction())
					transaction2.State.Coins -= 3;

				using (Transaction<GameState> transaction3 = gameStateService.StartTransaction())
				{
					transaction3.State.Exp += 1;

					using (Transaction<GameState> transaction4 = gameStateService.StartTransaction())
						transaction4.State.Coins -= 3;
				}
			}

			observer.Dispose();

			Assert.That(stateObserverCalled, "Observer not called");

			return;

			void Validate(GameState state)
			{
				stateObserverCalled = true;
				Assert.That(state.Coins, Is.EqualTo(1));
				Assert.That(state.Exp, Is.EqualTo(1));
			}
		}

		[Test]
		public void _6_CanAbortTransaction()
		{
			var gameStateService = new GameStateService(null);
			gameStateService.Init();

			using (Transaction<GameState> transaction = gameStateService.StartTransaction())
			{
				transaction.State.Coins = 10;
				transaction.State.Exp = 0;
			}

			GameState gameState = gameStateService.State;

			var stateObserverCalled = false;
			var somethingWrong = true;

			GameStateObserver<GameState> observer = gameStateService.CreateObserver(Validate,
				nameof(gameState.Coins), nameof(gameState.Exp));

			using (Transaction<GameState> transaction1 = gameStateService.StartTransaction())
			{
				transaction1.State.Coins -= 3;

				transaction1.State.Coins -= 2;
				transaction1.State.Exp += 1;

				using (Transaction<GameState> transaction2 = gameStateService.StartTransaction())
				{
					transaction2.State.Coins -= 2;
					transaction2.State.Exp += 1;

					transaction2.State.Coins -= 2;
					transaction2.State.Exp += 1;

					if (somethingWrong)
						transaction2.AbortTransaction();
				}
			}

			observer.Dispose();

			Assert.That(stateObserverCalled, "Observer not called");
			return;

			void Validate(GameState state)
			{
				stateObserverCalled = true;
				Assert.That(state.Coins, Is.EqualTo(5));
				Assert.That(state.Exp, Is.EqualTo(1));
			}
		}

		[Test]
		public void _7_CanValidateOnes()
		{
			var gameStateService = new GameStateService(null);
			gameStateService.Init();

			using (Transaction<GameState> transaction = gameStateService.StartTransaction())
			{
				transaction.State.Coins = 10;
				transaction.State.Exp = 0;
			}

			GameState gameState = gameStateService.State;

			var stateObserverCalled = false;
			var validationCounts = 0;

			GameStateObserver<GameState> observer = gameStateService.CreateObserver(Validate,
				nameof(gameState.Coins), nameof(gameState.Exp));

			using (Transaction<GameState> transaction1 = gameStateService.StartTransaction())
			{
				transaction1.State.Coins -= 3;

				using (Transaction<GameState> transaction2 = gameStateService.StartTransaction())
				{
					transaction2.State.Coins -= 2;
					transaction2.State.Exp += 1;

					transaction2.State.Coins -= 2;
					transaction2.State.Exp += 1;
				}

				using (Transaction<GameState> transaction3 = gameStateService.StartTransaction())
				{
					transaction3.State.Coins -= 2;
					transaction3.State.Exp += 1;
				}
			}

			observer.Dispose();

			Assert.That(stateObserverCalled, "Observer not called");
			return;

			void Validate(GameState state)
			{
				stateObserverCalled = true;
				validationCounts++;

				Assert.That(state.Coins, Is.EqualTo(1));
				Assert.That(state.Exp, Is.EqualTo(3));
				Assert.That(validationCounts, Is.EqualTo(1));
			}
		}
	}
}