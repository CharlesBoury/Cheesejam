using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tourniquet : MonoBehaviour
{
	public float speed = 1f;
	Rigidbody rb;
	float innerTime;


	void Start()
	{
		rb = GetComponent<Rigidbody>();
	}

	void FixedUpdate()
	{
		if (GameManager.Instance.gameTime <= 0)
		{
			rb.angularVelocity = Vector3.zero;
		}
		else
		{
			innerTime += Time.fixedDeltaTime;
			int step = ((int)innerTime) / 20 + 1;
			Vector3 deltaRotation = new Vector3(0, 1, 0) * Time.fixedDeltaTime * speed * step * step;

			rb.angularVelocity = deltaRotation;
		}
	}
}
