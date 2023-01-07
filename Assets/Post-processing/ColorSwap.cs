using System;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

[Serializable]
[PostProcess(typeof(ColorSwapRenderer), PostProcessEvent.AfterStack, "Custom/Color Swap")]
public sealed class ColorSwap : PostProcessEffectSettings {
	public ColorParameter redSwap = new ColorParameter();
	public ColorParameter greenSwap = new ColorParameter();
	public ColorParameter blueSwap = new ColorParameter();
	public ColorParameter greySwap = new ColorParameter();
	public ColorParameter restSwap = new ColorParameter();
}

public sealed class ColorSwapRenderer : PostProcessEffectRenderer<ColorSwap> {
	public override void Render(PostProcessRenderContext context) {
		var sheet = context.propertySheets.Get(Shader.Find("Hidden/Custom/ColorSwap"));
		sheet.properties.SetColor("_Red", settings.redSwap.value);
		sheet.properties.SetColor("_Green", settings.greenSwap.value);
		sheet.properties.SetColor("_Blue", settings.blueSwap.value);
		sheet.properties.SetColor("_Grey", settings.greySwap.value);
		sheet.properties.SetColor("_Rest", settings.restSwap.value);
		context.command.BlitFullscreenTriangle(context.source, context.destination, sheet, 0);
	}
}