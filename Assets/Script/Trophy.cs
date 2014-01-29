using UnityEngine;
using System.Collections;

public class Trophy : MonoBehaviour {
	public UIButton button;

	Animator animator;
	// Use this for initialization
	void Start () 
	{
		button.clickEvent += Click;
		//enabled = false;
		animator = GetComponent<Animator>();
	}

	void Click()
	{
		animator.SetTrigger("Click");
	}
}
