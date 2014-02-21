// This code is provided under the MIT license. Originally by Alessandro Pilati.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Duality;
using OpenTK;
using OpenTK.Input;

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

        private static readonly int[] POWER_OF_TWO_SIZES = new int[] { 
            1, 2, 4, 8, 16, 32, 64, 128, 256, 512, 
            1024, 2048, 4096, 8192, 16384, 32768, 65536, 131072, 262144, 524288, 1048576 };

        public static int GetNextPowerOfTwo (float inValue)
        {
            return POWER_OF_TWO_SIZES.First(s => s > inValue);
        }
    }
}
