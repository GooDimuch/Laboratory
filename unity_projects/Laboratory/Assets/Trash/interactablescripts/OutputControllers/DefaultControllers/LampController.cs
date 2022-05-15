using System;
using UnityEngine;

public class LampController : BaseOutputController<bool> {
	public enum AnimationType {
		EnableGameObject,
		ChangeMaterial,
		ChangeBrightness,
	}

#region constants
#endregion

#region inspector
	[Header("")] public AnimationType animationType;

#region EnableGameObject
	[ConditionalField(nameof(animationType), false, AnimationType.EnableGameObject)]
	public GameObject DecalForLampON;
#endregion

#region Change Brightness or Material
	[ConditionalField(nameof(animationType), false, AnimationType.ChangeMaterial, AnimationType.ChangeBrightness)]
	public int materialId;

	[ConditionalField(nameof(animationType), false, AnimationType.ChangeMaterial, AnimationType.ChangeBrightness)]
	public GameObject meshR;

#region ChangeMaterial
	[ConditionalField(nameof(animationType), false, AnimationType.ChangeMaterial)]
	public Material matOn;
#endregion

#region ChangeBrightness
	[ConditionalField(nameof(animationType), false, AnimationType.ChangeBrightness)]
	public int brightnessCoef = 300;
#endregion
#endregion
#endregion

#region internal variable
	private MeshRenderer mesh;
	private Color colorOff;
	private Color colorOn;
	private Material matOff;
#endregion

	public new void Start() {
		switch (animationType) {
			case AnimationType.EnableGameObject: break;
			case AnimationType.ChangeMaterial:
				mesh = meshR.GetComponent<MeshRenderer>();
				matOff = mesh.material;
				break;
			case AnimationType.ChangeBrightness:
				mesh = meshR.GetComponent<MeshRenderer>();
				colorOff = mesh.materials[materialId].color;
				var tempColor = colorOff;
				tempColor.r *= 1 + brightnessCoef / 100f;
				tempColor.g *= 1 + brightnessCoef / 100f;
				tempColor.b *= 1 + brightnessCoef / 100f;
				colorOn = tempColor;
				break;
			default: throw new ArgumentOutOfRangeException();
		}
		base.Start();
	}

	protected override void PlaySound(bool newState) { }

	protected override void Animate(bool newState) {
		switch (animationType) {
			case AnimationType.EnableGameObject:
				GameObjectOn(newState);
				return;
			case AnimationType.ChangeMaterial:
				ChangeMaterial(newState);
				return;
			case AnimationType.ChangeBrightness:
				ChangeBrightness(newState);
				return;
			default: return;
		}
	}

	private void GameObjectOn(bool newState) => DecalForLampON.SetActive(newState);
	private void ChangeMaterial(bool newState) => mesh.material = newState ? matOn : matOff;
	private void ChangeBrightness(bool newState) => mesh.materials[materialId].color = newState ? colorOn : colorOff;
}