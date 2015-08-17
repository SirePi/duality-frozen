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
    public class ListBoxAppearance : MultiLineAppearance
    {
        public new static string FileExt = ".ListBoxAppearance" + Resource.FileExt;

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