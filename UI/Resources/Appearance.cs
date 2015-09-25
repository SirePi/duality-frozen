// This code is provided under the MIT license. Originally by Alessandro Pilati.

using System;
using Duality;
using Duality.Editor;
using Duality.Resources;
using SnowyPeak.Duality.Plugin.Frozen.UI.Properties;
using SnowyPeak.Duality.Plugin.Frozen.UI.Widgets;
using Duality.Drawing;

namespace SnowyPeak.Duality.Plugin.Frozen.UI.Resources
{
    /// <summary>
    ///
    /// </summary>
    [EditorHintImage(ResNames.ImageSkin)]
    [EditorHintCategory(ResNames.CategoryWidgets)]
    public class Appearance : Resource
    {

        private ContentRef<Skin>[] _skins;

        protected Vector4 _border;

        public Vector4 Border
        {
            get { return _border; }
            set { _border = value; }
        }

        public ContentRef<Skin> Normal
        {
            get { return GetSkin(Widget.WidgetStatus.Normal); }
            set { SetSkin(Widget.WidgetStatus.Normal, value); }
        }

        public ContentRef<Skin> Hover
        {
            get { return GetSkin(Widget.WidgetStatus.Hover); }
            set { SetSkin(Widget.WidgetStatus.Hover, value); }
        }

        public ContentRef<Skin> Active
        {
            get { return GetSkin(Widget.WidgetStatus.Active); }
            set { SetSkin(Widget.WidgetStatus.Active, value); }
        }

        public ContentRef<Skin> Disabled
        {
            get { return GetSkin(Widget.WidgetStatus.Disabled); }
            set { SetSkin(Widget.WidgetStatus.Disabled, value); }
        }

        public Appearance()
        {
            _skins = new ContentRef<Skin>[Enum.GetNames(typeof(Widget.WidgetStatus)).Length];
        }

        internal ContentRef<Skin> GetSkin(Widget.WidgetStatus status)
        {
            return _skins[(int)status];
        }

        private void SetSkin(Widget.WidgetStatus status, ContentRef<Skin> skin)
        {
            _skins[(int)status] = skin;
        }
    }
}