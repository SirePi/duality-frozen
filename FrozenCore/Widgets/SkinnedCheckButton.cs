// This code is provided under the MIT license. Originally by Alessandro Pilati.

using System;
using Duality;
using Duality.Drawing;
using OpenTK;
using Duality.Resources;
using FrozenCore.Resources.Widgets;
using Duality.Components;

namespace FrozenCore.Widgets
{
    [Serializable]
    public class SkinnedCheckButton : SkinnedWidget
    {
        #region NonSerialized fields
        [NonSerialized]
        private FormattedText _fText;
        [NonSerialized]
        private GameObject _glyph;
        #endregion

        private ContentRef<WidgetSkin> _glyphSkin;
        private object _checkedArgument;
        private bool _isChecked;
        private ContentRef<Script> _onChecked;
        private ContentRef<Script> _onUnchecked;
        private string _text;
        private ContentRef<Font> _textFont;
        private ColorRgba _textColor;
        private object _uncheckedArgument;

        public object CheckedArgument
        {
            get { return _checkedArgument; }
            set { _checkedArgument = value; }
        }

        public bool IsChecked
        {
            get { return _isChecked; }
            set
            {
                _isChecked = value;
                OnCheckUncheck();
                OnStatusChange();
            }
        }

        public ContentRef<WidgetSkin> GlyphSkin
        {
            get { return _glyphSkin; }
            set { _glyphSkin = value; }
        }

        public ContentRef<Script> OnChecked
        {
            get { return _onChecked; }
            set { _onChecked = value; }
        }

        public ContentRef<Script> OnUnchecked
        {
            get { return _onUnchecked; }
            set { _onUnchecked = value; }
        }

        public string Text
        {
            get { return _text; }
            set { _text = value; }
        }

        public ContentRef<Font> TextFont
        {
            get { return _textFont; }
            set { _textFont = value; }
        }

        public ColorRgba TextColor
        {
            get { return _textColor; }
            set { _textColor = value; }
        }

        public object UncheckedArgument
        {
            get { return _uncheckedArgument; }
            set { _uncheckedArgument = value; }
        }

        public SkinnedCheckButton()
        {
            ActiveArea = Widgets.ActiveArea.LeftBorder;

            _fText = new FormattedText();
            _textColor = Colors.White;
        }

        public override void MouseLeave()
        {
            _isMouseOver = false;

            if (Status != WidgetStatus.Disabled)
            {
                Status = WidgetStatus.Normal;
            }
        }

        public override void MouseDown(OpenTK.Input.MouseButtonEventArgs e)
        {
            if (e.Button == OpenTK.Input.MouseButton.Left)
            {
                Status = WidgetStatus.Active;
            }
        }

        public override void MouseUp(OpenTK.Input.MouseButtonEventArgs e)
        {
            if (e.Button == OpenTK.Input.MouseButton.Left && _isMouseOver)
            {
                IsChecked = !IsChecked;
                Status = WidgetStatus.Hover;

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

        public override void MouseEnter()
        {
            _isMouseOver = true;

            if (Status != WidgetStatus.Disabled)
            {
                Status = WidgetStatus.Hover;
            }
        }

        protected override void DrawCanvas(Canvas inCanvas)
        {
            if (!String.IsNullOrWhiteSpace(_text))
            {
                Vector3 buttonCenter = (_points[5].WorldCoords + _points[10].WorldCoords) / 2;
                if (_textFont.Res != null && _fText.Fonts[0] != _textFont)
                {
                    _fText.Fonts[0] = _textFont;
                }

                _fText.SourceText = _text;

                inCanvas.PushState();
                inCanvas.State.ColorTint = _textColor;
                inCanvas.State.TransformAngle = GameObj.Transform.Angle;
                inCanvas.DrawText(_fText, buttonCenter.X, buttonCenter.Y, buttonCenter.Z + DELTA_Z, null, Alignment.Center);
                inCanvas.PopState();
            }
        }

        protected override void OnInit(Component.InitContext inContext)
        {
            base.OnInit(inContext);

            if (inContext == InitContext.Activate && !FrozenUtilities.IsDualityEditor)
            {
                if (_glyph == null)
                {
                    AddGlyph();

                    //_glyph.GetComponent<Widget>().Active = IsChecked;
                }
            }
        }

        private void AddGlyph()
        {
            _glyph = new GameObject("glyph", this.GameObj);

            Transform t = _glyph.AddComponent<Transform>();
            t.RelativePos = new Vector3(0, 0, 0);
            t.RelativeAngle = 0;

            SkinnedPanel sp = new SkinnedPanel();
            sp.ActiveArea = Widgets.ActiveArea.None;
            sp.VisibilityGroup = this.VisibilityGroup;
            sp.Skin = GlyphSkin;
            sp.Rect = Rect.AlignTopLeft(0, 0, GlyphSkin.Res.Size.X, GlyphSkin.Res.Size.Y);

            _glyph.AddComponent<SkinnedPanel>(sp);
            Scene.Current.AddObject(_glyph);
        }

        private void OnCheckUncheck()
        {
            if (_glyph != null)
            {
                _glyph.Active = IsChecked;
            }

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
            //MouseLeave();
        }

        protected override void OnStatusChange()
        {
            base.OnStatusChange();
            
            if (_glyph != null)
            {
                _glyph.Active = IsChecked;

                _glyph.GetComponent<Widget>().Status = (Status == WidgetStatus.Disabled ? WidgetStatus.Disabled : WidgetStatus.Normal);
            }
        }
    }
}