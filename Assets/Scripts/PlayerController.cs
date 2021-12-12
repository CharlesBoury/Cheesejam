using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public enum State
{
	Moving,
	Cutting_Down,
	Cutting_Up,
	Picking_Down,
	Picking_Up,
	Holding,
};

public class PlayerController : MonoBehaviour
{
	public float moveSpeed = 0.3f;
	public float maxAngle = 40f;
	public float animationTime = 1.0f;
	private float curTime = 0.0f;
	public float cutSpeed = 100.0f;
	public float pickSpeed = 100.0f;
	public float baseThreshold = 0.4f;
	public float timer = 0f;
	public bool canMoveOnCut = false;

	public List<AudioClip> woodSounds = new List<AudioClip>();

	private Slicer slicer;
	private Vector3 startPos;
	private Vector3 basePos;
	private Vector2 moveByEachFrame;
	private float playerHeight = 0.2f;
	private Vector3 positionWhenCut;
	private Vector3 positionWhenPick;
	static int id = 0;

	public State state;


	public void OnEnable()
	{
		state = State.Moving;
		slicer = GetComponentInChildren<Slicer>();

		switch (id)
		{
			case 0:
				startPos = new Vector3(-0.25f, playerHeight, 0.2f);
				basePos = new Vector3(-0.5f, playerHeight, 0.5f);
				transform.Rotate(0f, 135f, 0f);
				break;
			case 1:
				startPos = new Vector3(0.25f, playerHeight, 0.2f);
				basePos = new Vector3(0.5f, playerHeight, 0.5f);
				transform.Rotate(0f, -135f, 0f);
				break;
			case 2:
				startPos = new Vector3(-0.25f, playerHeight, -0.2f);
				basePos = new Vector3(-0.5f, playerHeight, -0.5f);
				transform.Rotate(0f, 45f, 0f);
				break;
			case 3:
				startPos = new Vector3(0.25f, playerHeight, -0.2f);
				basePos = new Vector3(0.5f, playerHeight, -0.5f);
				transform.Rotate(0f, -45f, 0f);
				break;
		}
		transform.position = startPos;
		id++;
	}

	public void OnPick()
	{
		if (state == State.Moving && timer <= 0.01f)
		{
			state = State.Picking_Down;
			positionWhenPick = transform.position;
		}
		else if (state == State.Holding)
		{
			state = State.Picking_Up;
			timer = Mathf.Max(timer, 0.5f);
			positionWhenPick = transform.position;
			foreach (Cheese child in GetComponentsInChildren<Cheese>())
			{
				float distToBase = Vector3.Distance(child.transform.position, basePos); 
				if(distToBase < baseThreshold) {
					GameManager.Instance.AddScore(1, id);
					Rigidbody rb = child.gameObject.GetComponent<Rigidbody>();
					// freeze position but not rotation
					rb.constraints = RigidbodyConstraints.FreezePosition;
					child.cuttable = false;
					child.pickable = false;
				} else {
					// release the beast
					Rigidbody rb = child.gameObject.GetComponent<Rigidbody>();
					rb.constraints = RigidbodyConstraints.None;
				}
				child.transform.SetParent(null);
			}
		}

	}

	public void OnMove(InputValue value)
	{
		moveByEachFrame = value.Get<Vector2>() * Time.deltaTime * moveSpeed;
	}

	public void OnCut()
	{
		if(state == State.Moving)
		{
			state = State.Cutting_Down;
			positionWhenCut = transform.position;
		}
	}


	void updatePos(bool clamped)
	{
		float radius = 0.1f;
		Vector3 newPos = new Vector3(
			transform.position.x + moveByEachFrame.x,
			transform.position.y,
			transform.position.z + moveByEachFrame.y);

		Vector3 vecFromBase = newPos - startPos;
		if(clamped) {
			vecFromBase = Vector3.ClampMagnitude(vecFromBase, radius);
		}
		transform.position = startPos + vecFromBase;
	}

	void Update()
	{
		timer -= Time.deltaTime;
		timer = Mathf.Max(0, timer);

		if (state == State.Moving || state == State.Holding || canMoveOnCut)
		{
			updatePos(state != State.Holding);
		}

		if (state == State.Cutting_Down || state == State.Cutting_Up)
		{
			// Dampen towards the target position
			Vector3 from;
			Vector3 to;
			curTime += Time.deltaTime;
			float t = curTime / animationTime;

			if(state == State.Cutting_Down)
			{
				slicer.isSharp = true;
				from = positionWhenCut;
				to = new Vector3(positionWhenCut.x, 0, positionWhenCut.z);
				Vector3 targetPosition = Vector3.Lerp(from, to, t);
				GetComponent<Rigidbody>().position = targetPosition;
			}
			else if (state == State.Cutting_Up)
			{
				slicer.isSharp = false;
				from = new Vector3(positionWhenCut.x, 0, positionWhenCut.z);
				to = positionWhenCut;
				Vector3 targetPosition = Vector3.Lerp(from, to, t);
				GetComponent<Rigidbody>().position = targetPosition;
			}


			if (t > 1.0f)
			{
				curTime = 0.0f;

				if (state == State.Cutting_Down)
				{
					state = State.Cutting_Up;
					if (slicer.hasSliced == false)
					{
						int index = Random.Range(0, woodSounds.Count);
						AudioSource.PlayClipAtPoint(woodSounds[index], transform.position, 1.0f);
					}
				}
				else if (state == State.Cutting_Up)
				{
					state = State.Moving;
					slicer.hasSliced = false;
				}
			}
		}

		if (state == State.Picking_Down || state == State.Picking_Up)
		{
			Vector3 from;
			Vector3 to;
			curTime += Time.deltaTime;
			float t = curTime / animationTime;

			if (state == State.Picking_Down)
			{
				from = positionWhenPick;
				to = new Vector3(positionWhenPick.x, 0, positionWhenPick.z);

				Vector3 targetPosition = Vector3.Lerp(from, to, t);
				GetComponent<Rigidbody>().position = targetPosition;
			}
			else if (state == State.Picking_Up)
			{
				from = new Vector3(positionWhenPick.x, 0, positionWhenPick.z);
				to = positionWhenPick;

				Vector3 targetPosition = Vector3.Lerp(from, to, t);
				GetComponent<Rigidbody>().position = targetPosition;
			}

			if (t > 1.0f)
			{
				curTime = 0.0f;

				if (state == State.Picking_Down)
				{
					state = State.Picking_Up;
				}
				else if (state == State.Picking_Up)
				{
					state = State.Moving;
				}
			}
		}
	}
}
