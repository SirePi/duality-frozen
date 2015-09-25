// This code is provided under the MIT license. Originally by Alessandro Pilati.

using System;
using Duality;
using Duality.Editor;
using Duality.Resources;

using SnowyPeak.Duality.Plugin.Frozen.UI.Properties;
using SnowyPeak.Duality.Plugin.Frozen.UI.Widgets;

namespace SnowyPeak.Duality.Plugin.Frozen.UI.Resources
{
    /// <summary>
    ///
    /// </summary>
    
    [EditorHintImage(ResNames.ImageSkin)]
    [EditorHintCategory(ResNames.CategoryWidgets)]
    public class ProgressBarAppearance : WidgetAppearance
    {

        private ContentRef<Appearance> _progressBarAppearance;

        public ContentRef<Appearance> ProgressBar
        {
            get { return _progressBarAppearance; }
            set { _progressBarAppearance = value; }
        }

        public ProgressBarAppearance()
        {
            _progressBarAppearance = new Appearance();
        }
    }
}