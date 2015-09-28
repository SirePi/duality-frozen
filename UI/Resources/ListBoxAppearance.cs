// This code is provided under the MIT license. Originally by Alessandro Pilati.

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
    public class ListBoxAppearance : MultiLineAppearance
    {
        private ContentRef<Appearance> _highlightAppearance;

        public ContentRef<Appearance> Highlight
        {
            get { return _highlightAppearance; }
            set { _highlightAppearance = value; }
        }

        public ListBoxAppearance()
            : base()
        {
            _highlightAppearance = new Appearance();
        }
    }
}