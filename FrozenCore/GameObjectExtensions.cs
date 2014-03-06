// This code is provided under the MIT license. Originally by Alessandro Pilati.

using System;
using System.Collections.Generic;
using Duality;

namespace FrozenCore
{
    public static class GameObjectExtensions
    {
        public static GameObject FindAncestorWithComponent<T>(this GameObject value) where T : Component
        {
            if (value.Parent == null)
            {
                return null;
            }

            return value.Parent.GetComponent<T>() != null ? value.Parent : value.Parent.FindAncestorWithComponent<T>();
        }
    }
}