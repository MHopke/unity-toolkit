using UnityEngine;
using gametheory.UI;

/// <summary>
/// A collection of extensions of classes within Unity.
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
	public static void SetXYScale(this Transform transform, float x, float y)
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
	public static void SetXYZScale(this Transform transform, float x,float y,float z)
	{
		transform.localScale = new Vector3(x,y,z);
	}
	public static void Scale(this Transform transform,float x,float y,float z)
	{
		transform.localScale = new Vector3(transform.localScale.x * x, transform.localScale.y * y, transform.localScale.z * z);
	}
	#endregion
	#endregion

	#region Renderer Extensions
    /// <summary>
    /// Calculates a Rect that represents the area within the renderer's bounds. (in pixels).
    /// </summary>
    /// <returns>The screen rect.</returns>
    /// <param name="renderer">Renderer.</param>
	public static Rect GetScreenRect(this Renderer renderer)
	{
		Vector2 max = UIScreen.Instance.UICamera.WorldToScreenPoint(renderer.bounds.max);
        Vector2 min = UIScreen.Instance.UICamera.WorldToScreenPoint(renderer.bounds.min);

		return new Rect(min.x, Screen.height - min.y - (max.y - min.y), max.x - min.x, (max.y - min.y));
	}
	#endregion

	#region Color Extensions
    /// <summary>
    /// Checks if a color is close to another. Utilized when lerping between colors.
    /// </summary>
    /// <returns><c>true</c>, if the colors are close, <c>false</c> otherwise.</returns>
    /// <param name="color">Color.</param>
    /// <param name="otherColor">Other color.</param>
    /// <param name="closeness">The difference between color values that is checked for similarity.</param>
	public static bool CloseTo(this Color color, Color otherColor, float closeness=0.03f)
	{
		return (Mathf.Abs(color.r - otherColor.r) <= closeness && Mathf.Abs(color.g - otherColor.g) <= closeness
			&& Mathf.Abs(color.b - otherColor.b) <= closeness && Mathf.Abs(color.a - otherColor.a) <= closeness);
	}
	public static Color SetAlpha(Color color, float alpha)
	{
		return new Color(color.r, color.g, color.b, alpha);
	}
    /// <summary>
    /// Creates a Unity Color from an RGBA color set.
    /// </summary>
    /// <returns>The generated Unity Color.</returns>
    /// <param name="r">The red component.</param>
    /// <param name="g">The green component.</param>
    /// <param name="b">The blue component.</param>
    /// <param name="a">The alpha component.</param>
	public static Color ColorFromRGB(int r, int g, int b, int a=255)
	{
		return new Color((float)r / 255f, (float)g / 255f, (float)b / 255f, (float)a / 255f);
	}
	#endregion

	#region Vector2 Extensions
    /// <summary>
    /// Creates a new Vector2 by scaling the current one.
    /// </summary>
    /// <returns>The new scaled vector.</returns>
    /// <param name="vector">Vector.</param>
    /// <param name="scale">Scale.</param>
	public static Vector2 CalculateVector(this Vector2 vector, Vector2 scale)
	{
		return new Vector2(vector.x * scale.x, vector.y * scale.y);
	}
    /// <summary>
    /// Parse the string into a Vector2.
    /// </summary>
    /// <param name="str">String.</param>
    public static bool Parse(string str, out Vector2 vector)
	{
		string[] arr = str.Split(',');

		float x = 0.0f;
		float y = 0.0f;

        if (arr.Length > 0)
        {
            if (float.TryParse(arr[0], out x))
            {
                if (arr.Length > 1)
                {
                    if (float.TryParse(arr[1], out y))
                    {
                        vector = new Vector2(x, y);
                        return true;
                    }
                }
            }
        }

        vector = Vector2.zero;

        return false;

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