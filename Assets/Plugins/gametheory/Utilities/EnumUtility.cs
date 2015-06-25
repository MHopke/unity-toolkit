using UnityEngine;

public static class EnumUtility 
{
    public static T ParseEnum<T>( string value )
    {
        return (T) System.Enum.Parse( typeof( T ), value, true );
    }
}
