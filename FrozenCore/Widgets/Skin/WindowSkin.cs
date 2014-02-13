using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Duality.Resources;
using Duality;
using Duality.ColorFormat;
using OpenTK;
using Duality.VertexFormat;
using Duality.EditorHints;

namespace FrozenCore.Widgets.Skin
{
    [Serializable]
    public class WindowSkin : BaseSkin
    {
        [NonSerialized]
        private ContentRef<BaseSkin> _closeButtonSkin;
        [NonSerialized]
        private ContentRef<BaseSkin> _minimizeButtonSkin;
        [NonSerialized]
        private ContentRef<BaseSkin> _maximizeButtonSkin;
        [NonSerialized]
        private ContentRef<BaseSkin> _restoreButtonSkin;

        public Vector2 ButtonsSize { get; set; }
        public Vector4 ButtonsBorder { get; set; } // X, Y, Z, W = Left, Top, Right, Bottom
        public SkinOrigin CloseButtonOrigin { get; set; }
        public SkinOrigin MinimizeButtonOrigin { get; set; }
        public SkinOrigin MaximizeButtonOrigin { get; set; }
        public SkinOrigin RestoreButtonOrigin { get; set; }

        [EditorHintFlags(MemberFlags.Invisible)]
        public ContentRef<BaseSkin> CloseButtonSkin
        {
            get
            {
                if (_closeButtonSkin == null)
                {
                    _closeButtonSkin = new ContentRef<BaseSkin>(new BaseSkin()
                    {
                        Border = ButtonsBorder,
                        Size = ButtonsSize,
                        Texture = Texture,
                        Origin = CloseButtonOrigin
                    });
                }
                return _closeButtonSkin;
            }
        }

        [EditorHintFlags(MemberFlags.Invisible)]
        public ContentRef<BaseSkin> MinimizeButtonSkin
        {
            get
            {
                if (_minimizeButtonSkin == null)
                {
                    _minimizeButtonSkin = new ContentRef<BaseSkin>(new BaseSkin()
                    {
                        Border = ButtonsBorder,
                        Size = ButtonsSize,
                        Texture = Texture,
                        Origin = MinimizeButtonOrigin
                    });
                }
                return _minimizeButtonSkin;
            }
        }

        [EditorHintFlags(MemberFlags.Invisible)]
        public ContentRef<BaseSkin> MaximizeButtonSkin
        {
            get
            {
                if (_maximizeButtonSkin == null)
                {
                    _maximizeButtonSkin = new ContentRef<BaseSkin>(new BaseSkin()
                    {
                        Border = ButtonsBorder,
                        Size = ButtonsSize,
                        Texture = Texture,
                        Origin = MaximizeButtonOrigin
                    });
                }
                return _maximizeButtonSkin;
            }
        }

        [EditorHintFlags(MemberFlags.Invisible)]
        public ContentRef<BaseSkin> RestoreButtonSkin
        {
            get
            {
                if (_restoreButtonSkin == null)
                {
                    _restoreButtonSkin = new ContentRef<BaseSkin>(new BaseSkin()
                    {
                        Border = ButtonsBorder,
                        Size = ButtonsSize,
                        Texture = Texture,
                        Origin = RestoreButtonOrigin
                    });
                }
                return _restoreButtonSkin;
            }
        }
    }
}
