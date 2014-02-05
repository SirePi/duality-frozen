using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenTK;

namespace FrozenCore.Widgets.Skin
{
    [Serializable]
    public struct SkinOrigin
    {
        public Vector2 Normal;
        public Vector2 Hover;
        public Vector2 Active;
        public Vector2 Disabled;
    }
}
