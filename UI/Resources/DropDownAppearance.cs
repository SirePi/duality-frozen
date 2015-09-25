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
    public class DropDownAppearance : WidgetAppearance
    {

        private ContentRef<ListBoxAppearance> _listBoxAppearance;

        public ContentRef<ListBoxAppearance> ListBox
        {
            get { return _listBoxAppearance; }
            set { _listBoxAppearance = value; }
        }

        public DropDownAppearance()
        {
            _listBoxAppearance = new ListBoxAppearance();
        }
    }
}