// This code is provided under the MIT license. Originally by Alessandro Pilati.

using System;
using System.Collections.Generic;
using Duality.Components;
using Duality;
using OpenTK;

namespace FrozenCore
{
    public static class RectExtensions
    {
        public static Rect Expand(this Rect inRect, float inExpansion)
        {
            return new Rect(inRect.X - inExpansion, inRect.Y - inExpansion, inRect.W + (inExpansion * 2), inRect.H + (inExpansion * 2));
        }
    }
}