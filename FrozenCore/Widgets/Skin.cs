using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Duality.Resources;
using Duality;
using Duality.ColorFormat;
using OpenTK;
using Duality.VertexFormat;

namespace FrozenCore.Widgets
{
    [Serializable]
    public class Skin : Resource
    {
        public ContentRef<Texture> Texture { get; set; }

        public Vector2 Size { get; set; }
        public Vector4 Border { get; set; } // X, Y, Z, W = Left, Top, Right, Bottom

        public Vector2 NormalOrigin { get; set; }
        public Vector2 HoverOrigin { get; set; }
        public Vector2 ActiveOrigin { get; set; }
        public Vector2 DisabledOrigin { get; set; }
    }
}
