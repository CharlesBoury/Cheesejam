using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class GameManager : Singleton<GameManager>
{
	public Text[] scoreTexts;
	private int[] playersScores = new int[] {0,0,0,0};

	void OnEnable()
	{
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
		playersScores[id] += score;
		// Debug.Log("SCORE of "+id+ "="+playersScores[id]);
		scoreTexts[id].text = playersScores[id].ToString();
	}
}
