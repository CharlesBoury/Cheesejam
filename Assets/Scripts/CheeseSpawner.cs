using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheeseSpawner : MonoBehaviour
{
	public List<GameObject> cheeseList = new List<GameObject>();


	public GameObject cheeseObject;


    [Range(1, 20)]
	public int nbCheesesToDrop = 10;
	private int nbCheesesDropped = 0;
    
	[Range(0f, 0.5f)]
	public float minDistanceFromCenter = 0.2f;
    [Range(0f, 0.5f)]
	public float minDistanceFromEdge = 0.2f;
	[Range(0f, 1f)]

	public float fallingDistance = 0.0001f;
	
	[Range(0f, 1f)]
	public float spawnTimeDelta = 0.01f;

	public bool regular = true;

	private float lastDropped = -1f;
	private float radius;

	void InstantiateRandomCheese(Vector3 pos, float angle) {
        int cheeseIndex = Random.Range(0, cheeseList.Count);
		GameObject original = cheeseList[cheeseIndex];
		Instantiate(original, pos, Quaternion.Euler(0, - angle * Mathf.Rad2Deg, 0), cheeseObject.transform);
	}

    // Start is called before the first frame update
    void Start()
    {
		radius = Random.Range(minDistanceFromCenter, (transform.localScale.x / 2) - minDistanceFromEdge);
    }

	void SpawnRandomCheese() {
		float angle;

		if (regular)
			angle = (2 * nbCheesesDropped * Mathf.PI) / nbCheesesToDrop;
		else
			angle = Random.Range(0f, 2f * Mathf.PI);
		if (!regular)
			radius = Random.Range(minDistanceFromCenter, (transform.localScale.x / 2) - minDistanceFromEdge);
		float x = Mathf.Cos(angle) * radius;
		float z = Mathf.Sin(angle) * radius;
		InstantiateRandomCheese(new Vector3(transform.position.x + x, transform.position.y + transform.localScale.y + fallingDistance, transform.position.z + z), angle);
		nbCheesesDropped++;
	}

    // Update is called once per frame
    void Update()
    {
		if (nbCheesesDropped < nbCheesesToDrop && Time.time - lastDropped > spawnTimeDelta) {
			SpawnRandomCheese();
			lastDropped = Time.time;
		}
    }
}
