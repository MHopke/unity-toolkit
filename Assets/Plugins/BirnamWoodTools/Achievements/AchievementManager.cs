using UnityEngine;
using MiniJSON;
using System.IO;
using System.Text;
using System.Xml;
using System.Collections.Generic;

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
					achievement._name = reader.Value;

					reader.MoveToAttribute("description");
					achievement._description = reader.Value;

					#if UNITY_IOS
					reader.MoveToAttribute("gameCenter");
					#elif UNITY_ANDROID
				readeder.MoveToAttribute("googePlay");
					#endif
					achievement._platformIdentifier = reader.Value;

					_achievements.Add(achievement._name, achievement);
				}
			}
		} else
			Debug.LogError("Missing or misnamed achievments file");

		//Unlock trophies
		UnlockAchievements(json);
	}

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
