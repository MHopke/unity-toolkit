using UnityEngine;
using System.Collections;

public class Vim : MonoBehaviour {

	#region Constants
	const string ACHIEVEMENT_KEY = "Achievements";
	#endregion

	public ButtonComponent button;

	public UIFadeComponent _fade;

	Animator animator;
	// Use this for initialization
	void Start () 
	{
		/*AchievementManager.LoadAchievements(PlayerPrefs.GetString(ACHIEVEMENT_KEY, ""));

		PlayerPrefs.SetString(ACHIEVEMENT_KEY,AchievementManager.SaveAchievements());*/

		button.Activate();
		button.clickEvent += Click;


		//iOSBinding.PopAlertWithHeaderAndText("header", "text!");
		//button.clickEvent += Click;
		//enabled = false;
		animator = GetComponent<Animator>();

	}

	/*void Update()
	{
		if(Input.GetMouseButtonDown(0))
			bar.CurrentPosition = new Vector2(300, 200f);
	}*/

	void OnApplicationPause(bool pause)
	{
		if(pause)
			Debug.Log("pausing");
	}

	void Authenticate(bool authenticated)
	{
		Debug.Log(GameKitBinding._localUser.displayName + GameKitBinding._localUser.playerID);
	}

	void Click()
	{
		_fade.FadeOut();
	}
}