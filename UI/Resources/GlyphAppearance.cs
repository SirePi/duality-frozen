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
    public class GlyphAppearance : WidgetAppearance
    {

        private ContentRef<Appearance> _glyphAppearance;

        private Vector2 _glyphSize;

        public Vector2 GlyphSize
        {
            get { return _glyphSize; }
            set { _glyphSize = value; }
        }

        public ContentRef<Appearance> Glyph
        {
            get { return _glyphAppearance; }
            set { _glyphAppearance = value; }
        }

        public GlyphAppearance()
        {
            _glyphAppearance = new Appearance();
        }
    }
}