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

        public Vector2 NormalTopLeft { get; set; }
        public Vector2 HoverTopLeft { get; set; }
        public Vector2 ActiveTopLeft { get; set; }
        public Vector2 DisabledTopLeft { get; set; }

        /// <summary>
        /// X, Y, Z, W = Left, Top, Right, Bottom
        /// </summary>
        public Vector4 Border { get; set; }
    }
}
