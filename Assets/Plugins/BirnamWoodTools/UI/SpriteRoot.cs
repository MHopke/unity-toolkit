using UnityEngine;
using System.Collections;

/// <summary>
/// This class acts as the base for any Sprite. This class should be used so that the class can
/// automatically adjust its starting position.
/// </summary>
public class SpriteRoot : MonoBehaviour {

	#region Public Variables
	public float _zDepth;
	public Vector2 _startPosition;
	#endregion

	// Use this for initialization
	protected void Start () 
	{
		_startPosition.Scale(UIScreen.AspectRatio);

		transform.Scale(UIScreen.AspectRatio.x, UIScreen.AspectRatio.y, 1);

		SetPosition(_startPosition);
	}

	public void SetPosition(Vector2 position)
	{
		transform.position = Camera.main.ScreenToWorldPoint(new Vector3(position.x,Screen.height - position.y,_zDepth));
	}
}
