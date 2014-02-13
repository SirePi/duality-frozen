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
    public class ScrollBarSkin : BaseSkin
    {
        [NonSerialized]
        private BaseSkin _buttonsSkin;
        [NonSerialized]
        private BaseSkin _sliderSkin;

        public Vector2 ButtonsSize { get; set; }
        public Vector4 ButtonsBorder { get; set; } // X, Y, Z, W = Left, Top, Right, Bottom
        public Vector2 SliderSize { get; set; }
        public Vector4 SliderBorder { get; set; } // X, Y, Z, W = Left, Top, Right, Bottom
        public SkinOrigin ButtonOrigin { get; set; }
        public SkinOrigin SliderOrigin { get; set; }

        public BaseSkin ButtonsSkin
        {
            get
            {
                if (_buttonsSkin == null)
                {
                    _buttonsSkin = new BaseSkin()
                    {
                        Border = ButtonsBorder,
                        Size = ButtonsSize,
                        Texture = Texture,
                        Origin = ButtonOrigin
                    };
                }
                return _buttonsSkin;
            }
        }

        public BaseSkin SliderSkin
        {
            get
            {
                if (_sliderSkin == null)
                {
                    _sliderSkin = new BaseSkin()
                    {
                        Border = SliderBorder,
                        Size = SliderSize,
                        Texture = Texture,
                        Origin = SliderOrigin
                    };
                }
                return _sliderSkin;
            }
        }
    }
}
