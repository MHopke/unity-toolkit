using UnityEditor;


/*[CanEditMultipleObjects]
[CustomEditor(typeof(UIButton))]
public class UIButtonEditor : Editor 
{
	UIButton _target;

	UIButton.ButtonType _type;

	public void OnEnable()
	{
		_target = (UIButton)target;
	}

	public override void OnInspectorGUI()
	{
		_target = (UIButton)target;

		_type = (UIButton.ButtonType)EditorGUILayout.EnumPopup(_target._type);

		if(_type == UIButton.ButtonType.BOTH)
		{
			_target._textRect = EditorGUILayout.RectField(_target._textRect);
		}

		_target._type = _type;
	}
}*/
