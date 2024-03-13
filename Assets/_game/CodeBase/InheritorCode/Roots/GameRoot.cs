using InheritorCode.SceneInjection;

namespace InheritorCode.Roots
{
	public sealed class GameRoot : ASceneRoot
	{
		[InjectScene] private FarmRoot _farm;
		[InjectScene] private GameBackgroundRoot _gameBackground;
		[InjectScene] private UiGameMenuRoot _gameMenu;

		public override void Go()
		{
			base.Go();
		}
	}
}