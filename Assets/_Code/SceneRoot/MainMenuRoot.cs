using GameCore;
using SimpleInjector;

namespace SceneRoot
{
	public class MainMenuRoot : SceneRootBase
	{
		[Inject] private GameStateMachine _gameStateMachine;
	}
}