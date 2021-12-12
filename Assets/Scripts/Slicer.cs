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
	public GameObject cheeses;

	public float thrust = 100f;
	public float waitingTime = 0.5f;
	public float minCuttableVolume = 10f; // in cm3
	public float minMass = 0.02f;
	public bool isSharp = false;
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
		newCheese.pickable = true;
		if (volume > minCuttableVolume) {
			newCheese.cuttable = true;
		}
	}

	private float getVolume(GameObject go)
	{
		Mesh mesh = go.GetComponent<MeshFilter>().mesh;
		float volume = mesh.bounds.size.x * mesh.bounds.size.y * mesh.bounds.size.z;
		return volume * 1000*1000;
	}

	private void	PickThing(GameObject go)
    {
		go.transform.SetParent(transform.parent);
    }


	private void OnTriggerEnter(Collider other)
	{
		Cheese cheese = other.GetComponent<Cheese>();
		if (!cheese)
			return;
		if (isSharp && timer <= 0.01f)
		{
			if (cheese.cuttable) {	 
				SliceThing(other.gameObject);
				DisableSliceFor(waitingTime);
			}
		}
		GameObject trident = transform.parent.gameObject;
		PlayerController ctrl = trident.GetComponent<PlayerController>();
		if (ctrl.hasPicked == false && ctrl.pickingState == 1 && cheese.pickable && ctrl.timer <= 0.01f)
		{
			ctrl.hasPicked = true;
			ctrl.pickingStateSave = ctrl.pickingState;
			ctrl.pickingState = 0;
			PickThing(other.gameObject);
		}
	}

	void DisableSliceFor(float amount)
	{
		timer = Mathf.Max(timer, amount);
	}
}
