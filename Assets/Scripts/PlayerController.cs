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
	static int idCounter = 0;

	
	public float maxAngle = 40f;
	public float animationTime = 1.0f;
	private float curTime = 0.0f;
	public float cutSpeed = 100.0f;
	public float pickSpeed = 100.0f;
	public float timer = 0f;
	public bool canMoveOnCut = false;
	public State state;
	public int id = 0;
	private float defaultMoveSpeed = 0.4f;
	private float minMoveSpeed = 0.1f;
	private float moveSpeed;

	public Object assiette;

	public List<AudioClip> woodSounds = new List<AudioClip>();

	private Slicer slicer;
	private Vector3 startPos;
	private Vector2 basePos;
	private Vector2 moveByEachFrame;
	private float playerHeight = 0.2f;
	private Vector3 positionWhenCut;
	private Vector3 positionWhenPick;
	private float assietteCoordonnee = 0.36f;
	private float baseThreshold = 0.2f;

	public void OnEnable()
	{
		id = idCounter;
		state = State.Moving;
		slicer = GetComponentInChildren<Slicer>();
		moveSpeed = defaultMoveSpeed;
		switch (id)
		{
			case 0:
				startPos = new Vector3(-0.25f, playerHeight, 0.2f);
				basePos = new Vector2(-assietteCoordonnee, assietteCoordonnee);
				transform.Rotate(0f, 135f, 0f);
				break;
			case 1:
				startPos = new Vector3(0.25f, playerHeight, 0.2f);
				basePos = new Vector2(assietteCoordonnee, assietteCoordonnee);
				transform.Rotate(0f, -135f, 0f);
				break;
			case 2:
				startPos = new Vector3(-0.25f, playerHeight, -0.2f);
				basePos = new Vector2(-assietteCoordonnee, -assietteCoordonnee);
				transform.Rotate(0f, 45f, 0f);
				break;
			case 3:
				startPos = new Vector3(0.25f, playerHeight, -0.2f);
				basePos = new Vector2(assietteCoordonnee, -assietteCoordonnee);
				transform.Rotate(0f, -45f, 0f);
				break;
		}
		transform.position = startPos;
		Vector3 positionAssiette = new Vector3(basePos.x, 0, basePos.y);
		Instantiate(assiette, positionAssiette, Quaternion.identity);
		idCounter++;
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
				Vector2 pos2D = new Vector2(child.transform.position.x, child.transform.position.z);
				float distToBase = Vector2.Distance(pos2D, basePos);

				if(distToBase < baseThreshold)
				{
					GameManager.Instance.AddScore(1, id);
					Rigidbody rb = child.gameObject.GetComponent<Rigidbody>();
					// freeze position but not rotation
					rb.constraints = RigidbodyConstraints.FreezePosition;
					child.cuttable = false;
					child.pickable = false;

					CheeseSpawner cheeseSpawner = GameManager.Instance.GetComponent<CheeseSpawner>();
					cheeseSpawner.OnRemoveCheese(child.gameObject);
				} else {
					// release the beast
					Rigidbody rb = child.gameObject.GetComponent<Rigidbody>();
					rb.constraints = RigidbodyConstraints.None;
				}
				child.transform.SetParent(null);
			}
			moveSpeed = defaultMoveSpeed;
		}

	}

	public void OnMove(InputValue value)
	{
		moveByEachFrame = value.Get<Vector2>() * Time.deltaTime * moveSpeed;
	}

	public void OnCut()
	{
		if (state == State.Moving)
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

	public void ReduceSpeedFromCheese(Cheese cheese) {
		float volume = Utils.GetVolume(cheese.gameObject);
		moveSpeed = Mathf.Max(minMoveSpeed, ((cheese.maxPickableVolume - volume) / cheese.maxPickableVolume) * defaultMoveSpeed);
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
