using System;
using System.Collections.Generic;
using UnityEngine;

public class RotatorController : BaseTouchInputController<float> {
	public enum AnimationType {
		States,
		LegacyAnimations,
		Rotation,
	}

#region constants
	private const int MAX_STATES_FOR_STATE_ROTATION = 20;
#endregion

#region inspector
	[Header("Конвертировать к [0..1]")] public bool convertTo01;
	[Header("Инвертировать направление вращения")] public bool invertRotation;
	[Header("Инвертированная анимация")] public bool invertAnimation;
	[Header("Переход через 0")] public bool circular;
	[Header("Длина отступа для изменения состояния")] public float offsetLength = 30f;
	[Header("Зона захвата значения")] public float changeStateZone = 0.7f;
	[Header("Тип анимации")] public AnimationType animationType;

#region States
	[ConditionalField(nameof(animationType), false, AnimationType.States)]
	public List<string> triggers = new List<string>();
#endregion

#region LegacyAnimations
	[ConditionalField(nameof(animationType), false, AnimationType.LegacyAnimations)]
	[Header("Clips must be legacy")]
	public List<AnimationClip> animations;
#endregion

#region Rotation
	[ConditionalField(nameof(animationType), false, AnimationType.Rotation)]
	public Axis axisOfRotation;

	[ConditionalField(nameof(animationType), false, AnimationType.Rotation)]
	public float startAngle;

	[ConditionalField(nameof(animationType), false, AnimationType.Rotation)]
	public float step;

	[ConditionalField(nameof(animationType), false, AnimationType.Rotation)]
	public int numberStates;
#endregion
#endregion

#region internal variable
	[ReadOnly] [SerializeField] [Header("")] protected int nStates;
	[ReadOnly] [SerializeField] protected float stepForSpecificRotationType;

	private Animator animator;
	private new Animation animation;
	private Vector2 beginDragPosition;
	private Vector2 startDirection;
	private float deviationAngle;
#endregion

	public new void Start() {
		ConfigureAnimator();
		step = Mathf.Abs(step);
		offsetLength = Mathf.Abs(offsetLength);
		changeStateZone = Mathf.Abs(changeStateZone);
		if (circular) { step = 360f / nStates; }
		stepForSpecificRotationType = GetStep();
		base.Start();
	}

	private void ConfigureAnimator() {
		switch (animationType) {
			case AnimationType.States:
				if (TryGetComponent(out animator)) animator.enabled = true;
				break;
			case AnimationType.LegacyAnimations:
				if (!TryGetComponent<Animator>(out _)) {
					animation = gameObject.AddComponent<Animation>();
					animations.ForEach(clip => { animation.AddClip(clip, clip.name); });
				}
				nStates = animations.Count;
				break;
			case AnimationType.Rotation:
				if (TryGetComponent(out animator)) animator.enabled = false;
				nStates = Mathf.Abs(numberStates);
				break;
			default: throw new ArgumentOutOfRangeException();
		}
	}

	private float GetStep() {
		bool invertForward;
		switch (axisOfRotation) {
			case Axis.X:
				invertForward = transform.right != Vector3.right;
				break;
			case Axis.Y:
				invertForward = transform.up == Vector3.forward;
				break;
			case Axis.Z:
				invertForward = transform.forward == Vector3.forward;
				break;
			default: throw new ArgumentOutOfRangeException();
		}
		if (invertForward != invertRotation != invertAnimation) { return -step; }
		return step;
	}

	protected override float GetCurrentState(float currentState, float potential) {
		if (convertTo01) { currentState *= numberStates; }
		if (invertRotation) { potential *= -1; }
		currentState += potential;

		if (circular) {
			if (currentState > nStates - 1) currentState -= nStates;
			if (currentState < 0) currentState += nStates;
		} else { currentState = Mathf.Clamp(currentState, 0, nStates - 1); }
		if (convertTo01) { currentState /= numberStates; }
		return currentState;
	}

	public override void OnTouchStart(Vector2 position) {
		beginDragPosition = position;
		stepForSpecificRotationType = GetStep();
	}

	public override void OnTouchDrug(Vector2 position) {
		startDirection = GetStartDirection(axisOfRotation);
		var touchDirection = position - beginDragPosition;

		if (touchDirection.magnitude < offsetLength) return;
		deviationAngle = Vector2.SignedAngle(touchDirection, startDirection);

		var potential = GetPotential(deviationAngle);
		if (Math.Abs(potential) < float.Epsilon) return;

		ChangeState(potential);
	}

	private float GetPotential(float angle) {
		if ((int) (angle / (step * changeStateZone)) == 0) return 0;
		if (nStates < MAX_STATES_FOR_STATE_ROTATION) return (int) Mathf.Sign(angle);
		return Mathf.Clamp(Mathf.Abs(angle), 1, nStates / 10f) * Mathf.Sign(angle);
	}

	private Vector2 GetStartDirection(Axis axis) {
		switch (axis) {
			case Axis.X:
				Debug.LogError("Not supported(");
				return invertAnimation
						? new Vector2(-transform.up.z, transform.up.y)
						: new Vector2(transform.up.z, transform.up.y);
			case Axis.Y:
				return invertAnimation
						? new Vector2(transform.forward.x, -transform.forward.y)
						: new Vector2(-transform.forward.x, -transform.forward.y);
			case Axis.Z: return invertAnimation ? Vector3.Scale(transform.up, new Vector3(-1, 1, 1)) : transform.up;
			default: return default;
		}
	}

	public override void OnTouchEnd(Vector2 position) { }

	protected override void PlaySound(float newState) { }

	protected override void Animate(float newState) {
		if (convertTo01) { newState *= numberStates; }
		switch (animationType) {
			case AnimationType.States:
				StateAnimate(newState);
				return;
			case AnimationType.LegacyAnimations:
				LegacyAnimationsAnimate(newState);
				return;
			case AnimationType.Rotation:
				RotateAnimate(newState);
				return;
			default: return;
		}
	}

	private void StateAnimate(float newState) { animator.SetTrigger(triggers[(int) newState]); }

	private void LegacyAnimationsAnimate(float newState) { animation.Play(animations[(int) newState].name); }

	private void RotateAnimate(float newState) {
		var newAngle = startAngle + newState * stepForSpecificRotationType;
		var eulerAngles = transform.localRotation.eulerAngles;
		eulerAngles[(int) axisOfRotation] = newAngle;
		transform.localRotation = Quaternion.Euler(eulerAngles);
	}
}