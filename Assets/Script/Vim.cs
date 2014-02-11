using UnityEngine;
using System.Collections;
using UnityEngine.SocialPlatforms;

public class Vim : MonoBehaviour {
	
	public UIPercentBar bar;

	Animator animator;
	// Use this for initialization
	void Start () 
	{
		GameKitBinding.AuthenticatePlayer();
		//iOSBinding.PopAlertWithHeaderAndText("header", "text!");
		//button.clickEvent += Click;
		//enabled = false;
		animator = GetComponent<Animator>();
		//Debug.Log(animator.Pl);
	}

	void Update()
	{
		if(Input.GetMouseButtonDown(0))
			bar.AdjustBar(50.0f);
	}

	void OnApplicationPause(bool pause)
	{
		if(pause)
			Debug.Log("pausing");
	}

	void Authenticate(bool authenticated)
	{
		Debug.Log(authenticated);
	}

	/*void Click()
	{
		animator.SetTrigger("Idle_Trigger");
	}*/
}