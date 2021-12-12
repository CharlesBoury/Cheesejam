using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tourniquet : MonoBehaviour
{
	public float speed = 1f;
	Rigidbody rb;
	void Start()
	{
		rb = GetComponent<Rigidbody>();
	}

	void FixedUpdate()
	{
		int step = ((int)Time.time) / 10 + 1;
		Vector3 deltaRotation = new Vector3(0, 1, 0) * Time.fixedDeltaTime * speed * step * step;

		rb.angularVelocity = deltaRotation;
	}
}
