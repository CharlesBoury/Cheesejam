using UnityEditor;
using UnityEngine;

// code from https://stackoverflow.com/questions/54431635

[InitializeOnLoad]
static class FullscreenShortcut
{
	static FullscreenShortcut()
	{
		EditorApplication.update += Update;
	}

	static void Update()
	{
		#if UNITY_EDITOR
		if (EditorApplication.isPlaying && ShouldToggleMaximize())
		{
			EditorWindow.mouseOverWindow.maximized = !EditorWindow.mouseOverWindow.maximized;
		}
		#endif
	}

	private static bool ShouldToggleMaximize()
	{
		return Input.GetKey(KeyCode.Space) && Input.GetKey(KeyCode.LeftShift);
	}
}
