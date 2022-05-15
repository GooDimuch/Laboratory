using UnityEngine;

public interface ITouchListener {
	bool BlockedTouch { get; }

	void OnTouchStart(Vector2 position, Camera camera);

	void OnTouchDrag(Vector2 position, Camera camera); //activate only after OnTouchStart

	void OnTouchEnd(Vector2 position, Camera camera); //activate only after OnTouchStart
}