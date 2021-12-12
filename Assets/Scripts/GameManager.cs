using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
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
