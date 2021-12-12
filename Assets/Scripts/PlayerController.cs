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
	public int pickingStateSave = 0;
	public bool hasPicked = false;

	public AudioClip audioClip;

	private Slicer slicer;
	private Vector3 startPos;
	private Vector2 moveBy;
	private float playerHeight = 0.2f;
	private Vector3 positionWhenCut;
	private Vector3 positionWhenPick;
	static int id = 0;

	public float timer = 0f;

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
		if (pickingState == 0 && hasPicked == false && timer <= 0.01f)
		{
			pickingState = 1;
			positionWhenPick = transform.position;
		}
		else if (hasPicked == true)
		{
			pickingState = pickingStateSave;
			hasPicked = false;
			timer = Mathf.Max(timer, 0.5f);
			foreach (Cheese child in GetComponentsInChildren<Cheese>())
			{
				child.transform.SetParent(null);
			}
		}
			
	}

	public void OnMove(InputValue value)
	{
		moveBy = value.Get<Vector2>() * Time.deltaTime * moveSpeed;
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

		Vector3 vecFromBase = newPos - startPos;
		vecFromBase = Vector3.ClampMagnitude(vecFromBase, radius);
		transform.position = startPos + vecFromBase;
	}

	void Update()
	{
		timer -= Time.deltaTime;
		timer = Mathf.Max(0, timer);

		//if (cuttingState == 0 || canMoveOnCut)
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
			Vector3 from;
			Vector3 to;
			curTime += Time.deltaTime;
			float t = curTime / animationTime;

			// picking down
			if (pickingState == 1)
			{
				from = positionWhenPick;
				to = new Vector3(positionWhenPick.x, 0, positionWhenPick.z);
			}
			// goind back up
			else
			{
				from = new Vector3(positionWhenPick.x, 0, positionWhenPick.z);
				to = positionWhenPick;
			}

			Vector3 targetPosition = Vector3.Lerp(from, to, t);
			GetComponent<Rigidbody>().position = targetPosition;

			if (t > 1.0f)
			{
				curTime = 0.0f;
				pickingState = (pickingState + 1) % 3;
			}
		}
	}
}
