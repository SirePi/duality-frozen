﻿// This code is provided under the MIT license. Originally by Alessandro Pilati.

using Duality;
using Duality.Editor;

using SnowyPeak.Duality.Plugin.Frozen.UI.Properties;

namespace SnowyPeak.Duality.Plugin.Frozen.UI.Resources
{
    /// <summary>
    ///
    /// </summary>

    [EditorHintImage(ResNames.ImageSkin)]
    [EditorHintCategory(ResNames.CategoryWidgets)]
    public class MultiLineAppearance : WidgetAppearance
    {
        private ContentRef<ScrollBarAppearance> _scrollAppearance;

        public ContentRef<ScrollBarAppearance> ScrollBar
        {
            get { return _scrollAppearance; }
            set { _scrollAppearance = value; }
        }

        public MultiLineAppearance()
            : base()
        {
            _scrollAppearance = new ScrollBarAppearance();
        }
    }
}