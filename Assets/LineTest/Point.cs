using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

public class Point : MonoBehaviour 
{
	#region Events
	public static event System.Action<Point> clicked;
	#endregion

	#region Public Vars
	public int LineIndex;
	public LineRenderer Line;
	#endregion

	#region PRviate Vars
	Vector3 _dragPos;
	#endregion

	#region UI Methods
	public void OnPointerDown(BaseEventData data)
	{
		if(Line)
			return;
		
		if(clicked != null)
			clicked(this);
	}
	public void OnDrag(BaseEventData data)
	{
		if(Line == null)
			return;

		PointerEventData pointData = data as PointerEventData;

		_dragPos = Camera.main.ScreenToWorldPoint(pointData.position);
		_dragPos.z = transform.position.z;

		transform.position = _dragPos;

		Line.SetPosition(LineIndex,transform.position);
	}
	#endregion
}
