using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cheese : MonoBehaviour
{
	public float hardness = 1f;
	public float density = 1f;
	public bool cuttable = true;
	public bool pickable = false;
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
			Destroy(gameObject);
		}
	}
}
