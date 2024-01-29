using System;
using UnityEngine;
using UnityEngine.UIElements;
using Utils;

public sealed class GameSettingsController : IDisposable
{
	private const string k_hideRight = "translate-hided-right";
	private const string k_settings = "settingsMenu";
	private const string k_master = "master";
	private const string k_music = "music";
	private const string k_sound = "sound";
	private const string k_back = "btn_back";

	private readonly VisualElement _settings;
	private readonly Button _master;
	private readonly Button _music;
	private readonly Button _sound;
	private readonly Button _back;
	private readonly Action _onSettingsHide;

	public GameSettingsController(UIDocument document, Action onSettingsHide)
	{
		_onSettingsHide = onSettingsHide;
		_settings = document.rootVisualElement.Q<VisualElement>(k_settings);

		if (_settings == null)
			Debug.LogError("cant find settings");

		_master = _settings.GetButton(k_master);
		_music = _settings.GetButton(k_music);
		_sound = _settings.GetButton(k_sound);
		_back = _settings.GetButton(k_back);

		_back.RegisterCallback<ClickEvent>(Hide);
	}

	public void Show() =>
		_settings.RemoveFromClassList(k_hideRight);

	public void Dispose() =>
		_back.UnregisterCallback<ClickEvent>(Hide);

	private void Hide(ClickEvent evt)
	{
		_settings.AddToClassList(k_hideRight);
		_onSettingsHide?.Invoke();
	}
}