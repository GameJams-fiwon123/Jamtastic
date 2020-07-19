using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {
	
	public PlayerController target;
	public float verticalOffset;
	public float lookAheadDstX;
	public float lookSmoothTimeX;
	public float verticalSmoothTime;
	public Vector2 focusAreaSize;
	public float leftBarrier, rightBarrier, bottomBarrier;

	private FocusArea focusArea;

	private float currentLookAheadX;
	private float targetLookAheadX;
	private float lookAheadDirX;
	private float smoothLookVelocityX;
	private float smoothVelocityY;

	private bool lookAheadStopped;

	void Start() {
		
		focusArea = new FocusArea (target.GetComponent<Collider2D>().bounds, focusAreaSize);

	}

	void LateUpdate() {

		focusArea.Update (target.GetComponent<Collider2D>().bounds);

		Vector2 focusPosition = focusArea.centre + Vector2.up * verticalOffset;

		if (focusArea.velocity.x != 0) {

			lookAheadDirX = Mathf.Sign (focusArea.velocity.x);

			if (Mathf.Sign(target.GetComponent<Rigidbody2D>().velocity.x) == Mathf.Sign(focusArea.velocity.x) && target.GetComponent<Rigidbody2D>().velocity.x != 0) {

				lookAheadStopped = false;
				targetLookAheadX = lookAheadDirX * lookAheadDstX;

			} else {

				if (!lookAheadStopped) {
					lookAheadStopped = true;

					targetLookAheadX = currentLookAheadX + (lookAheadDirX * lookAheadDstX - currentLookAheadX)/4f;

				}

			}

		}

		currentLookAheadX = Mathf.SmoothDamp (currentLookAheadX, targetLookAheadX, ref smoothLookVelocityX, lookSmoothTimeX);

		focusPosition.y = Mathf.SmoothDamp (transform.position.y, focusPosition.y, ref smoothVelocityY, verticalSmoothTime);
		focusPosition += Vector2.right * currentLookAheadX;

		if(focusPosition.x <= leftBarrier) {
			focusPosition.x = transform.position.x;
		}
		if(focusPosition.x >= rightBarrier) {
			focusPosition.x = transform.position.x;
		}
		if(focusPosition.y <= bottomBarrier) {
			focusPosition.y = transform.position.y;
		}

		transform.position = (Vector3)focusPosition + Vector3.forward * -10;

	}

	void OnDrawGizmos() {

		Gizmos.color = new Color (1, 0, 0, .5f);
		Gizmos.DrawCube (focusArea.centre, focusAreaSize);
		Gizmos.DrawIcon(new Vector3(leftBarrier,0,0),"leftBarrier",false);
		Gizmos.DrawIcon(new Vector3(rightBarrier,0,0),"leftBarrier",false);
		Gizmos.DrawIcon(new Vector3(0,bottomBarrier,0),"leftBarrier",false);

	}

	struct FocusArea {

		public Vector2 centre;
		public Vector2 velocity;
		float left,right;
		float top,bottom;

		public FocusArea(Bounds targetBounds, Vector2 size) {
			
			left = targetBounds.center.x - size.x/2;
			right = targetBounds.center.x + size.x/2;
			bottom = targetBounds.min.y;
			top = targetBounds.min.y + size.y;

			velocity = Vector2.zero;
			centre = new Vector2((left+right)/2,(top +bottom)/2);

		}

		public void Update(Bounds targetBounds) {

			float shiftX = 0;

			if (targetBounds.min.x < left) {
				shiftX = targetBounds.min.x - left;
			} else if (targetBounds.max.x > right) {
				shiftX = targetBounds.max.x - right;
			}

			left += shiftX;
			right += shiftX;

			float shiftY = 0;
			if (targetBounds.min.y < bottom) {
				shiftY = targetBounds.min.y - bottom;
			} else if (targetBounds.max.y > top) {
				shiftY = targetBounds.max.y - top;
			}

			top += shiftY;
			bottom += shiftY;
			centre = new Vector2((left+right)/2,(top +bottom)/2);
			velocity = new Vector2 (shiftX, shiftY);

		}

	}

}
