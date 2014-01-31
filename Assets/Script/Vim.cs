using UnityEngine;
using System.Collections;

public class Vim : MonoBehaviour {
	
	public UIButton button;

	Animator animator;
	// Use this for initialization
	void Start () 
	{
		//button.clickEvent += Click;
		//enabled = false;
		animator = GetComponent<Animator>();
	}

	void OnApplicationPause(bool pause)
	{
		if(pause)
			Debug.Log("pausing");
	}

	void Click()
	{
		animator.SetTrigger("Idle_Trigger");
	}
}
