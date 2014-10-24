// This code is provided under the MIT license. Originally by Alessandro Pilati.

using System;

namespace FrozenCore.Widgets
{
    [Serializable]
    public class SkinnedTextBlock : SkinnedMultiLineWidget
    {
        private string _text;

        public String Text
        {
            get { return _text; }
            set
            {
                _text = value;
                _dirtyFlags |= DirtyFlags.Value;
            }
        }

        public SkinnedTextBlock()
        {
            ActiveArea = Widgets.ActiveArea.None;
        }

        protected override void OnUpdate(float inSecondsPast)
        {
            base.OnUpdate(inSecondsPast);

            if ((_dirtyFlags & DirtyFlags.Value) != DirtyFlags.None)
            {
                _fText.SourceText = _text;
                UpdateWidget(true);
            }
        }
    }
}