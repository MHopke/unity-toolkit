using UnityEngine;
using MiniJSON;
using System.IO;
using System.Text;
using System.Xml;
using System.Collections.Generic;

namespace gametheory
{
	[System.Obsolete]
    /// <summary>
    /// Manages progress, saving, and loading of achievements.
    /// </summary>
    public class AchievementManager 
    {
    	#region Private Variables
    	static Dictionary<string, Achievement> _achievements;
    	#endregion

    	#region Methods
    	public static void LoadAchievements(string json)
    	{
    		_achievements = new Dictionary<string, Achievement>();

    		TextAsset asset = (TextAsset)Resources.Load("Achievements");

    		if(asset != null)
    		{
    			StringReader strReader = new StringReader(asset.text);

    			using(XmlReader reader = XmlReader.Create(strReader))
    			{
    				while(reader.ReadToFollowing("Achievement"))
    				{
    					Achievement achievement = new Achievement();

    					reader.MoveToAttribute("name");
    					achievement.Name = reader.Value;

    					reader.MoveToAttribute("description");
    					achievement.Description = reader.Value;

    					#if UNITY_IOS
    					reader.MoveToAttribute("gameCenter");
    					#elif UNITY_ANDROID
    					reader.MoveToAttribute("googePlay");
    					#endif
    					achievement.PlatformIdentifier = reader.Value;

    					_achievements.Add(achievement.Name, achievement);
    				}
    			}
    		} else
    			Debug.LogError("Missing or misnamed achievments file");

    		//Unlock trophies
    		UnlockAchievements(json);
    	}

        /// <summary>
        /// Used to unlock any previously earned achievements.
        /// </summary>
        /// <param name="json">Json.</param>
    	static void UnlockAchievements(string json)
    	{
    		if(json != "")
    		{
    			Dictionary<string, object> dict = Json.Deserialize(json) as Dictionary<string, object>;

    			foreach(KeyValuePair<string,object> pair in dict)
    			{
    				if(_achievements.ContainsKey(pair.Key))
    				{
    					_achievements[pair.Key].Parse((string)pair.Value);
    				}
    			}
    		}
    	}

    	public static string SaveAchievements()
    	{
    		return Json.Serialize(_achievements);
    	}

        /// <summary>
        /// Updates the progress on an achievement.
        /// </summary>
        /// <param name="achievementId">Achievement identifier.</param>
        /// <param name="progress">Progress.</param>
    	public static void UpdateAchievement(string achievementId, float progress)
    	{
    		if(_achievements.ContainsKey(achievementId))
    		{
    			//Save the achievements if something has been updated
    			if(_achievements[achievementId].ReportProgress(progress))
    				SaveAchievements();
    		}
    	}
    	#endregion
    }
}