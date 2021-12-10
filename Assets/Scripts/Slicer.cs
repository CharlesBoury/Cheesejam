using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using EzySlice;

// ref:
// https://github.com/DavidArayan/ezy-slice

public class Slicer : MonoBehaviour
{
	public Material crossSectionMaterial;
	TextureRegion textureRegion;

	public float thrust = 100f;
	public float waitingTime = 0.5f;
	float timer = 0;

	void Update()
	{
		timer -= Time.deltaTime;
		timer = Mathf.Max(0, timer);
	}


	void Slice(GameObject go)
	{
		textureRegion = crossSectionMaterial.GetTextureRegion(0, 0, 100, 100);
		GameObject[] slices;

		slices = go.SliceInstantiate(transform.position, transform.up, textureRegion, crossSectionMaterial);

		foreach(GameObject slice in slices)
		{
			slice.AddComponent<MeshCollider>();
			slice.GetComponent<MeshCollider>().convex = true;

			slice.AddComponent<Rigidbody>();
			slice.GetComponent<Rigidbody>().AddForce(transform.up * thrust * (Random.value > 0.5f ? 1f : -1f));
		}
			Destroy(go);
	}


	private void OnTriggerEnter(Collider other)
	{
		if (timer == 0)
		{
			Slice(other.gameObject);
			DisableSliceFor(waitingTime);
		}
	}

	void DisableSliceFor(float amount)
	{
		timer = Mathf.Max(timer, amount);
	}
}
