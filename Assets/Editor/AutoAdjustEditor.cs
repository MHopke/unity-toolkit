using UnityEditor;
using System.Collections;

[CanEditMultipleObjects]
[CustomEditor(typeof(AutoAdjust))]
public class AutoAdjustEditor : Editor {

	AutoAdjust _target;

	AutoAdjust.ConstraintType _previousType;
	AutoAdjust.ConstraintType _type;

	public void OnEnable()
	{
		_target = (AutoAdjust)target;
		_previousType = _target._constraintType;
	}

	public override void OnInspectorGUI()
	{
		_target = (AutoAdjust)target;
		_target._contraints = EditorGUILayout.RectField(_target._contraints);

		_type = (AutoAdjust.ConstraintType)EditorGUILayout.EnumPopup(_target._constraintType);

		if(_type != _previousType)
		{
			_target.ChangeConstraints(_type);
		}

		_previousType = _type;

		_target._constraintType = _type;
	}
}
