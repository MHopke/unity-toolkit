using UnityEngine;

/// <summary>
/// BWG plugins manager. All BWG Plugins should be components of this GameObject.
/// ONLY ONE OF THIS SHOULD EXIST AT A TIME.
/// </summary>
public class BWGPluginsManager : MonoBehaviour 
{
	BWGPluginsManager singleton = null;

	void Awake()
	{
		if (singleton == null) 
		{
			DontDestroyOnLoad(gameObject);
			singleton = this;
		} 
		else
			Destroy(gameObject);
	}
}
