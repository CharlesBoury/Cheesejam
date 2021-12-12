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
    
	[Range(0.05f, 2f)]
	public float minDistanceFromCenter = 0.05f;
    [Range(0.1f, 2f)]
	public float minDistanceFromEdge = 0.1f;

	[Range(0f, 1f)]
	public float fallingDistance = 0.0001f;
	
	[Range(0f, 1f)]
	public float spawnTimeDelta = 0.01f;
	
	public bool regular = true;
	public bool cheeseInTheMiddle = true;
	private float lastDropped = -1f;
	private float radius;
	private float cheeseOrientation;

	public Transform height;

	private readonly float tableRadius = 0.35f;

	void OnEnable()
	{
		if (height == null)
			height = transform;
	}

	void InstantiateRandomCheese(Vector3 pos, float angle) {
        int cheeseIndex = Random.Range(0, cheeseList.Count);
		GameObject original = cheeseList[cheeseIndex];
		Instantiate(original, pos, Quaternion.Euler(0, - angle * Mathf.Rad2Deg + cheeseOrientation, 0), cheeseObject.transform);
	}

    void Start()
    {
		Debug.Log("table radius: " + tableRadius);
		if (cheeseInTheMiddle)
			radius = tableRadius / 2f;
		else
			radius = Mathf.Max(minDistanceFromCenter, Random.Range(minDistanceFromCenter, (tableRadius - minDistanceFromEdge)));
		cheeseOrientation = 90 * Random.Range(-2, 2);
    }

	void SpawnRandomCheese() {
		float angle;

		if (regular)
			angle = (2 * nbCheesesDropped * Mathf.PI) / nbCheesesToDrop;
		else
			angle = Random.Range(0f, 2f * Mathf.PI);
		if (!regular)
			radius = Mathf.Max(minDistanceFromCenter, Random.Range(minDistanceFromCenter, (tableRadius - minDistanceFromEdge)));
		Debug.Log("radius" + radius);

		float x = Mathf.Cos(angle) * radius;
		float z = Mathf.Sin(angle) * radius;
		InstantiateRandomCheese(new Vector3(height.position.x + x, height.position.y + fallingDistance, height.position.z + z), angle);
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
