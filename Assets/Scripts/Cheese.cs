using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cheese : MonoBehaviour
{
	public float hardness = 1f;
	public float density = 1f;
	public bool cuttable = true;
	public bool pickable = false;
	public float minCuttableVolume = 10f; // in cm3
	public float maxPickableVolume = 400f; // in cm3

	public Material crossSectionMaterial;
    public AudioClip fallAudio;

	private bool hasSoundPlayed=false;

	void Update()
	{
		if (transform.position.y < -0.5f)
		{
			if(!hasSoundPlayed) {
				AudioSource.PlayClipAtPoint(fallAudio, transform.position, 1.0f);
				hasSoundPlayed = true;
			}
		}
		if (transform.position.y < -2f)
		{
			CheeseSpawner cheeseSpawner = GameManager.Instance.GetComponent<CheeseSpawner>();
			cheeseSpawner.OnRemoveCheese(gameObject);
			Destroy(gameObject);
		}
	}

	public float getVolume()
	{
		Mesh mesh = GetComponent<MeshFilter>().sharedMesh;
		float volume = mesh.bounds.size.x * mesh.bounds.size.y * mesh.bounds.size.z;
		return volume * 1000*1000;
	}

	public void inheritCheeseProperties(Cheese originalCheese, float volume) 
	{
		hardness = originalCheese.hardness;
		density = originalCheese.density;
		fallAudio = originalCheese.fallAudio;
		crossSectionMaterial = originalCheese.crossSectionMaterial;
		//Debug.Log(volume);
		// Check if cuttable
		cuttable = volume > minCuttableVolume;

		// Check if pickable
		pickable = volume < maxPickableVolume;
	}


}
