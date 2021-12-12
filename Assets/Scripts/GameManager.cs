using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class GameManager : Singleton<GameManager>
{
	public Text[] scoreTexts;
	private int[] playersScores = new int[] {0,0,0,0};
	public Text timerText;
	public float gameTime = 60f;

	void Update()
	{
		UpdateScores();
		ConstrainMouse();
	}

	void ConstrainMouse()
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
		gameTime -= Time.deltaTime;
		timerText.text = ((int)gameTime).ToString() + "s";
	}

	public void AddScore(int score, int id)
	{
		playersScores[id] += score;
	}

	void UpdateScores()
	{
		for(int id = 0; id < scoreTexts.Length; id++)
		{
			scoreTexts[id].text = playersScores[id].ToString();
		}
	}
}
