using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cheese : MonoBehaviour
{
	public float hardness = 1f;
	public float density = 1f;
	public bool cuttable = true;


	void Update()
	{
		if (transform.position.y < -2f)
		{
			Destroy(gameObject);
		}
	}
}
