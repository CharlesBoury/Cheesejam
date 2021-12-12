using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : Singleton<GameManager>
{
	public int[] playersScores;

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
}
