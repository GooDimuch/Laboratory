using System;
using System.Collections.Generic;
using UnityEngine;

public class SwitchController : BaseTouchInputController<int> {
	public enum AnimationType {
		States,
		Rotation,
	}

	public enum Trigger {
		Down,
		Mid,
		UP,
	}

	public enum Direction {
		Up,
		Right,
		Down,
		Left,
		Custom,
	}

#region constants
#endregion

#region inspector
	[Header("Возвращается в стартовое положение?")] public bool isReturn;

	[ConditionalField(nameof(isReturn))] public Trigger startState;

	[Header("Направление для увеличения значения State")] public Direction direction;

	[ConditionalField(nameof(direction), false, Direction.Custom)]
	public Vector2 customDirection;

	[Header("Длина свайпа для изменения состояния")] public float swipeLength = 30f;
	[Header("Тип анимации")] public AnimationType animationType;

	[ConditionalField(nameof(animationType), false, AnimationType.Rotation)]
	public int numberStates;

#region States
	[ConditionalField(nameof(animationType), false, AnimationType.States)]
	public RuntimeAnimatorController animatorController;

	[ConditionalField(nameof(animationType), false, AnimationType.States)]
	public List<string> triggers = new List<string>();
#endregion

#region Rotation
	[ConditionalField(nameof(animationType), false, AnimationType.Rotation)]
	public Axis axisOfRotation;

	[ConditionalField(nameof(animationType), false, AnimationType.Rotation)]
	public float startAngle;

	[ConditionalField(nameof(animationType), false, AnimationType.Rotation)]
	public float step;
#endregion
#endregion

#region internal variable
	private Animator animator;
	private bool startingDrag;
	private Vector2 beginDragPosition;
#endregion

	public new void Start() {
		ConfigureAnimator();
		base.Start();
	}

	private void ConfigureAnimator() {
		switch (animationType) {
			case AnimationType.States:
				Animator _;
				if (!TryGetComponent<Animator>(out _)) {
					animator = gameObject.AddComponent<Animator>();
					animator.runtimeAnimatorController = animatorController;
				}
				break;
			case AnimationType.Rotation:
				if (TryGetComponent(out animator)) { animator.enabled = false; }
				break;
			default: throw new ArgumentOutOfRangeException();
		}
	}

	protected override int GetCurrentState(int currentState, int potential) {
		currentState += potential;
		return Mathf.Clamp(currentState, 0, numberStates - 1);
	}

	public override void OnTouchStart(Vector2 position) {
		beginDragPosition = position;
		startingDrag = true;
	}

	public override void OnTouchDrug(Vector2 position) {
		if (startingDrag) {
			beginDragPosition = position;
			startingDrag = false;
		}
		if (Mathf.Abs(getProjectionOnDirection(position - beginDragPosition)) < swipeLength) { return; }
		ChangeState((int) Mathf.Sign(getProjectionOnDirection(position - beginDragPosition)));
		beginDragPosition = position;
	}

	public override void OnTouchEnd(Vector2 position) {
		if (isReturn) { SetState((int) startState); }
	}

	private float getProjectionOnDirection(Vector2 dragVector) {
		switch (direction) {
			case Direction.Up: return dragVector.y;
			case Direction.Right: return dragVector.x;
			case Direction.Down: return -dragVector.y;
			case Direction.Left: return -dragVector.x;
			case Direction.Custom: return Vector3.Dot(customDirection.normalized, dragVector);
			default: throw new ArgumentOutOfRangeException();
		}
	}

	protected override void PlaySound(int newState) { }

	protected override void Animate(int newState) {
		switch (animationType) {
			case AnimationType.States:
				StateAnimate(newState);
				return;
			case AnimationType.Rotation:
				RotateAnimate(newState);
				return;
			default: return;
		}
	}

	private void StateAnimate(int newState) { animator.SetTrigger(triggers[newState]); }

	private void RotateAnimate(int newState) {
		var newAngle = startAngle + newState * step;
		var eulerAngles = transform.localRotation.eulerAngles;
		eulerAngles[(int) axisOfRotation] = newAngle;
		transform.localRotation = Quaternion.Euler(eulerAngles);
	}
}