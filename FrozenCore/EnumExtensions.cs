// This code is provided under the MIT license. Originally by Alessandro Pilati.

using System;
using System.Collections.Generic;

namespace FrozenCore
{
    /// <summary>
    /// http://leonardinius.galeoconsulting.com/2012/06/howto-c-enums-with-additional-attributes-java-enum-gaps-covered/
    /// </summary>
    public static class EnumExtensions
    {
        public static TAttribute AttributeOf<TAttribute>(this Enum @enum)
            where TAttribute : Attribute
        {
            return (TAttribute)GetFirstOrNull(GetEnumValueAttributes<TAttribute>(@enum));
        }

        private static object[] GetEnumValueAttributes<TAttribute>(Enum @enum)
            where TAttribute : Attribute
        {
            return @enum.GetType()
                .GetField(Enum.GetName(@enum.GetType(), @enum))
                .GetCustomAttributes(typeof(TAttribute), false);
        }

        private static T GetFirstOrNull<T>(IList<T> array)
            where T : class
        {
            return array == null || array.Count <= 0 ? null : array[0];
        }
    }
}