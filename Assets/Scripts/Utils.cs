using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Utils
{
	public static float GetVolume(GameObject go) {
		Mesh mesh = go.GetComponent<MeshFilter>().mesh;
		float volume = mesh.bounds.size.x * mesh.bounds.size.y * mesh.bounds.size.z;
		return volume * 1000 * 1000;
	}
}
