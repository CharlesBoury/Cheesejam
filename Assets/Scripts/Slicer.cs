using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using EzySlice;

public class Slicer : MonoBehaviour
{
	public Material crossSectionMaterial;
	TextureRegion textureRegion;

	public float thrust = 100f;
	float timeToWait = 0;

	void Update()
	{
		timeToWait -= Time.deltaTime;
		timeToWait = Mathf.Max(0, timeToWait);
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
		if (timeToWait == 0)
		{
			Slice(other.gameObject);
			DisableSliceFor(2f);
		}
	}

	void DisableSliceFor(float amount)
	{
		timeToWait = Mathf.Max(timeToWait, amount);
	}
}
