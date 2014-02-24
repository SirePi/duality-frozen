// This code is provided under the MIT license. Originally by Alessandro Pilati.

using System;
using Duality;
using Duality.Resources;
using OpenTK;
using Duality.Editor;

namespace FrozenCore.Widgets.Skin
{
    [Serializable]
    public class ListBoxSkin : BaseSkin
    {
        [NonSerialized]
        private BaseSkin _selectorSkin;

        public SkinOrigin SelectorOrigin { get; set; }
        public Vector4 SelectorBorder { get; set; }
        public Vector2 SelectorSize { get; set; }

        [EditorHintFlags(MemberFlags.Invisible)]
        public BaseSkin SelectorSkin
        {
            get
            {
                if (_selectorSkin == null)
                {
                    _selectorSkin = new BaseSkin()
                    {
                        Border = SelectorBorder,
                        Size = SelectorSize,
                        Texture = Texture,
                        Origin = SelectorOrigin
                    };
                }
                return _selectorSkin;
            }
        }
    }
}