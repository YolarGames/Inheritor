﻿using Configs;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
	[RequireComponent(typeof(CanvasScaler))]
	public sealed class CanvasScalerSetup : MonoBehaviour
	{
		[SerializeField] private CanvasScalerConfig _config;
		private CanvasScaler _scaler;

#if UNITY_EDITOR
		private void Reset()
		{
			_scaler = GetComponent<CanvasScaler>();

			_scaler.uiScaleMode = _config.ScaleMode;
			_scaler.referenceResolution = _config.ReferenceResolution;
			_scaler.screenMatchMode = _config.MatchMode;
			_scaler.matchWidthOrHeight = _config.MatchWidthHeight;
			_scaler.referencePixelsPerUnit = _config.ReferencePixelsPerUnit;
		}
#endif
	}
}