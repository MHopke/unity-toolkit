using UnityEngine;

namespace gametheory
{
    /// <summary>
    /// This acts as the base GameObject that gametheory plugin's should be attached to. The native
    /// side 
    /// </summary>
    public class PluginsManager : MonoBehaviour 
    {
        PluginsManager singleton = null;

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
}