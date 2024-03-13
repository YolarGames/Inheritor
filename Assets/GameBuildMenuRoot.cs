using System;
using InheritorCode.SceneInjection;
using InheritorCode.Utils;
using UnityEngine;
using UnityEngine.UIElements;

public class GameBuildMenuRoot : ASceneRoot
{
	#region UXML Fields

	private const string k_buildItemShown = "build-item-shown";
	private const string k_buildButton = "btn_build";
	private const string k_hayButton = "btn_hay";
	private const string k_cottonButton = "btn_cotton";
	private const string k_mineButton = "btn_mine";
	private const string k_breadButton = "btn_bread";
	private const string k_clothButton = "btn_cloth";
	private const string k_weaponButton = "btn_weapon";
	private const string k_solderButton = "btn_solder";

	private Button _buildButton;
	private Button _hayButton;
	private Button _cottonButton;
	private Button _mineButton;
	private Button _breadButton;
	private Button _clothButton;
	private Button _weaponButton;
	private Button _solderButton;

	#endregion

	[SerializeField] private UIDocument _document;
	private VisualElement _root;

	public override void Go()
	{
		base.Go();

		_root = _document.rootVisualElement;
		_buildButton = _root.GetButton(k_buildButton);
		_hayButton = _root.GetButton(k_hayButton);
		_cottonButton = _root.GetButton(k_cottonButton);
		_mineButton = _root.GetButton(k_mineButton);
		_breadButton = _root.GetButton(k_breadButton);
		_clothButton = _root.GetButton(k_clothButton);
		_weaponButton = _root.GetButton(k_weaponButton);
		_solderButton = _root.GetButton(k_solderButton);

		RegisterCallbacks();
	}

	private void OnDisable()
	{
		UnregisterCallbacks();
	}

	private void RegisterCallbacks()
	{
		_buildButton.RegisterClickEvent(ToggleBuildMenu);
		_hayButton.RegisterClickEvent(OnHayButton);
		_cottonButton.RegisterClickEvent(OnCottonButton);
		_weaponButton.RegisterClickEvent(OnWeaponButton);
		_mineButton.RegisterClickEvent(OnMineButton);
		_breadButton.RegisterClickEvent(OnBreadButton);
		_clothButton.RegisterClickEvent(OnClothButton);
		_solderButton.RegisterClickEvent(OnSolderButton);
	}

	private void UnregisterCallbacks()
	{
		_buildButton.UnregisterClickEvent(ToggleBuildMenu);
		_hayButton.UnregisterClickEvent(OnHayButton);
		_cottonButton.UnregisterClickEvent(OnCottonButton);
		_weaponButton.UnregisterClickEvent(OnWeaponButton);
		_mineButton.UnregisterClickEvent(OnMineButton);
		_breadButton.UnregisterClickEvent(OnBreadButton);
		_clothButton.UnregisterClickEvent(OnClothButton);
		_solderButton.UnregisterClickEvent(OnSolderButton);
	}

	private void OnHayButton(ClickEvent evt)
	{
		throw new NotImplementedException();
	}

	private void OnCottonButton(ClickEvent evt)
	{
		throw new NotImplementedException();
	}

	private void OnWeaponButton(ClickEvent evt)
	{
		throw new NotImplementedException();
	}

	private void OnMineButton(ClickEvent evt)
	{
		throw new NotImplementedException();
	}

	private void OnBreadButton(ClickEvent evt)
	{
		throw new NotImplementedException();
	}

	private void OnClothButton(ClickEvent evt)
	{
		throw new NotImplementedException();
	}

	private void OnSolderButton(ClickEvent evt)
	{
		throw new NotImplementedException();
	}

	private void ToggleBuildMenu(ClickEvent evt)
	{
		_hayButton.ToggleInClassList(k_buildItemShown);
		_cottonButton.ToggleInClassList(k_buildItemShown);
		_mineButton.ToggleInClassList(k_buildItemShown);
		_breadButton.ToggleInClassList(k_buildItemShown);
		_clothButton.ToggleInClassList(k_buildItemShown);
		_weaponButton.ToggleInClassList(k_buildItemShown);
		_solderButton.ToggleInClassList(k_buildItemShown);
	}
}