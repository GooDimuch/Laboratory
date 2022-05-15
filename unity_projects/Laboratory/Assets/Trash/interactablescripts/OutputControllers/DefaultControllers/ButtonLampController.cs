using System;
using UnityEngine;

public class ButtonLampController : BaseOutputController<float> {
#region constants
#endregion

#region inspector
	public int materialId;
	public GameObject meshR;
	public int brightnessCoef = 300;
#endregion

#region internal variable
	private MeshRenderer mesh;
	private Color colorOff;
	private Color colorOn;
#endregion

	public new void Start() {
		mesh = meshR.GetComponent<MeshRenderer>();
		colorOff = mesh.materials[materialId].color;
		var tempColor = colorOff;
		tempColor.r *= 1 + brightnessCoef / 100f;
		tempColor.g *= 1 + brightnessCoef / 100f;
		tempColor.b *= 1 + brightnessCoef / 100f;
		colorOn = tempColor;
		base.Start();
	}

	private Color GetColor(float newState) =>
			new Color {
				r = colorOff.r + (colorOn.r - colorOff.r) * newState,
				g = colorOff.g + (colorOn.g - colorOff.g) * newState,
				b = colorOff.b + (colorOn.b - colorOff.b) * newState
			};

	protected override void PlaySound(float newState) { }

	protected override void Animate(float newState) {
		if (Math.Abs(newState) < float.Epsilon) {
			mesh.materials[materialId].SetColor("_EmissionColor", Color.black);
			mesh.materials[materialId].SetColor("_Color", GetColor(newState));
		} else if (newState > 0) { mesh.materials[materialId].SetColor("_EmissionColor", GetColor(newState)); }
	}
}