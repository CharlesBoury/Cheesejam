using UnityEngine;
using System;

// from https://www.unitygeek.com/unity_c_singleton/
// But without the DontDestroyOnLoad(this.gameObject);
// see https://forum.unity.com/threads/singleton-references-when-changing-scene.989975/#post-6428642

public class Singleton<T> : MonoBehaviour where T : Component
{
	private static T _instance;
	public static T Instance
	{
		get
		{
			if(_instance == null)
			{
				_instance = FindObjectOfType<T>();
				if(_instance == null)
				{
					GameObject go = new GameObject();
					go.name = typeof(T).Name;
					_instance = go.AddComponent<T>();
				}
			}
			return _instance;
		}
	}

	public virtual void Awake()
	{
		if(_instance == null)
			_instance = this as T;
		else
			Destroy(gameObject);
	}
}
