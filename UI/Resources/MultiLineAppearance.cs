// This code is provided under the MIT license. Originally by Alessandro Pilati.

using System;
using Duality;
using Duality.Editor;
using Duality.Resources;
using OpenTK;
using SnowyPeak.Duality.Plugin.Frozen.UI.Properties;
using SnowyPeak.Duality.Plugin.Frozen.UI.Widgets;

namespace SnowyPeak.Duality.Plugin.Frozen.UI.Resources
{
    /// <summary>
    ///
    /// </summary>
    [Serializable]
    [EditorHintImage(typeof(Res), ResNames.ImageSkin)]
    [EditorHintCategory(typeof(Res), ResNames.CategoryWidgets)]
    public class MultiLineAppearance : WidgetAppearance
    {
        public new static string FileExt = ".MultiLineAppearance" + Resource.FileExt;

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