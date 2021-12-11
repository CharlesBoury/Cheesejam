using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
	public float moveSpeed = 0.3f;
	public float maxAngle = -40f;
	private Quaternion targetQuaternion;
	private Quaternion initQuaternion;
	public float cutSpeed = 100.0f;
	public int cuttingState = 0;
	Vector2 moveBy;

	public void OnEnable() {
		initQuaternion = transform.rotation;
		targetQuaternion = initQuaternion * Quaternion.AngleAxis (maxAngle, Vector3.up); 
	}

	public void OnPick()
	{
		moveBy = Vector2.zero;
	}

	public void OnMove(InputValue value)
	{
		moveBy = value.Get<Vector2>() * moveSpeed;
	}

	public void OnCut()
	{
		if(cuttingState == 0) {
			cuttingState = 1; // cutting down
		}
	}

	void Update()
	{
		transform.position = new Vector3(
			transform.position.x + moveBy.x,
			transform.position.y,
			transform.position.z + moveBy.y);

		if(cuttingState > 0) {
			// Convert the X angle target into a quaternion: to maxAngle or initialAngle
			Quaternion target;
			if(cuttingState == 1){
				target = targetQuaternion;
			} else {
				target = initQuaternion;
			}

			// Dampen towards the target rotation
			transform.rotation = Quaternion.RotateTowards(transform.rotation, target, Time.deltaTime * cutSpeed);
			float angle = Quaternion.Angle(transform.rotation, target);
			if(angle < 0.01f) {
				cuttingState = (cuttingState + 1)%3;
			}
		}
	}
}
