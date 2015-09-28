// This code is provided under the MIT license. Originally by Alessandro Pilati.

using Duality;
using Duality.Components;
using Duality.Drawing;
using Duality.Editor;
using Duality.Input;
using Duality.Resources;
using SnowyPeak.Duality.Plugin.Frozen.Core;
using SnowyPeak.Duality.Plugin.Frozen.UI.Properties;
using SnowyPeak.Duality.Plugin.Frozen.UI.Resources;
using System;

namespace SnowyPeak.Duality.Plugin.Frozen.UI.Widgets
{
    /// <summary>
    /// A CheckButton Widget
    /// </summary>

    [EditorHintImage(ResNames.ImageCheckButton)]
    [EditorHintCategory(ResNames.CategoryWidgets)]
    public class CheckButton : Widget
    {
        #region NonSerialized fields

        [DontSerialize]
        private FormattedText _fText;

        [DontSerialize]
        private GameObject _glyph;

        #endregion NonSerialized fields

        private object _checkedArgument;
        private bool _isChecked;
        private ContentRef<Script> _onChecked;
        private ContentRef<Script> _onUnchecked;
        private string _text;
        private ColorRgba _textColor;
        private ContentRef<Font> _textFont;
        private object _uncheckedArgument;
        private Alignment _glyphLocation;

        private ContentRef<GlyphAppearance> _glyphAppearance;

