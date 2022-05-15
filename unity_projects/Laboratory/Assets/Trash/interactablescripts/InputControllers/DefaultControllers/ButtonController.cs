using System;
using UnityEngine;

public class ButtonController : BaseTouchInputController<bool> {
	public enum AnimationType {
		States,
		Movement,
	}

#region constants
#endregion

#region inspector
	[Header("Западает ли кнопка?")] public bool isButtonHold;

	[Header("Добавить текст на кнопку?")] public bool hasText;
	[ConditionalField(nameof(hasText))] public GameObject textObject;
	[ConditionalField(nameof(hasText))] public Axis axisPushText;
	[ConditionalField(nameof(hasText))] public float heightPushText;

	[Header("Тип анимации")] public AnimationType animationType;

#region States
	[ConditionalField(nameof(animationType), false, AnimationType.States)]
	public RuntimeAnimatorController animatorController;
#endregion

#region Movement
	[ConditionalField(nameof(animationType), false, AnimationType.Movement)]
	public Axis axisPush;

	[ConditionalField(nameof(animationType), false, AnimationType.Movement)]
	public float heightPush;
#endregion
#endregion

#region internal variable
	private Vector3 startButtonPos;
	private Vector3 startTextPos;
#endregion

	protected new void Start() {
		ConfigureAnimator();
		startButtonPos = transform.localPosition;
		if (hasText) { startTextPos = textObject.transform.localPosition; }
		base.Start();
	}

	private void ConfigureAnimator() {
		switch (animationType) {
			case AnimationType.States:
				if (!TryGetComponent<Animator>(out _))
					gameObject.AddComponent<Animator>().runtimeAnimatorController = animatorController;
				break;
			case AnimationType.Movement:
				if (TryGetComponent<Animator>(out var animator)) animator.enabled = false;
				break;
			default: throw new ArgumentOutOfRangeException();
		}
	}

	protected override bool GetCurrentState(bool currentState, bool newState) { return newState; }

	public override void OnTouchStart(Vector2 position) {
		if (!isButtonHold) { ChangeState(true); } else { ChangeState(!CurrentState); }
	}

	public override void OnTouchDrug(Vector2 position) { }

	public override void OnTouchEnd(Vector2 position) {
		if (!isButtonHold) ChangeState(false);
	}

	protected override void PlaySound(bool newState) { }

	protected override void Animate(bool newState) {
		switch (animationType) {
			case AnimationType.States:
				StateAnimate(newState);
				return;
			case AnimationType.Movement:
				MovementAnimate(newState);
				return;
			default: return;
		}
	}

	private void StateAnimate(bool newState) {
		gameObject.GetComponent<Animator>().SetTrigger($"state_{(newState ? 1 : 0)}");
	}

	private void MovementAnimate(bool newState) {
		MovementObject(newState, transform, startButtonPos, axisPush, heightPush);
		if (hasText) { MovementObject(newState, textObject.transform, startTextPos, axisPushText, heightPushText); }
	}

	private Vector3 v3;

	private void MovementObject(bool state, Transform transform, Vector3 startPos, Axis axis, float length) {
		v3 = transform.localPosition;
		v3[(int) axis] = startPos[(int) axis] + (state ? length : 0);
		transform.localPosition = v3;
	}
}