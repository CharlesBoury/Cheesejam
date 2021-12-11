using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheeseSpawner : MonoBehaviour
{

	public List<GameObject> prefabList = new List<GameObject>();
    //public GameObject cheese1;
    //public GameObject cheese2;
	
	/*
	void InstantiateCheese() {
		Instantiate(cheese, new Vector3(0f,0.5f,0f), Quaternion.identity);
	}
	*/
	void InstantiateRandomCheese(Vector3 pos) {
		Debug.Log(prefabList.Count);
        int prefabIndex = UnityEngine.Random.Range(0, prefabList.Count);
		Debug.Log(prefabIndex);
		Instantiate(prefabList[prefabIndex], pos, Quaternion.identity);
	}

    // Start is called before the first frame update
    void Start()
    {
 		//InstantiateCheese();
		InstantiateRandomCheese(new Vector3(0f,0.5f,0f));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
