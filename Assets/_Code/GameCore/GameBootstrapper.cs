using UI;
using UnityEngine;

namespace GameCore
{
	public sealed class GameBootstrapper : MonoBehaviour
	{
		[SerializeField] private LoadingScreen _loadingScreen;

		private void Awake()
		{
			GameStateMachine.Instance.InitStateMachine(_loadingScreen);
			GameStateMachine.Instance.Enter<StateBootstrap>();
		}
	}
}