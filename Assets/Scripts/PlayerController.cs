using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
	public float moveSpeed = 0.3f;

	Vector2 moveBy;

	void OnEnable()
	{
		moveBy = Vector2.zero;
	}

	void Update()
	{
		transform.position = new Vector3(
			transform.position.x + moveBy.x,
			transform.position.y,
			transform.position.z + moveBy.y);
	}

	public void OnPick()
	{
	}

	public void OnMove(InputValue value)
	{
		moveBy = value.Get<Vector2>() * moveSpeed;
	}

	public void OnCut()
	{
		Debug.Log(Gamepad.current);
	}
}
