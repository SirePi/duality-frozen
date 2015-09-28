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
    public class ScrollBarAppearance : WidgetAppearance
    {
        private ContentRef<Appearance> _increaseAppearance;
        private ContentRef<Appearance> _decreaseAppearance;
        private ContentRef<Appearance> _cursorAppearance;

        private Vector2 _buttonSize;
        private Vector2 _cursorSize;

        public Vector2 ButtonSize
        {
            get { return _buttonSize; }
            set { _buttonSize = value; }
        }

        public Vector2 CursorSize
        {
            get { return _cursorSize; }
            set { _cursorSize = value; }
        }

        public ContentRef<Appearance> Increase
        {
            get { return _increaseAppearance; }
            set { _increaseAppearance = value; }
        }

        public ContentRef<Appearance> Decrease
        {
            get { return _decreaseAppearance; }
            set { _decreaseAppearance = value; }
        }

        public ContentRef<Appearance> Cursor
        {
            get { return _cursorAppearance; }
            set { _cursorAppearance = value; }
        }

        public ScrollBarAppearance()
            : base()
        {
            _increaseAppearance = new Appearance();
            _decreaseAppearance = new Appearance();
            _cursorAppearance = new Appearance();
        }
    }
}