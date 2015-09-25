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
    public class WidgetAppearance : Resource
    {

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