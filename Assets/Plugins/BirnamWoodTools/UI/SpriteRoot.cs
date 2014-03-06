using UnityEngine;
using System.Collections;

/// <summary>
/// This class acts as the base for any Sprite. This class should be used so that the class can
/// automatically adjust its starting position.
/// </summary>
public class SpriteRoot : MonoBehaviour {

	#region Public Variables
	public Vector2 _startPosition;
	#endregion

	// Use this for initialization
	void Start () 
	{
		_startPosition.Scale(UINavigationController.AspectRatio);

		SetPosition(_startPosition);
	}

	public void SetPosition(Vector2 position)
	{
		transform.position = Camera.main.ScreenToWorldPoint(new Vector3(position.x,position.y,1f));
	}
}
