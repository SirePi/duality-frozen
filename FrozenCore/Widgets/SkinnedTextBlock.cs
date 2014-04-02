// This code is provided under the MIT license. Originally by Alessandro Pilati.

using System;

namespace FrozenCore.Widgets
{
    [Serializable]
    public class SkinnedTextBlock : SkinnedMultiLineWidget
    {
        [NonSerialized]
        private bool _textChanged;

        private string _text;

        public String Text
        {
            get { return _text; }
            set
            {
                _text = value;
                _textChanged = true;
            }
        }

        public SkinnedTextBlock()
        {
            ActiveArea = Widgets.ActiveArea.None;
            _textChanged = true;
        }

        protected override void OnUpdate(float inSecondsPast)
        {
            if (_textChanged)
            {
                _textChanged = false;

                _fText.SourceText = _text;
                UpdateWidget(true);
            }
        }
    }
}