        public ContentRef<GlyphAppearance> Appearance
        {
            get { return _glyphAppearance; }
            set
            {
                _glyphAppearance = value;
                _dirtyFlags |= DirtyFlags.Appearance;
            }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public CheckButton()
        {
            ActiveArea = ActiveArea.LeftBorder;

            _fText = new FormattedText();
            _textColor = Colors.White;
            _glyphLocation = Alignment.Left;

            Appearance = DefaultGradientSkin.GLYPH;
        }

        /// <summary>
        ///
        /// </summary>
        public object CheckedArgument
        {
            get { return _checkedArgument; }
            set { _checkedArgument = value; }
        }

        /// <summary>
        /// [GET / SET] if the Button is Checked
        /// </summary>
        public bool IsChecked
        {
            get { return _isChecked; }
            set
            {
                if (_isChecked != value)
                {
                    _isChecked = value;
                    _dirtyFlags |= DirtyFlags.Value;
                }
            }
        }

        /// <summary>
        ///
        /// </summary>
        public ContentRef<Script> OnChecked
        {
            get { return _onChecked; }
            set { _onChecked = value; }
        }

        /// <summary>
        ///
        /// </summary>
        public ContentRef<Script> OnUnchecked
        {
            get { return _onUnchecked; }
            set { _onUnchecked = value; }
        }

        /// <summary>
        /// [GET / SET] the Text of the Button
        /// </summary>
        public string Text
        {
            get { return _text; }
            set { _text = value; }
        }

        public Alignment GlyphLocation
        {
            get { return _glyphLocation; }
            set { _glyphLocation = value; }
        }

        /// <summary>
        /// [GET / SET] the Color of the Text
        /// </summary>
        public ColorRgba TextColor
        {
            get { return _textColor; }
            set { _textColor = value; }
        }

        /// <summary>
        /// [GET / SET] the Font of the Text
        /// </summary>
        public ContentRef<Font> TextFont
        {
            get { return _textFont; }
            set { _textFont = value; }
        }

        /// <summary>
        ///
        /// </summary>
        public object UncheckedArgument
        {
            get { return _uncheckedArgument; }
            set { _uncheckedArgument = value; }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="e"></param>
        public override void MouseDown(MouseButtonEventArgs e)
        {
            if (Status != WidgetStatus.Disabled)
            {
                if (e.Button == MouseButton.Left)
                {
                    Status = WidgetStatus.Active;
                }
            }
        }

        /// <summary>
        ///
        /// </summary>
        public override void MouseEnter()
        {
            _isMouseOver = true;

            if (Status != WidgetStatus.Disabled)
            {
                Status = WidgetStatus.Hover;
            }
        }

        /// <summary>
        ///
        /// </summary>
        public override void MouseLeave()
        {
            _isMouseOver = false;

            if (Status != WidgetStatus.Disabled)
            {
                Status = WidgetStatus.Normal;
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="e"></param>
        public override void MouseUp(MouseButtonEventArgs e)
        {
            if (e.Button == MouseButton.Left && _isMouseOver)
            {
                IsChecked = !IsChecked;
                Status = _isMouseOver ? WidgetStatus.Hover : WidgetStatus.Normal;

                if (IsChecked && OnChecked.Res != null)
                {
                    OnChecked.Res.Execute(this.GameObj, CheckedArgument);
                }
                if (!IsChecked && OnUnchecked.Res != null)
                {
                    OnUnchecked.Res.Execute(this.GameObj, UncheckedArgument);
                }
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="inCanvas"></param>
        protected override void DrawCanvas(Canvas inCanvas)
        {
            if (!String.IsNullOrWhiteSpace(_text))
            {
                if (_textFont.Res != null && _fText.Fonts[0] != _textFont)
                {
                    _fText.Fonts[0] = _textFont;
                }

                _fText.SourceText = _text;
                Vector3 buttonCenter = (_points[5].WorldCoords + _points[10].WorldCoords) / 2;

                inCanvas.PushState();

                inCanvas.State.ColorTint = _textColor;
                inCanvas.State.TransformAngle = GameObj.Transform.Angle;
                inCanvas.DrawText(_fText, buttonCenter.X, buttonCenter.Y, buttonCenter.Z + DELTA_Z, null, Alignment.Center);

                inCanvas.PopState();
            }
        }

        /// <summary>
        ///
        /// </summary>
        protected override void OnStatusChange()
        {
            base.OnStatusChange();

            if (_glyph != null)
            {
                _glyph.GetComponent<Panel>().Status = (Status == WidgetStatus.Disabled ? WidgetStatus.Disabled : WidgetStatus.Normal);
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="inSecondsPast"></param>
        protected override void OnUpdate(float inSecondsPast)
        {
            if (_glyph == null && !_glyphAppearance.Res.Glyph.IsExplicitNull)
            {
                AddGlyph();
            }

            if ((_dirtyFlags & DirtyFlags.Value) != DirtyFlags.None)
            {
                OnCheckUncheck();
            }

            if (_glyph != null)
            {
                if ((_dirtyFlags & DirtyFlags.Appearance) != DirtyFlags.None)
                {
                    Panel p = _glyph.GetComponent<Panel>();
                    p.Appearance = AppearanceManager.RequestAppearanceContentRef(_glyphAppearance.Res.Glyph);
                    p.Rect = Rect.Align(Alignment.TopLeft, 0, 0, _glyphAppearance.Res.GlyphSize.X, _glyphAppearance.Res.GlyphSize.Y);
                }

                _glyph.Active = IsChecked;
            }
        }

        private void AddGlyph()
        {
            _glyph = new GameObject("glyph", this.GameObj);

            Transform t = _glyph.AddComponent<Transform>();
            t.RelativePos = new Vector3(0, 0, 0);
            t.RelativeAngle = 0;

            Panel p = new Panel();
            p.VisibilityGroup = this.VisibilityGroup;
            p.Appearance = AppearanceManager.RequestAppearanceContentRef(_glyphAppearance.Res.Glyph);
            p.Rect = Rect.Align(Alignment.Center, 0, 0, _glyphAppearance.Res.GlyphSize.X, _glyphAppearance.Res.GlyphSize.Y);

            _glyph.AddComponent<Panel>(p);
            Scene.Current.AddObject(_glyph);
        }

        private void OnCheckUncheck()
        {
            if (IsChecked)
            {
                if (OnChecked.Res != null)
                {
                    OnChecked.Res.Execute(this.GameObj, UncheckedArgument);
                }
            }
            else
            {
                if (OnUnchecked.Res != null)
                {
                    OnUnchecked.Res.Execute(this.GameObj, UncheckedArgument);
                }
            }
        }

        protected override Appearance GetBaseAppearance()
        {
            return _glyphAppearance.Res.Widget.Res;
        }
    }
}