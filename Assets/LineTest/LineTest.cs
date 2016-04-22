using UnityEngine;
using System.Collections;

public class LineTest : MonoBehaviour 
{
	#region Public Vars
	public LineRenderer Prefab;
	#endregion

	#region Private Vars
	LineRenderer _line = null;
	#endregion

	#region Unity MEthods
	void Awake()
	{
		Point.clicked += PointClicked;
	}
	void OnDestroy()
	{
		Point.clicked -= PointClicked;
	}
	#endregion

	#region Event Listeners
	void PointClicked(Point point)
	{
		if(_line == null)
		{
			_line = (LineRenderer)GameObject.Instantiate(Prefab,Vector3.zero,Quaternion.identity);

			_line.SetPosition(0,point.transform.position);
			_line.SetPosition(1,point.transform.position);

			point.LineIndex = 0;
			point.Line = _line;
		}
		else
		{

			point.Line = _line;
			point.LineIndex = 1;
			_line.SetPosition(1,point.transform.position);
			_line = null;
		}
	}
	#endregion
}
