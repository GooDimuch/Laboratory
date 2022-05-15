using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public abstract class BaseInputController<T> : BaseControlController<T> {
#region constants
#endregion

#region inspector
#endregion

#region internal variable
	private State<T> state;
	protected bool BlockedByExternalCall { get; set; }
#endregion

	private new void Awake() { base.Awake(); }

	protected new void Start() {
		state = gameObject.GetComponent<State<T>>();
		state.Mode = State<T>.WorkMode.Input;
		base.Start();
	}

	protected void ChangeState(T potential) => state.CurrentState = GetCurrentState(CurrentState, potential);

	protected void SetState(T value) => state.CurrentState = value;

	public void BlockAndChangeState(T potential) {
		BlockedByExternalCall = true;
		ChangeState(potential);
	}

	public void BlockAndSetState(T value) {
		BlockedByExternalCall = true;
		SetState(value);
	}

	protected abstract T GetCurrentState(T currentState, T shift);

	public void ReleaseBlockByExternalCall() { BlockedByExternalCall = false; }
}

public abstract class BaseTouchInputController<T> : BaseInputController<T>, ITouchListener {
	public bool BlockedTouch => BlockedByExternalCall;
	public void OnTouchStart(Vector2 position, Camera camera) { throw new System.NotImplementedException(); }
	public void OnTouchDrag(Vector2 position, Camera camera) { throw new System.NotImplementedException(); }
	public void OnTouchEnd(Vector2 position, Camera camera) { throw new System.NotImplementedException(); }
	public abstract void OnTouchStart(Vector2 position);
	public abstract void OnTouchDrug(Vector2 position);
	public abstract void OnTouchEnd(Vector2 position);
}