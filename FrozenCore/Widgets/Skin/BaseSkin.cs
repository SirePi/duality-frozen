// This code is provided under the MIT license. Originally by Alessandro Pilati.

using System;
using Duality;
using Duality.Resources;
using OpenTK;

namespace FrozenCore.Widgets.Skin
{
    [Serializable]
    public class BaseSkin : Resource
    {
        public static readonly BaseSkin NO_SKIN = new BaseSkin()
        {
            Texture = Duality.Resources.Texture.None,
            Size = Vector2.One,
            Border = Vector4.Zero,
            Origin = new SkinOrigin()
            {
                Active = Vector2.Zero,
                Disabled = Vector2.Zero,
                Hover = Vector2.Zero,
                Normal = Vector2.Zero
            }
        };

        public static readonly BaseSkin WHITE_SKIN = new BaseSkin()
        {
            Texture = Duality.Resources.Texture.Checkerboard,
            Size = Vector2.One,
            Border = Vector4.Zero,
            Origin = new SkinOrigin()
            {
                Active = Vector2.Zero,
                Disabled = Vector2.Zero,
                Hover = Vector2.Zero,
                Normal = Vector2.Zero
            }
        };

        private Vector4 _border;
        private SkinOrigin _origin;
        private Vector2 _size;
        private ContentRef<Texture> _texture;
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
        public Vector2 Size
        {
            get { return _size; }
            set { _size = value; }
        }
        public ContentRef<Texture> Texture
        {
            get { return _texture; }
            set { _texture = value; }
        }

        public static ContentRef<T> GetWhiteSkin<T>() where T : BaseSkin
        {
            return new ContentRef<T>(WHITE_SKIN as T);
        }
    }
}