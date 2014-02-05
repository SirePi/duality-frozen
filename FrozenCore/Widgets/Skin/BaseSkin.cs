using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Duality.Resources;
using Duality;
using Duality.ColorFormat;
using OpenTK;
using Duality.VertexFormat;

namespace FrozenCore.Widgets.Skin
{
    [Serializable]
    public class BaseSkin : Resource
    {
        private ContentRef<Texture> _texture;
        private Vector2 _size;
        private Vector4 _border;
        private SkinOrigin _origin;

        public ContentRef<Texture> Texture 
        {
            get { return _texture; }
            set { _texture = value; }
        }

        public Vector2 Size
        {
            get { return _size; }
            set { _size = value; }
        }

        public Vector4 Border 
        {
            get { return _border; }
            set { _border = value; }
        }

        public SkinOrigin Origin
        {
            get { return _origin; }
            set { _origin = value; }
        }
    }
}
