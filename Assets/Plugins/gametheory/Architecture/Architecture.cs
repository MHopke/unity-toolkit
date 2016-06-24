using UnityEngine;

using System;
using System.Collections.Generic;

namespace gametheory.Architecture
{
	[Serializable]
	public class Architecture 
	{
		#region Public Vars
		public string Name;
		public string Description;

		public List<MonoBehaviour> Behaviors;
		#endregion
	}

	[AttributeUsage(AttributeTargets.Class,Inherited = true)]
	public class ArchitectureTag : Attribute
	{
		#region Public Vars
		public string Tag;
		#endregion

		#region Constructors
		public ArchitectureTag(){}
		public ArchitectureTag(string tag)
		{
			Tag = tag;
		}
		#endregion
	}
}
