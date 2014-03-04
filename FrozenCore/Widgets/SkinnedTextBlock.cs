// This code is provided under the MIT license. Originally by Alessandro Pilati.

using System;
using Duality;
using Duality.Drawing;
using Duality.Components;
using Duality.Resources;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace FrozenCore.Widgets
{
    [Serializable]
    public class SkinnedTextBlock : SkinnedMultiLineWidget
    {
        public ObservableFormattedText Text
        {
            get { return _text as ObservableFormattedText; }
            set { _text = value; }
        }

        public SkinnedTextBlock()
        {
            ActiveArea = Widgets.ActiveArea.None;

            _text = new ObservableFormattedText();
        }

        protected override void OnUpdate(float inSecondsPast)
        {
            if (Text.TextChanged)
            {
                Text.TextChanged = false; 
                UpdateWidget(true);
            }
        }
    }
}