// This code is provided under the MIT license. Originally by Alessandro Pilati.

using System;
using Duality.Editor;
using SnowyPeak.Duality.Plugin.Frozen.UI.Properties;
using SnowyPeak.Duality.Plugin.Frozen.UI.Resources;
using Duality;

namespace SnowyPeak.Duality.Plugin.Frozen.UI.Widgets
{
    /// <summary>
    /// A scrollable TextBlock
    /// </summary>
    
    [EditorHintImage(ResNames.ImageTextBlock)]
    [EditorHintCategory(ResNames.CategoryWidgets)]
    public class TextBlock : MultiLineWidget
    {
        private string _text;

        /// <summary>
        /// [GET / SET] the Skin that will be used for the Scrollbar
        /// </summary>
        protected ContentRef<MultiLineAppearance> Appearance
        {
            set
            {
                _multiAppearance = value;
                _dirtyFlags |= DirtyFlags.Appearance;
            }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public TextBlock()
        {
            ActiveArea = ActiveArea.None;
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