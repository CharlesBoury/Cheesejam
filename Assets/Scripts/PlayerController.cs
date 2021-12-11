using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
	public float moveSpeed = 0.3f;
	public float maxAngle = 40f;
	public float animationTime = 1.0f;
	public int cuttingState = 0;
	public bool canMoveOnCut = false;

	public AudioClip audioClip;

	private Slicer slicer;
	private Quaternion targetQuaternion;
	private Quaternion initQuaternion;
	private Vector3 startPos;
	private Vector2 moveBy;
	private float curTime = 0.0f;
	private float playerHeight = 0.2f;
	static int id = 0;

	public void OnEnable() {
		slicer = GetComponentInChildren<Slicer>();

		switch (id)
		{
			case 0:
				startPos = new Vector3(-0.25f, playerHeight, 0.2f);
				transform.Rotate(0f, 135f, 0f);
				break;
			case 1:
				startPos = new Vector3(0.25f, playerHeight, 0.2f);
				transform.Rotate(0f, -135f, 0f);
				break;
			case 2:
				startPos = new Vector3(-0.25f, playerHeight, -0.2f);
				transform.Rotate(0f, 45f, 0f);
				break;
			case 3:
				startPos = new Vector3(0.25f, playerHeight, -0.2f);
				transform.Rotate(0f, -45f, 0f);
				break;
		}
		transform.position = startPos;

		initQuaternion = transform.rotation;
		targetQuaternion = initQuaternion * Quaternion.AngleAxis (maxAngle, new Vector3(1,0,0));

		id++;
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
		if(cuttingState == 0)
		{
			cuttingState = 1; // .
		}
	}


	void updatePos() {
		float radius = 0.1f;	
		Vector3 newPos = new Vector3(
			transform.position.x + moveBy.x,
			transform.position.y,
			transform.position.z + moveBy.y);

		float distance = Vector3.Distance(newPos, startPos);
		if (distance <= radius)
		{
			//newPos *= radius / distance;
			transform.position = newPos;
		}
	}

	void Update()
	{
		if (cuttingState == 0 || canMoveOnCut)
			updatePos();
		if (cuttingState > 0) {
			// Convert the X angle target into a quaternion: to maxAngle or initialAngle
			Quaternion from;
			Quaternion to;
			// Dampen towards the target rotation
			curTime += Time.deltaTime;
			float t = curTime / animationTime;


			// cutting down
			if(cuttingState == 1)
			{
				slicer.isSharp = true;
				from = initQuaternion;
				to = targetQuaternion;
			}
			// goind back up
			else
			{
				slicer.isSharp = false;
				from = targetQuaternion;
				to = initQuaternion;
			}

			Quaternion deltaRotation = Quaternion.Slerp(from, to, t);
			GetComponent<Rigidbody>().MoveRotation(deltaRotation);

			if (t > 1.0f)
			{
				curTime = 0.0f;
				if(cuttingState == 1)
				{
					AudioSource.PlayClipAtPoint(audioClip, transform.position, 1.0f);
				}
				cuttingState = (cuttingState + 1)%3;
			}
		}
	}
}
