using UnityEngine;
using System.Collections;

public class Trophy : MonoBehaviour {

	GameObject obj;

	// Use this for initialization
	void Start () 
	{
		obj = (GameObject)GameObject.Instantiate(Resources.Load("Screens/UIManager"));
	}

	void Update()
	{
		if(Input.GetMouseButtonDown(0) && obj)
			Destroy(obj);
	}
}
