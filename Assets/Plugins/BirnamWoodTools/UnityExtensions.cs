using UnityEngine;

/// <summary>
/// A collection of extensions of Unity classes.
/// </summary>
public static class UnityExtensions 
{
	#region Transform Extentensions
	#region Position
	public static void SetXPosition(this Transform transform, float x)
	{
		transform.position = new Vector3(x,transform.position.y,transform.position.z);
	}
	public static void AddXPosition(this Transform transform, float x)
	{
		transform.position = new Vector3(transform.position.x + x, transform.position.y, transform.position.z);
	}
	public static void SetYPosition(this Transform transform, float y)
	{
		transform.position = new Vector3(transform.position.x,y,transform.position.z);
	}
	public static void AddYPosition(this Transform transform, float y)
	{
		transform.position = new Vector3(transform.position.x, transform.position.y + y, transform.position.z);
	}
	public static void SetZPosition(this Transform transform, float z)
	{
		transform.position = new Vector3(transform.position.x, transform.position.y, z);
	}
	public static void AddZPosition(this Transform transform, float z)
	{
		transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z + z);
	}
	public static void SetXYPosition(this Transform transform, float x, float y)
	{
		transform.position = new Vector3(x, y, transform.position.z);
	}
	#endregion

	#region Scale
	public static void SetXScale(this Transform transform, float x)
	{
		transform.localScale = new Vector3(x, transform.localScale.y, transform.localScale.z);
	}
	public static void AddXScale(this Transform transform, float x)
	{
		transform.localScale = new Vector3(transform.localScale.x + x, transform.localScale.y, transform.localScale.z);
	}
    public static void ScaleX(this Transform transform, float x)
    {
        transform.localScale = new Vector3(transform.localScale.x * x, transform.localScale.y, transform.localScale.z);
    }
	public static void SetYScale(this Transform transform, float y)
	{
		transform.localScale = new Vector3(transform.localScale.x, y, transform.localScale.z);
	}
	public static void AddYScale(this Transform transform, float y)
	{
		transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y + y, transform.localScale.z);
	}
    public static void ScaleY(this Transform transform, float y)
    {
        transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y * y, transform.localScale.z);
    }
    public static void SetZScale(this Transform transform, float z)
	{
		transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, z);
	}
	public static void AddZScale(this Transform transform, float z)
	{
		transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, transform.localScale.z + z);
	}
    public static void ScaleZ(this Transform transform, float z)
    {
        transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, transform.localScale.z * z);
    }
	public static void AddXYScale(this Transform transform, Vector2 vector)
	{
		transform.localScale = new Vector3(transform.localScale.x + vector.x, transform.localScale.y + vector.y, transform.localScale.z);
	}
	public static void AddXYScale(this Transform transform, float x, float y)
	{
		transform.localScale = new Vector3(transform.localScale.x + x, transform.localScale.y + y, transform.localScale.z);
	}
	public static void SetScaleXY(this Transform transform, float x, float y)
	{
		transform.localScale = new Vector3(x, y, transform.localScale.z);
	}
	public static void AddXYZScale(this Transform transform, Vector3 vector)
	{
		transform.localScale = new Vector3(transform.localScale.x + vector.x, transform.localScale.y + vector.y, transform.localScale.z + vector.z);
	}
	public static void SetXYZScale(this Transform transform, Vector3 vector)
	{
		transform.localScale = vector;
	}
	public static void Scale(this Transform transform,float x,float y,float z)
	{
		transform.localScale = new Vector3(transform.localScale.x * x, transform.localScale.y * y, transform.localScale.z * z);
	}
	#endregion
	#endregion

	#region Renderer Extensions
	public static Rect GetScreenRect(this Renderer renderer)
	{
		Vector2 pos = Camera.main.WorldToScreenPoint(renderer.bounds.min);
		Vector2 max = Camera.main.WorldToScreenPoint(renderer.bounds.max);
		Vector2 min = Camera.main.WorldToScreenPoint(renderer.bounds.min);

		return new Rect(min.x, Screen.height - min.y - (max.y - min.y), max.x - min.x, (max.y - min.y));
	}
	#endregion

	#region Color Extensions
	public static bool CloseTo(this Color color, Color otherColor, float closeness=0.03f)
	{
		return (Mathf.Abs(color.r - otherColor.r) <= closeness && Mathf.Abs(color.g - otherColor.g) <= closeness
			&& Mathf.Abs(color.b - otherColor.b) <= closeness && Mathf.Abs(color.a - otherColor.a) <= closeness);
	}
	public static void SetAlpha(this Color color, float alpha)
	{
		color = new Color(color.r, color.g, color.b, alpha);
	}
	#endregion

	#region Vector2 Extensions
	public static Vector2 ReturnScale(this Vector2 vector, Vector2 scale)
	{
		return new Vector2(vector.x * scale.x, vector.y * scale.y);
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
	#endregion

	#region Rect Extensions
	public static void Scale(this Rect rect, Vector2 scale)
	{
		rect.x *= scale.x;
		rect.y *= scale.y;
		rect.width *= scale.x;
		rect.height *= scale.y;
	}
	#endregion
}