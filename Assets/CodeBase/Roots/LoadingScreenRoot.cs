using System;
using System.Threading.Tasks;
using DG.Tweening;
using SceneInjection;
using UnityEngine;

namespace Roots
{
	public sealed class LoadingScreenRoot : ASceneRoot
	{
		[SerializeField] private CanvasGroup _canvasGroup;
		[SerializeField] private float _showDuration = 0.5f;
		[SerializeField] private float _hideDuration = 0.5f;
		
		private Tween _tween;

		public async Task Show(Action onShown = null)
		{
			await RunShowTween();
			onShown?.Invoke();
		}

		[ContextMenu("Show")]
		private async Task RunShowTween()
		{
			_tween?.Kill();

			_canvasGroup.interactable = true;
			_canvasGroup.blocksRaycasts = true;

			_tween = _canvasGroup.DOFade(1, _showDuration);
			_tween.onComplete += OnTweenComplete;
			await _tween.AsyncWaitForCompletion();
		}

		[ContextMenu("Hide")]
		public async Task Hide()
		{
			_tween?.Kill();

			_canvasGroup.interactable = false;
			_canvasGroup.blocksRaycasts = false;

			_tween = _canvasGroup.DOFade(0, _hideDuration);
			_tween.onComplete += OnTweenComplete;
			await _tween.AsyncWaitForCompletion();
		}

		private void OnTweenComplete() =>
			_tween?.Kill();
	}
}