using InheritorCode.SceneInjection;

namespace InheritorCode.Roots
{
	public sealed class GameRoot : ASceneRoot
	{
		[InjectScene] private GameBuildMenuRoot _gameBuildMenu;
		[InjectScene] private UiGameMenuRoot _gameMenu;
		[InjectScene] private FarmRoot _farm;
		[InjectScene] private GameBackgroundRoot _gameBackground;

		public override void Go()
		{
			base.Go();
		}
	}
}