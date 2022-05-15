using TMPro;
using UnityEngine;

[HelpURL("https://docs.microsoft.com/en-us/dotnet/api/system.double.tostring?view=netframework-4.8")]
public class DigitalController : BaseOutputController<float> {
#region constants
#endregion

#region inspector
	public bool castToInt;

	public bool specificFormat;

	[Header("See examples of formats in helpURL"), ConditionalField(nameof(specificFormat))]
	public string format;

	[Header("[n] is replaced by the value"), ConditionalField(nameof(specificFormat))]
	public string template = "[n]";
#endregion

#region internal variable
	private TextMeshPro textMesh;
#endregion

	private new void Start() {
		if (!TryGetComponent(out textMesh)) { Debug.LogError("TextMeshPro not found!"); }
		base.Start();
	}

	private void Update() { }

	protected override void PlaySound(float newState) { }

	protected override void Animate(float newState) {
		textMesh.text = template.Replace("[n]", (castToInt ? (int) newState : newState).ToString(format));
	}
}