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
    public class ProgressBarAppearance : WidgetAppearance
    {
        public new static string FileExt = ".ProgressBarAppearance" + Resource.FileExt;

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