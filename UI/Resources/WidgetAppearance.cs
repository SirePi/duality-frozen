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
    public class WidgetAppearance : Resource
    {
        public new static string FileExt = ".WidgetAppearance" + Resource.FileExt;

        private ContentRef<Appearance> _widgetAppearance;

        public ContentRef<Appearance> Widget
        {
            get { return _widgetAppearance; }
            set { _widgetAppearance = value; }
        }

        public WidgetAppearance()
        {
            _widgetAppearance = new Appearance();
        }

        internal ContentRef<Skin> GetWidgetSkin(Widget.WidgetStatus status)
        {
            return _widgetAppearance.Res.GetSkin(status);
        }
    }
}