using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Duality;

namespace FrozenCore
{
    public static class FrozenUtilities
    {
        public static bool IsDualityEditor
        {
            get { return (DualityApp.ExecEnvironment == DualityApp.ExecutionEnvironment.Editor && DualityApp.ExecContext == DualityApp.ExecutionContext.Editor); }
        }
    }
}
