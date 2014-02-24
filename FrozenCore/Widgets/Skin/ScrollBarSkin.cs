// This code is provided under the MIT license. Originally by Alessandro Pilati.

using System;
using OpenTK;
using Duality.Editor;

namespace FrozenCore.Widgets.Skin
{
    [Serializable]
    public class ScrollBarSkin : BaseSkin
    {
        [NonSerialized]
        private BaseSkin _buttonsSkin;

        [NonSerialized]
        private BaseSkin _cursorSkin;

        public SkinOrigin ButtonOrigin { get; set; }
        public Vector4 ButtonsBorder { get; set; }
        public Vector2 ButtonsSize { get; set; }

        [EditorHintFlags(MemberFlags.Invisible)]
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
        public Vector4 CursorBorder { get; set; }
        // X, Y, Z, W = Left, Top, Right, Bottom
        public SkinOrigin CursorOrigin { get; set; }
        // X, Y, Z, W = Left, Top, Right, Bottom
        public Vector2 CursorSize { get; set; }
        [EditorHintFlags(MemberFlags.Invisible)]
        public BaseSkin CursorSkin
        {
            get
            {
                if (_cursorSkin == null)
                {
                    _cursorSkin = new BaseSkin()
                    {
                        Border = CursorBorder,
                        Size = CursorSize,
                        Texture = Texture,
                        Origin = CursorOrigin
                    };
                }
                return _cursorSkin;
            }
        }
    }
}