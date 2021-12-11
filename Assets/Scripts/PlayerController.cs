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
	public float animationTime = 1.0f;
	private float curTime = 0.0f;
	public int cuttingState = 0;
	Vector2 moveBy;

	public AudioClip audioClip;

	static int id = 0;
	Vector3 startPos;

	void Start()
	{
		
	}

	public void OnEnable() {

		switch (id)
		{
			case 0:
				startPos = new Vector3(-0.25f, 0.15f, 0.2f);
				transform.Rotate(0f, 0.0f, 0.0f);
				break;
			case 1:
				startPos = new Vector3(0.25f, 0.15f, 0.2f);
				transform.Rotate(90f, 0.0f, 0.0f);
				break;
			case 2:
				startPos = new Vector3(-0.25f, 0.15f, -0.2f);
				transform.Rotate(-90f, 0.0f, 0.0f);
				break;
			case 3:
				startPos = new Vector3(0.25f, 0.15f, -0.2f);
				transform.Rotate(180f, 0.0f, 0.0f);
				break;
		}
		transform.position = startPos;
		id++;

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
		

		if(cuttingState > 0) {
			// Convert the X angle target into a quaternion: to maxAngle or initialAngle
			Quaternion target;
			if(cuttingState == 1){
				target = targetQuaternion;
				
			} else {
				target = initQuaternion;
			}

			// Dampen towards the target rotation
			curTime += Time.deltaTime;
			float t = curTime / animationTime;
			Quaternion deltaRotation = Quaternion.Slerp(transform.rotation, target, t); 
			GetComponent<Rigidbody>().MoveRotation(deltaRotation); 
			//Quaternion.AngleAxis (maxAngle, Vector3.up); 
			
			if(t > 1.0f) {
				curTime = 0.0f;
				if(cuttingState == 1) {
					AudioSource.PlayClipAtPoint(audioClip, transform.position, 1.0f);
				}
				cuttingState = (cuttingState + 1)%3;
			}
		}
	}
}
