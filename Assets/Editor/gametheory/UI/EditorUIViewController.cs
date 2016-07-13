using UnityEditor;
using UnityEngine;

using System.Collections;

namespace gametheory.UI
{
	[CustomEditor(typeof(UIViewController))]
	public class EditorUIViewController : Editor
	{
		#region Private Vars
		UIViewController _controller;
		#endregion

		public override void OnInspectorGUI()
		{
			DrawDefaultInspector();

			_controller = (UIViewController)target;
			if(GUILayout.Button("Assign UIViews"))
			{
				_controller.SetupViewList();
			}
		}
	}
}