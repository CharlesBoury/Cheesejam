using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
	public Text[] scoreTexts;
	public static int[] playersScores = new int[] {0,0,0,0};
	public Text timerText;
	public float gameTime = 60f;
	public bool scoreBasedOnVolume = true;
	AudioSource fxSound; 

	void Start ()
	{
		// Audio Source to play 
		fxSound = GetComponent<AudioSource> ();
		fxSound.Play ();
		playersScores = new int[] { 0, 0, 0, 0 };
	}

	void Update()
	{
		ConstrainMouse();

		UpdateScores();

		gameTime -= Time.deltaTime;
		gameTime = Mathf.Max(gameTime, 0);
		timerText.text = ((int)gameTime).ToString() + "s";

		if (gameTime == 0)
		{
			GameOver();
		}
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

	void GameOver()
	{
		int winner = 0;
		int bestScore = 0;
		for(int id = 0; id < scoreTexts.Length; id++)
		{
			if (playersScores[id] > bestScore)
			{
				bestScore = playersScores[id];
				winner = id;
			}

		}
		SceneManager.LoadScene("EndGame");
	}
}
