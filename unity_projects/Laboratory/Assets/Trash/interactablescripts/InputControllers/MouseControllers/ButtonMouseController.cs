public class ButtonMouseController : ButtonController {
#region constants
#endregion

#region inspector
#endregion

#region internal variable
#endregion

	public new void Start() {
		BlockedByExternalCall = true;
		base.Start();
	}

	private void OnMouseDown() { SetState(!isButtonHold || !CurrentState); }

	private void OnMouseUp() { SetState(isButtonHold && CurrentState); }

	private void OnDisable() { BlockedByExternalCall = false; }
}