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
    public class WindowAppearance : WidgetAppearance
    {

        private ContentRef<Appearance> _minimizeAppearance;
        private ContentRef<Appearance> _maximizeAppearance;
        private ContentRef<Appearance> _restoreAppearance;
        private ContentRef<Appearance> _closeAppearance;

        private Vector2 _buttonSize;
        private Vector2 _closeSize;

        public Vector2 ButtonSize
        {
            get { return _buttonSize; }
            set { _buttonSize = value; }
        }

        public Vector2 CloseButtonSize
        {
            get { return _closeSize; }
            set { _closeSize = value; }
        }

        public ContentRef<Appearance> Minimize
        {
            get { return _minimizeAppearance; }
            set { _minimizeAppearance = value; }
        }

        public ContentRef<Appearance> Maximize
        {
            get { return _maximizeAppearance; }
            set { _maximizeAppearance = value; }
        }

        public ContentRef<Appearance> Restore
        {
            get { return _restoreAppearance; }
            set { _restoreAppearance = value; }
        }

        public ContentRef<Appearance> Close
        {
            get { return _closeAppearance; }
            set { _closeAppearance = value; }
        }

        public WindowAppearance()
            : base()
        {
            _minimizeAppearance = new Appearance(); 
            _maximizeAppearance = new Appearance();
            _restoreAppearance = new Appearance();
            _closeAppearance = new Appearance();
        }
    }
}