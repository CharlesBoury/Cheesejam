using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : Singleton<GameManager>
{
	public int[] playersScores;

	void OnEnable()
	{
		// init playersScores
	}

	void Update()
	{
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			Cursor.lockState = CursorLockMode.None;
			Cursor.visible = true;
		}
		if (Input.GetMouseButtonDown(0))
		{
			Cursor.lockState = CursorLockMode.Confined;
			Cursor.visible = false;
		}
	}

	public void AddScore(int score, int id)
	{
		Debug.Log("SCORE!!");
		// playersScores[id] += score;
	}
}
