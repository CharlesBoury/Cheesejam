using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheeseSpawner : MonoBehaviour
{
	public List<GameObject> cheeseList = new List<GameObject>();

    [Range(1, 10)]
	public int nbCheesesAtStart = 5;
    [Range(0f, 0.5f)]
	public float minDistanceFromCenter = 0.2f;
    [Range(0f, 0.5f)]
	public float minDistanceFromEdge = 0.2f;
	public bool regular = true;

	void InstantiateRandomCheese(Vector3 pos) {
        int cheeseIndex = UnityEngine.Random.Range(0, cheeseList.Count);
		Instantiate(cheeseList[cheeseIndex], pos, Quaternion.Euler(0, UnityEngine.Random.Range(0, 180), 0));
	}
	
	void InstantiateRandomCheese(Vector3 pos, float angle) {
		Debug.Log(angle);
        int cheeseIndex = UnityEngine.Random.Range(0, cheeseList.Count);
		Instantiate(cheeseList[cheeseIndex], pos, Quaternion.Euler(0, - angle * Mathf.Rad2Deg, 0));
	}

	void totalRandom() {
		for (int i = 0; i < nbCheesesAtStart; i++) {		
			float radius = UnityEngine.Random.Range(minDistanceFromCenter, (transform.localScale.x / 2) - minDistanceFromEdge);
			float angle = Random.Range(0f, 2f * Mathf.PI);
			float x = Mathf.Cos(angle) * radius;
			float z = Mathf.Sin(angle) * radius;
			InstantiateRandomCheese(new Vector3(transform.position.x + x, transform.position.y + transform.localScale.y, transform.position.z + z));
		}
	}

    // Start is called before the first frame update
    void Start()
    {
		float angle;

		float regularRadius = UnityEngine.Random.Range(minDistanceFromCenter, (transform.localScale.x / 2) - minDistanceFromEdge);
        for (int i = 0; i < nbCheesesAtStart; i++) {		
			float radius = UnityEngine.Random.Range(minDistanceFromCenter, (transform.localScale.x / 2) - minDistanceFromEdge);
			if (regular)
				angle = (2 * i * Mathf.PI) / nbCheesesAtStart;
			else
				angle = Random.Range(0f, 2f * Mathf.PI);
			float x = Mathf.Cos(angle) * radius;
			float z = Mathf.Sin(angle) * radius;
			InstantiateRandomCheese(new Vector3(transform.position.x + x, transform.position.y + transform.localScale.y, transform.position.z + z), angle);
		}
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
