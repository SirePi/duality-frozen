using Duality;
using OpenTK.Graphics.OpenGL;

namespace FrozenCore
{
    /// <summary>
    /// Defines a Duality core plugin.
    /// </summary>
    public class FrozenCore : CorePlugin
    {
        internal static readonly string FX_VISUAL_LOG = "FrozenCoreFXVisualLog";

        private static FastRandom _fastRandomInstance;

        public static FastRandom FastRandom
        {
            get
            {
                if (_fastRandomInstance == null)
                {
                    _fastRandomInstance = new FastRandom();
                }

                return _fastRandomInstance;
            }
        }
    }
}