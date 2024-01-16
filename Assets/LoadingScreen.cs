using System;
using System.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

[RequireComponent(typeof(CanvasGroup))]
public sealed class LoadingScreen : MonoBehaviour
{
	[SerializeField] private float _showDuration;
	[SerializeField] private float _hideDuration;
	private Tween _tween;

	private CanvasGroup _canvasGroup;

	private void Awake() =>
		_canvasGroup = GetComponent<CanvasGroup>();

	public async Task Show(Action onShown)
	{
		_tween?.Kill();

		_canvasGroup.interactable = true;
		_canvasGroup.blocksRaycasts = true;

		_tween = _canvasGroup.DOFade(1, _showDuration);
		_tween.onComplete += OnTweenComplete;
		await _tween.AsyncWaitForCompletion();
		onShown?.Invoke();
	}

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