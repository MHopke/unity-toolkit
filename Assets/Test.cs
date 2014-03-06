using UnityEngine;
using System.Collections;

public class Test : MonoBehaviour {

	public Rect rect;

	Vector2 position;
	Vector3 scale;

	float relativeScale;
	int originalSize;
	public GUIStyle style;

	// Use this for initialization
	void Start () {
		scale = transform.localScale;
		originalSize = style.fontSize;
	}

	void Update()
	{
		position = Camera.main.WorldToScreenPoint(transform.position);
		relativeScale = scale.x / transform.localScale.x;
		rect.x = position.x;
		rect.y = position.y;
	}

	void OnGUI()
	{
		style.fontSize = Mathf.RoundToInt(originalSize * relativeScale);
		//Debug.Log(style.fontSize);
		GUI.Label(rect, "TEST",style);
	}
}
