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
    public class WindowSkin : BaseSkin
    {
        [NonSerialized]
        private BaseSkin _closeButtonSkin;
        [NonSerialized]
        private BaseSkin _minimizeButtonSkin;
        [NonSerialized]
        private BaseSkin _maximizeButtonSkin;
        [NonSerialized]
        private BaseSkin _restoreButtonSkin;

        public Vector2 ButtonsSize { get; set; }
        public Vector4 ButtonsBorder { get; set; } // X, Y, Z, W = Left, Top, Right, Bottom
        public SkinOrigin CloseButtonOrigin { get; set; }
        public SkinOrigin MinimizeButtonOrigin { get; set; }
        public SkinOrigin MaximizeButtonOrigin { get; set; }
        public SkinOrigin RestoreButtonOrigin { get; set; }

        public BaseSkin CloseButtonSkin
        {
            get
            {
                if (_closeButtonSkin == null)
                {
                    _closeButtonSkin = new BaseSkin()
                    {
                        Border = ButtonsBorder,
                        Size = ButtonsSize,
                        Texture = Texture,
                        Origin = CloseButtonOrigin
                    };
                }
                return _closeButtonSkin;
            }
        }

        public BaseSkin MinimizeButtonSkin
        {
            get
            {
                if (_minimizeButtonSkin == null)
                {
                    _minimizeButtonSkin = new BaseSkin()
                    {
                        Border = ButtonsBorder,
                        Size = ButtonsSize,
                        Texture = Texture,
                        Origin = MinimizeButtonOrigin
                    };
                }
                return _minimizeButtonSkin;
            }
        }

        public BaseSkin MaximizeButtonSkin
        {
            get
            {
                if (_maximizeButtonSkin == null)
                {
                    _maximizeButtonSkin = new BaseSkin()
                    {
                        Border = ButtonsBorder,
                        Size = ButtonsSize,
                        Texture = Texture,
                        Origin = MaximizeButtonOrigin
                    };
                }
                return _maximizeButtonSkin;
            }
        }

        public BaseSkin RestoreButtonSkin
        {
            get
            {
                if (_restoreButtonSkin == null)
                {
                    _restoreButtonSkin = new BaseSkin()
                    {
                        Border = ButtonsBorder,
                        Size = ButtonsSize,
                        Texture = Texture,
                        Origin = RestoreButtonOrigin
                    };
                }
                return _restoreButtonSkin;
            }
        }
    }
}
