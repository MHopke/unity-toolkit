using gametheory.iOS.GameKit;

namespace gametheory
{
	[System.Obsolete]
    /// <summary>
    /// Representation of an achievement.
    /// </summary>
    public class Achievement 
    {
    	#region Constants
    	const string UNLOCKED_KEY = "UNLOCKED";
    	const string PROGRESS_KEY = "PROGRESS";
    	#endregion

    	#region Public Variables
    	public string Name;
    	public string Description;

    	public bool Hidden;
    	public bool Unlocked;

        /// <summary>
        /// The current progress on the achievement. 100f indicates completion.
        /// </summary>
    	public float Progress;

        /// <summary>
        /// Platform specific identifier. This is how the achievement shows up in GameCenter,
        /// Google Play, Steam, etc.
        /// </summary>
    	public string PlatformIdentifier;
    	#endregion

    	#region Constructors
    	public Achievement(){}

    	public Achievement(string name, string description, string platformId)
    	{
    		Name = name;
    		Description = description;
    		PlatformIdentifier = platformId;
    	}
    	#endregion

    	#region Methods
    	/// <summary>
    	/// Updates the progress of the achievment.
    	/// </summary>
    	/// <returns><c>true</c>, if progress was reported, <c>false</c> otherwise.</returns>
    	/// <param name="progress">Progress.</param>
    	public bool ReportProgress(float progress)
    	{
    		if(Unlocked)
    			return false;
    		else
    		{
    			Progress = progress;

    			if(progress >= 100.0f)
    			{
    				Unlocked = true;
    				Progress = 100.0f;
    			}

    			#if UNITY_IPHONE
    			GameKitBinding.ReportAchievement(Name, Progress);
    			#elif UNITY_ANDROID
    			#endif

    			return true;
    		}
    	}
    	#endregion

    	#region String Parsing
    	public void Parse(string str)
    	{
    		string[] splitStr = str.Split(',');

    		if(splitStr.Length > 1)
    		{
    			if(!bool.TryParse(splitStr[0], out Unlocked))
    				Unlocked = false;

    			if(splitStr.Length > 2)
    			{
    				if(float.TryParse(splitStr[1], out Progress))
    					Progress = 0.0f;
    			}
    		}
    	}
    	public override string ToString()
    	{
    		return Unlocked.ToString() + "," + Progress.ToString();
    	}
    	#endregion
    }
}