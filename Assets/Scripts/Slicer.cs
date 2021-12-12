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
	public float minCuttableVolume = 10f; // in cm3
	public float minMass = 0.02f;
	public bool isSharp = true;
	float timer = 0;

	void Update()
	{
		timer -= Time.deltaTime;
		timer = Mathf.Max(0, timer);
	}

	void SliceThing(GameObject go)
	{
		textureRegion = crossSectionMaterial.GetTextureRegion(0, 0, 100, 100);
		GameObject[] slices;

		slices = go.SliceInstantiate(transform.position, transform.up, textureRegion, crossSectionMaterial);

		Cheese originalCheese = go.GetComponent<Cheese>();
		float sign=1.0f;
		foreach(GameObject slice in slices)
		{
			slice.AddComponent<MeshCollider>();
			slice.GetComponent<MeshCollider>().convex = true;

			slice.AddComponent<Rigidbody>();
			Rigidbody rb = slice.GetComponent<Rigidbody>();
			float volume = getVolume(slice);
			rb.mass = volume * originalCheese.density / 1000.0f;
			if(rb.mass < minMass) {
				rb.mass = minMass;
			}
			
			slice.AddComponent<Cheese>();

			updateNewCheese(slice.GetComponent<Cheese>(), originalCheese, volume);
			rb.AddForce(transform.up * thrust * sign);
			sign = -1.0f;
		}
		Destroy(go);
	}

	private void updateNewCheese(Cheese newCheese, Cheese originalCheese, float volume) 
	{
		newCheese.hardness = originalCheese.hardness;
		newCheese.density = originalCheese.density;
		newCheese.fallAudio = originalCheese.fallAudio;
		if(volume > minCuttableVolume) {
			newCheese.cuttable = true;
		} else {
			Debug.Log("Volume of cheese is below cuttable threshold" + volume);
		}
	}

	private float getVolume(GameObject go)
	{
		Mesh mesh = go.GetComponent<MeshFilter>().mesh;
		float volume = mesh.bounds.size.x * mesh.bounds.size.y * mesh.bounds.size.z;
		return volume * 1000*1000;
	}


	private void OnTriggerEnter(Collider other)
	{
		Debug.Log("collision!");
		if (isSharp && timer <= 0.01f)
		{
			Cheese cheese = other.GetComponent<Cheese>();
 			if (cheese) {
				if(cheese.cuttable) {	 
					SliceThing(other.gameObject);
					DisableSliceFor(waitingTime);
				}
			}
			else {
				Debug.Log("no cut");
			}
		}
	}

	void DisableSliceFor(float amount)
	{
		timer = Mathf.Max(timer, amount);
	}
}
