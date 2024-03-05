using System.Threading.Tasks;
using InheritorCode.SceneInjection;
using InheritorCode.UI;
using InheritorCode.Utils;
using UnityEngine;
using UnityEngine.UIElements;

namespace InheritorCode.Roots
{
	public sealed class UiLoadingScreenRoot : ASceneRoot
	{
		[SerializeField] private UIDocument _document;

		private const string k_loadingScreen = "loadingScreen";

		private VisualElement _loadingScreen;
		private ShowHideHandler _showHideHandler;

		private VisualElement LoadingScreen => _loadingScreen ??= _document.GetVisualElement(k_loadingScreen);

		private ShowHideHandler ShowHideHandler => _showHideHandler ??= new ShowHideHandler(LoadingScreen, this);

		[ContextMenu("Show")]
		public async Task Show() =>
			await ShowHideHandler.Show();

		[ContextMenu("Hide")]
		public async Task Hide() =>
			await ShowHideHandler.Hide();
	}
}