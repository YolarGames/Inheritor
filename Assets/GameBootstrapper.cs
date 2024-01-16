using GameCore;
using UnityEngine;

public sealed class GameBootstrapper : MonoBehaviour
{
	private void Awake()
	{
		GameStateMachine.Instance.CreateStates();
		GameStateMachine.Instance.Enter<StateBootstrap>();
	}
}