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
	public float minCuttableVolume = 1f; // in cm3
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
		Debug.Log(slices);

		Cheese originalCheese = go.GetComponent<Cheese>();

		foreach(GameObject slice in slices)
		{
			slice.AddComponent<MeshCollider>();
			slice.GetComponent<MeshCollider>().convex = true;

			slice.AddComponent<Rigidbody>();
			slice.GetComponent<Rigidbody>().AddForce(transform.up * thrust * (Random.value > 0.5f ? 1f : -1f));

			slice.AddComponent<Cheese>();
			float volume = getVolume(slice);
			updateNewCheese(slice.GetComponent<Cheese>(), originalCheese, volume); 
			
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
		if (timer < 0.01f)
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
