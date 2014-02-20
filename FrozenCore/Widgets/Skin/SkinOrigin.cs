// This code is provided under the MIT license. Originally by Alessandro Pilati.

using System;
using OpenTK;

namespace FrozenCore.Widgets.Skin
{
    [Serializable]
    public struct SkinOrigin
    {
        public Vector2 Active;
        public Vector2 Disabled;
        public Vector2 Hover;
        public Vector2 Normal;
    }
}