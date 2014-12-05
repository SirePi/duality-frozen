// This code is provided under the MIT license. Originally by Alessandro Pilati.

using System;
using System.Collections.Generic;
using Duality;

namespace SnowyPeak.Duality.Plugin.Frozen.Core
{
    /// <summary>
    /// GameObject extensions
    /// </summary>
    public static class GameObjectExtensions
    {
        /// <summary>
        /// Travels backwards through ancestors until one with the desired Component is found
        /// </summary>
        /// <typeparam name="T">The Type of the Component that should be looked for</typeparam>
        /// <param name="value">The caller</param>
        /// <returns>The first GameObject in the caller's ancestry containing a Component of the required type.
        /// null otherwise</returns>
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