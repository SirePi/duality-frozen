// This code is provided under the MIT license. Originally by Alessandro Pilati.

using System;
using Duality;
using Duality.Editor;
using Duality.Resources;
using OpenTK;

namespace FrozenCore.Widgets.Resources
{
    [Serializable]
    public class WidgetSkin : Resource
    {
        public static readonly ContentRef<WidgetSkin> DEFAULT = new ContentRef<WidgetSkin>(new WidgetSkin()
        {
            Texture = Duality.Resources.Texture.Checkerboard,
            Size = new Vector2(64, 64),
            Border = Vector4.Zero,
            Origin = new SkinOrigin()
            {
                Active = Vector2.Zero,
                Disabled = Vector2.Zero,
                Hover = Vector2.Zero,
                Normal = Vector2.Zero
            }
        });

        [NonSerialized]
        private BatchInfo _batchInfo;

        private Vector4 _border;
        private SkinOrigin _origin;
        private Vector2 _size;
        private ContentRef<Texture> _texture;

        [EditorHintFlags(MemberFlags.Invisible)]
        public BatchInfo BatchInfo
        {
            get
            {
                if (_batchInfo == null)
                {
                    _batchInfo = new BatchInfo(DrawTechnique.Mask, Colors.White, Texture);
                }

                return _batchInfo;
            }
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
    }
}