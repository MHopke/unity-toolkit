using UnityEngine;

using System;
using System.Collections.Generic;

public static class EnumUtility 
{
    public static T ParseEnum<T>( string value )
    {
        return (T) Enum.Parse( typeof( T ), value, true );
    }

	public static Array GetValues<T>() {
		return Enum.GetValues(typeof(T));
	}
}
