using UnityEngine;
using System.Collections;

public static class UnityExtensions {

	public static void SetX(this Transform transform, float x)
	{
		transform.position = new Vector3(x,transform.position.y,transform.position.z);
	}
	public static void AddX(this Transform transform, float x)
	{
		transform.position = new Vector3(transform.position.x + x, transform.position.y, transform.position.z);
	}
	public static void SetY(this Transform transform, float y)
	{
		transform.position = new Vector3(transform.position.x,y,transform.position.z);
	}

	public static bool CloseTo(this Color color, Color otherColor)
	{
		return (Mathf.Abs(color.r - otherColor.r) <= 0.03f && Mathf.Abs(color.g - otherColor.g) <= 0.03f
			&& Mathf.Abs(color.b - otherColor.b) <= 0.03f && Mathf.Abs(color.a - otherColor.a) <= 0.03f);
	}

	public static Vector2 Parse(string str)
	{
		string[] arr = str.Split(',');

		float x = 0.0f;
		float y = 0.0f;

		if(arr.Length > 0)
		{
			float.TryParse(arr[0], out x);

			if(arr.Length > 1)
				float.TryParse(arr[1], out y);
		}

		return new Vector2(x, y);
	}
}