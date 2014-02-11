public class Achievement 
{
	#region Constants
	const string UNLOCKED_KEY = "UNLOCKED";
	const string PROGRESS_KEY = "PROGRESS";
	#endregion

	#region Public Variables
	public string _name;
	public string _description;

	public bool _hidden;
	public bool _unlocked;

	public float _progress;

	public string _platformIdentifier;
	#endregion

	#region Constructors
	public Achievement(){}

	public Achievement(string name, string description, string platformId)
	{
		_name = name;
		_description = description;
		_platformIdentifier = platformId;
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
		if(_unlocked)
			return false;
		else
		{
			_progress = progress;

			if(progress >= 100.0f)
			{
				_unlocked = true;
				_progress = 100.0f;
			}

			#if UNITY_IPHONE
			GameKitBinding.ReportAchievement(_name, _progress);
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
			if(!bool.TryParse(splitStr[0], out _unlocked))
				_unlocked = false;

			if(splitStr.Length > 2)
			{
				if(float.TryParse(splitStr[1], out _progress))
					_progress = 0.0f;
			}
		}
	}
	public override string ToString()
	{
		return _unlocked.ToString() + "," + _progress.ToString();
	}
	#endregion
}