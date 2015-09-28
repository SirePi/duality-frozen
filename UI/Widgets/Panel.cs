// This code is provided under the MIT license. Originally by Alessandro Pilati.

using Duality;
using Duality.Editor;
using SnowyPeak.Duality.Plugin.Frozen.UI.Properties;
using SnowyPeak.Duality.Plugin.Frozen.UI.Resources;

namespace SnowyPeak.Duality.Plugin.Frozen.UI.Widgets
{
    /// <summary>
    /// A Panel Widget
    /// </summary>

    [EditorHintImage(ResNames.ImagePanel)]
    [EditorHintCategory(ResNames.CategoryWidgets)]
    public class Panel : Widget
    {
        private ContentRef<WidgetAppearance> _widgetAppearance;

        public ContentRef<WidgetAppearance> Appearance
        {
            get { return _widgetAppearance; }
            set
            {
                _widgetAppearance = value;
                _dirtyFlags |= DirtyFlags.Appearance;
            }
        }

        public Panel()
        {
            Appearance = DefaultGradientSkin.WIDGET;
        }

        protected override Appearance GetBaseAppearance()
        {
            return _widgetAppearance.Res.Widget.Res;
        }
    }
}