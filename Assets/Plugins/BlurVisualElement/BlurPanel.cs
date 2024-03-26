using UnityEngine;
using UnityEngine.UIElements;

public class BlurPanel : VisualElement
{
	private static readonly int IntensityProperty = Shader.PropertyToID("_Intensity");
	private readonly Material _blurMaterial = new(Shader.Find("Shader Graphs/Blur"));

	public float Intensity
	{
		get => _blurMaterial.GetFloat(IntensityProperty);
		private set
		{
			if (value is < 0 or > 2)
				return;

			_blurMaterial.SetFloat(IntensityProperty, value);
			BlurTexture();
		}
	}

	private Texture _srcTexture;
	private RenderTexture _destTexture;


	public BlurPanel()
	{
		RegisterCallback<GeometryChangedEvent>(evt => { BlurTexture(); });
		RegisterCallback<AttachToPanelEvent>(evt => { BlurTexture(); });

		// Take a picture behind the panel
		// Apply a blur to the picture
		// Set th	e picture as the background of the panel
		// Create a new VisualElement to hold the picture
		var picture = new VisualElement();
		picture.name = "Picture";
	}

	private void BlurTexture()
	{
		if (_srcTexture == null)
			_srcTexture = resolvedStyle.backgroundImage.texture ?? resolvedStyle.backgroundImage.sprite?.texture;

		if (_srcTexture == null)
			return;

		style.backgroundImage = ToTexture2D();
		MarkDirtyRepaint();
	}

	private Texture2D ToTexture2D()
	{
		RenderTexture currentRT = RenderTexture.active;

		var renderTexture = new RenderTexture(_srcTexture.width, _srcTexture.height, 32);
		Graphics.Blit(_srcTexture, renderTexture, _blurMaterial);

		RenderTexture.active = renderTexture;
		var texture2D = new Texture2D(_srcTexture.width, _srcTexture.height, TextureFormat.RGBA32, false);
		texture2D.ReadPixels(new Rect(0, 0, renderTexture.width, renderTexture.height), 0, 0);
		texture2D.Apply();

		RenderTexture.active = currentRT;

		return texture2D;
	}

	#region UXML Stuff

	public new class UxmlTraits : VisualElement.UxmlTraits
	{
		private readonly UxmlFloatAttributeDescription _blurAmount = new()
		{
			name = "Intensity",
			defaultValue = 0.5f,
		};

		public override void Init(VisualElement ve, IUxmlAttributes bag, CreationContext cc)
		{
			base.Init(ve, bag, cc);
			((BlurPanel)ve).Intensity = _blurAmount.GetValueFromBag(bag, cc);
		}
	}

	public new class UxmlFactory : UxmlFactory<BlurPanel, UxmlTraits> { }

	#endregion
}