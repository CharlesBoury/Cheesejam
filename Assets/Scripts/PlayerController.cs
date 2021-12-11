using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
	public float moveSpeed = 0.3f;

	public void OnPick()
	{
	}

	public void OnMove(InputValue value)
	{
		Vector2 moveBy = value.Get<Vector2>() * moveSpeed;

		transform.position = new Vector3(
			transform.position.x + moveBy.x,
			transform.position.y,
			transform.position.z + moveBy.y);
	}

	public void OnCut()
	{
		Debug.Log(Gamepad.current);
	}
}
