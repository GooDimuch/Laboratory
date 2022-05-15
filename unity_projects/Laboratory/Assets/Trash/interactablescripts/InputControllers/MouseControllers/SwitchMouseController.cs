using UnityEngine;

public class SwitchMouseController : SwitchController {
#region constants
	private const float DELAY_TIME = 100f;
#endregion

#region inspector
#endregion

#region internal variable
	private bool isOver;
	private bool pressed;
	private bool lmbPressed;
	private bool rmbPressed;
	private bool lmbIsPressed;
	private bool rmbIsPressed;
	private float delay;
#endregion

	public new void Awake() { base.Awake(); }

	public new void Start() {
		BlockedByExternalCall = true;
		base.Start();
	}

	public void Update() {
		if (!isOver && !pressed) {
			if (isReturn) { ReturnSwitch(); }
			return;
		}
		lmbPressed = Input.GetMouseButton(0);
		rmbPressed = Input.GetMouseButton(1);
		lmbIsPressed = Input.GetMouseButtonDown(0);
		rmbIsPressed = Input.GetMouseButtonDown(1);
		if (lmbIsPressed || rmbIsPressed) {
			pressed = true;
			delay = 0;
			var shift = lmbIsPressed ? 1 : (rmbIsPressed ? -1 : 0);
			ChangeState(shift);
		} else if (!lmbPressed && !rmbPressed && pressed) {
			delay += Time.deltaTime * 1000f;
			if (delay < DELAY_TIME) { return; }
			pressed = false;
			if (!isReturn) { return; }
			ReturnSwitch();
		}
		ChangeState(0);
	}

	private void ReturnSwitch() {
		var shift = (numberStates == 2 ? (int) startState / 2 : (int) startState) - CurrentState;
		ChangeState(shift);
	}

	private void OnMouseOver() { isOver = true; }

	private void OnMouseExit() { isOver = false; }

	private void OnDisable() { BlockedByExternalCall = false; }
}