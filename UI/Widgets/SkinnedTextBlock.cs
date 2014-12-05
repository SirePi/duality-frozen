// This code is provided under the MIT license. Originally by Alessandro Pilati.

using System;
using Duality.Editor;
using SnowyPeak.Duality.Plugin.Frozen.UI.Properties;

namespace SnowyPeak.Duality.Plugin.Frozen.UI.Widgets
{
    /// <summary>
    /// A scrollable TextBlock
    /// </summary>
    [Serializable]
    [EditorHintImage(typeof(Res), ResNames.ImageTextBlock)]
    [EditorHintCategory(typeof(Res), ResNames.CategoryWidgets)]
    public class SkinnedTextBlock : SkinnedMultiLineWidget
    {
        private string _text;

        /// <summary>
        /// Constructor
        /// </summary>
        public SkinnedTextBlock()
        {
            ActiveArea = Widgets.ActiveArea.None;
        }

        /// <summary>
        /// [GET / SET] the Text of the TextBlock
        /// </summary>
        public String Text
        {
            get { return _text; }
            set
            {
                _text = value;
                _dirtyFlags |= DirtyFlags.Value;
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="inSecondsPast"></param>
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