using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using EzySlice;

// ref:
// https://github.com/DavidArayan/ezy-slice

public class Slicer : MonoBehaviour
{
	public GameObject cheeses;

	public float thrust = 10f;
	public float waitingTime = 0.5f;
	public float minCuttableVolume = 10f; // in cm3
	public float minMass = 0.1f;
	public bool isSharp = false;
	float timer = 0;
	public bool hasSliced = false;

	public List<AudioClip> sliceSounds = new List<AudioClip>();

	void Update()
	{
		timer -= Time.deltaTime;
		timer = Mathf.Max(0, timer);
	}

	void SliceThing(GameObject go)
	{
		// Get original cheese properties
		Cheese originalCheese = go.GetComponent<Cheese>();
		Material crossSectionMaterial = originalCheese.crossSectionMaterial;
		TextureRegion textureRegion = crossSectionMaterial.GetTextureRegion(0, 0, 100, 100);

		// Attempt to slice
		GameObject[] slices;
		slices = go.SliceInstantiate(transform.position, transform.up, textureRegion, crossSectionMaterial);

		if(slices==null) {
			Debug.Log("Slicing Failed!");
			return;
		}
		hasSliced = true;
		int index = Random.Range(0, sliceSounds.Count);
		AudioSource.PlayClipAtPoint(sliceSounds[index], transform.position, 1.0f);
		// Loop over slices: first slice has positive thrust, second negative
		float sign=1.0f; 
		foreach(GameObject slice in slices)
		{
			slice.AddComponent<MeshCollider>();
			slice.GetComponent<MeshCollider>().convex = true;

			slice.AddComponent<Rigidbody>();
			Rigidbody rb = slice.GetComponent<Rigidbody>();

			// Compute volume and mass
			float volume = Utils.GetVolume(slice);
			rb.mass = volume * originalCheese.density / 1000.0f;
			if(rb.mass < minMass) {
				rb.mass = minMass;
			}
			
			// Add cheese properties to slices
			slice.AddComponent<Cheese>();
			updateNewCheese(slice.GetComponent<Cheese>(), originalCheese, volume);

			// Thrust!
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
		newCheese.crossSectionMaterial = originalCheese.crossSectionMaterial;
		newCheese.pickable = true;
		if(volume > minCuttableVolume) {
			newCheese.cuttable = true;
		}
	}

	private void	PickThing(GameObject go)
    {
		go.transform.SetParent(transform.parent);
		Rigidbody rb = go.GetComponent<Rigidbody>();
		rb.constraints = RigidbodyConstraints.FreezeAll;
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
		if (ctrl.state == State.Picking_Down && cheese.pickable && ctrl.timer <= 0.01f)
		{
			ctrl.state = State.Holding;
			PickThing(other.gameObject);
		}
	}

	void DisableSliceFor(float amount)
	{
		timer = Mathf.Max(timer, amount);
	}
}
