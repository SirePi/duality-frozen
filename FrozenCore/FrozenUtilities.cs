// This code is provided under the MIT license. Originally by Alessandro Pilati.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Duality;
using OpenTK;

namespace FrozenCore
{
    public static class FrozenUtilities
    {
        public static bool IsDualityEditor
        {
            get { return (DualityApp.ExecEnvironment == DualityApp.ExecutionEnvironment.Editor && DualityApp.ExecContext == DualityApp.ExecutionContext.Editor); }
        }

        public static void DrawCross(Canvas inCanvas, Vector3 inPosition, float inRadius)
        {
            inCanvas.DrawLine(inPosition.X - inRadius, inPosition.Y, inPosition.X + inRadius, inPosition.Y);
            inCanvas.DrawLine(inPosition.X, inPosition.Y - inRadius, inPosition.X, inPosition.Y + inRadius);
        }
    }
}
