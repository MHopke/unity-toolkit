using UnityEngine;
using System.Collections;

public static class UnityExtensions {

	#region Transform Extentensions
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

	public static void ScaleX(this Transform transform, float x)
	{
		transform.localScale = new Vector3(transform.localScale.x + x, transform.localScale.y, transform.localScale.z);
	}
	public static void ScaleXY(this Transform transform, Vector2 vector)
	{
		transform.localScale = new Vector3(transform.localScale.x + vector.x, transform.localScale.y + vector.y, transform.localScale.z);
	}
	public static void ScaleXY(this Transform transform, float x, float y)
	{
		transform.localScale = new Vector3(transform.localScale.x + x, transform.localScale.y + y, transform.localScale.z);
	}

	public static void ScaleXYZ(this Transform transform, Vector3 vector)
	{
		transform.localScale = new Vector3(transform.localScale.x + vector.x, transform.localScale.y + vector.y, transform.localScale.z + vector.z);
	}
	#endregion

	#region Color Extensions
	public static bool CloseTo(this Color color, Color otherColor)
	{
		return (Mathf.Abs(color.r - otherColor.r) <= 0.03f && Mathf.Abs(color.g - otherColor.g) <= 0.03f
			&& Mathf.Abs(color.b - otherColor.b) <= 0.03f && Mathf.Abs(color.a - otherColor.a) <= 0.03f);
	}
	#endregion

	#region Vector2 Extensions
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
	#endregion
}