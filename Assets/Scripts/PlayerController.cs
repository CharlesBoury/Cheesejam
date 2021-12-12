using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
	public float moveSpeed = 0.3f;
	public float maxAngle = 40f;
	public float animationTime = 1.0f;
	private float curTime = 0.0f;
	public float cutSpeed = 100.0f;
	public float pickSpeed = 100.0f;
	public int cuttingState = 0;
	public bool canMoveOnCut = false;

	public int pickingState = 0;
	public bool hasPicked = false;

	public AudioClip audioClip;

	private Slicer slicer;
	private Vector3 startPos;
	private Vector2 moveBy;
	private float playerHeight = 0.2f;
	private Vector3 positionWhenCut;
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
		id++;
	}

	public void OnPick()
	{
		if (pickingState == 0 && hasPicked == false)
		{
			pickingState = 1;
		}
		else if (hasPicked == true)
		{
			pickingState = 1;
			hasPicked = false;
		}
		foreach (Cheese child in GetComponentsInChildren<Cheese>())
		{
			child.transform.SetParent(null);
		}
			
	}

	public void OnMove(InputValue value)
	{
		moveBy = value.Get<Vector2>() * moveSpeed;
	}

	public void OnCut()
	{
		if(cuttingState == 0)
		{
			cuttingState = 1;
			AudioSource.PlayClipAtPoint(audioClip, transform.position, 1.0f);
			positionWhenCut = transform.position;
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

		// cutting
		if (cuttingState > 0) {
			// Dampen towards the target position
			Vector3 from;
			Vector3 to;
			curTime += Time.deltaTime;
			float t = curTime / animationTime;

			// cutting down
			if(cuttingState == 1)
			{
				slicer.isSharp = true;
				from = positionWhenCut;
				to = new Vector3(positionWhenCut.x, 0, positionWhenCut.z);
			}
			// goind back up
			else
			{
				slicer.isSharp = false;
				from = new Vector3(positionWhenCut.x, 0, positionWhenCut.z);
				to = positionWhenCut;
			}

			Vector3 targetPosition = Vector3.Lerp(from, to, t);
			GetComponent<Rigidbody>().position = targetPosition;

			if (t > 1.0f)
			{
				curTime = 0.0f;
				cuttingState = (cuttingState + 1)%3;
			}
		}

		if (pickingState > 0)
        {
			// Convert the X angle target into a quaternion: to maxAngle or initialAngle
			Quaternion target;
			if (pickingState == 1)
			{
				target = targetQuaternion;

			}
			else
			{
				target = initQuaternion;
			}

			// Dampen towards the target rotation
			curTime += Time.deltaTime;
			float t = curTime / animationTime;
			Quaternion deltaRotation = Quaternion.Slerp(transform.rotation, target, t);
			GetComponent<Rigidbody>().MoveRotation(deltaRotation);
			//Quaternion.AngleAxis (maxAngle, Vector3.up); 

			if (t > 1.0f)
			{
				curTime = 0.0f;
				if (pickingState == 1)
				{
					AudioSource.PlayClipAtPoint(audioClip, transform.position, 1.0f);
				}
				pickingState = (pickingState + 1) % 3;
			}
		}
	}
}
