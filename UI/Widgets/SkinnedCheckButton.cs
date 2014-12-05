// This code is provided under the MIT license. Originally by Alessandro Pilati.

using System;
using Duality;
using Duality.Components;
using Duality.Drawing;
using Duality.Editor;
using Duality.Resources;
using OpenTK;
using SnowyPeak.Duality.Plugin.Frozen.Core;
using SnowyPeak.Duality.Plugin.Frozen.UI.Properties;
using SnowyPeak.Duality.Plugin.Frozen.UI.Resources;

namespace SnowyPeak.Duality.Plugin.Frozen.UI.Widgets
{
    /// <summary>
    /// A CheckButton Widget
    /// </summary>
    [Serializable]
    [EditorHintImage(typeof(Res), ResNames.ImageCheckButton)]
    [EditorHintCategory(typeof(Res), ResNames.CategoryWidgets)]
    public class SkinnedCheckButton : SkinnedWidget
    {
        #region NonSerialized fields

        [NonSerialized]
        private FormattedText _fText;

        [NonSerialized]
        private GameObject _glyph;

        #endregion NonSerialized fields

        private object _checkedArgument;
        private ContentRef<WidgetSkin> _glyphSkin;
        private bool _isChecked;
        private ContentRef<Script> _onChecked;
        private ContentRef<Script> _onUnchecked;
        private string _text;
        private Alignment _textAlignment;
        private ColorRgba _textColor;
        private ContentRef<Font> _textFont;
        private object _uncheckedArgument;

        /// <summary>
        /// Constructor
        /// </summary>
        public SkinnedCheckButton()
        {
            ActiveArea = Widgets.ActiveArea.LeftBorder;

            _fText = new FormattedText();
            _textColor = Colors.White;
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
        /// [GET / SET] the skin used for the Glyph
        /// </summary>
        public ContentRef<WidgetSkin> GlyphSkin
        {
            get { return _glyphSkin; }
            set
            {
                _glyphSkin = value;
                _dirtyFlags |= DirtyFlags.Custom1;
            }
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

        /// <summary>
        /// [GET / SET] the Alignment of the Text
        /// </summary>
        public Alignment TextAlignment
        {
            get { return _textAlignment; }
            set { _textAlignment = value; }
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
        public override void MouseDown(OpenTK.Input.MouseButtonEventArgs e)
        {
            if (Status != WidgetStatus.Disabled)
            {
                if (e.Button == OpenTK.Input.MouseButton.Left)
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
        public override void MouseUp(OpenTK.Input.MouseButtonEventArgs e)
        {
            if (e.Button == OpenTK.Input.MouseButton.Left && _isMouseOver)
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
                Vector3 textOrigin = Vector3.Zero;

                if (_textFont.Res != null && _fText.Fonts[0] != _textFont)
                {
                    _fText.Fonts[0] = _textFont;
                }

                _fText.SourceText = _text;

                inCanvas.PushState();
                inCanvas.State.ColorTint = _textColor;
                inCanvas.State.TransformAngle = GameObj.Transform.Angle;

                switch (_textAlignment)
                {
                    case Alignment.Left:
                        textOrigin = (_points[5].WorldCoords + _points[9].WorldCoords) / 2;
                        inCanvas.DrawText(_fText, textOrigin.X, textOrigin.Y, textOrigin.Z + DELTA_Z, null, Alignment.Left);
                        break;

                    case Alignment.Right:
                        textOrigin = (_points[6].WorldCoords + _points[10].WorldCoords) / 2;
                        inCanvas.DrawText(_fText, textOrigin.X, textOrigin.Y, textOrigin.Z + DELTA_Z, null, Alignment.Right);
                        break;

                    default:
                        textOrigin = (_points[5].WorldCoords + _points[10].WorldCoords) / 2;
                        inCanvas.DrawText(_fText, textOrigin.X, textOrigin.Y, textOrigin.Z + DELTA_Z, null, Alignment.Center);
                        break;
                }

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
                _glyph.GetComponent<Widget>().Status = (Status == WidgetStatus.Disabled ? WidgetStatus.Disabled : WidgetStatus.Normal);
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="inSecondsPast"></param>
        protected override void OnUpdate(float inSecondsPast)
        {
            if (_glyph == null && _glyphSkin != null)
            {
                AddGlyph();
            }

            if ((_dirtyFlags & DirtyFlags.Value) != DirtyFlags.None)
            {
                OnCheckUncheck();
            }

            if ((_dirtyFlags & DirtyFlags.Custom1) != DirtyFlags.None && _glyph != null)
            {
                _glyph.GetComponent<SkinnedWidget>().Skin = _glyphSkin;
            }

            if (_glyph != null)
            {
                _glyph.Active = IsChecked;
            }

            base.OnUpdate(inSecondsPast);
        }

        private void AddGlyph()
        {
            _glyph = new GameObject("glyph", this.GameObj);

            Transform t = _glyph.AddComponent<Transform>();
            t.RelativePos = new Vector3(0, 0, 0);
            t.RelativeAngle = 0;

            SkinnedPanel sp = new SkinnedPanel();
            sp.VisibilityGroup = this.VisibilityGroup;
            sp.Skin = GlyphSkin;
            sp.Rect = Rect.AlignTopLeft(0, 0, GlyphSkin.Res.Size.X, GlyphSkin.Res.Size.Y);

            _glyph.AddComponent<SkinnedPanel>(sp);
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
    }
}