// This code is provided under the MIT license. Originally by Alessandro Pilati.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Duality;
using FrozenCore.Components;
using Duality.Components.Renderers;
using OpenTK;
using Duality.Resources;
using FrozenCore.Widgets.Skin;
using Duality.EditorHints;
using Duality.ColorFormat;
using Duality.Components;

namespace FrozenCore.Widgets
{
    [Serializable]
    public class SkinnedTextBox : SkinnedWidget<BaseSkin>
    {
        private FormattedText _text;
        private ColorRgba _textColor;

        [NonSerialized]
        private float _secondsFromLastTick;
        [NonSerialized]
        private string _lastText;
        [NonSerialized]
        private GameObject _caret;

        public ColorRgba TextColor
        {
            get { return _textColor; }
            set { _textColor = value; }
        }

        public FormattedText Text
        {
            get { return _text; }
            set { _text = value; }
        }

        public SkinnedTextBox()
        {
            ActiveArea = Widgets.ActiveArea.Center;

            _text = new FormattedText();
            _textColor = Colors.White;
        }

        protected override void DrawCanvas(IDrawDevice inDevice, Canvas inCanvas)
        {
            if (Text != null)
            {
                Vector3 textLeft = (_points[5].WorldCoords + _points[9].WorldCoords) / 2;

                inCanvas.PushState();
                inCanvas.State.ColorTint = _textColor;
                inCanvas.State.TransformAngle = GameObj.Transform.Angle;
                inCanvas.DrawText(Text, textLeft.X, textLeft.Y, textLeft.Z + DELTA_Z, null, Alignment.Left);

                inCanvas.PopState();
            }
        }

        protected override void OnInit(Component.InitContext inContext)
        {
            base.OnInit(inContext);

            if (inContext == InitContext.Activate && !FrozenUtilities.IsDualityEditor)
            {
                AddCaret();

                UpdateCaret();
            }
        }

        private void AddCaret()
        {
            _caret = new GameObject("caret", this.GameObj);

            Transform t = _caret.AddComponent<Transform>();
            t.RelativePos = new Vector3(Rect.W / 2, Rect.H / 2, DELTA_Z);
            t.RelativeAngle = 0;

            SpriteRenderer sr = new SpriteRenderer();
            sr.VisibilityGroup = this.VisibilityGroup;
            sr.Rect = Rect.AlignCenter(0, 0, 3, Text.TextMetrics.Size.Y);
            sr.SharedMaterial = Material.InvertWhite;

            _caret.AddComponent<SpriteRenderer>(sr);
            Scene.Current.AddObject(_caret);
        }

        private void UpdateCaret()
        {
            if(_caret != null)
            {
                Vector3 caretPosition = _caret.Transform.RelativePos;
                caretPosition.X = Skin.Res.Border.X + _text.Size.X;

                _caret.Transform.RelativePos = caretPosition;
            }
        }

        protected override void OnUpdate(float inSecondsPast)
        {
            if (IsWidgetActive)
            {
                if (_text.SourceText != _lastText)
                {
                    _lastText = _text.SourceText;
                    UpdateCaret();
                }

                _secondsFromLastTick += inSecondsPast;
                if (_secondsFromLastTick > .5)
                {
                    _secondsFromLastTick = 0;
                    _caret.Active = !_caret.Active;
                }
            }
            else
            {
                _caret.Active = false;
            }
        }

        internal override void KeyDown(OpenTK.Input.KeyboardKeyEventArgs e, WidgetController.ModifierKeys k)
        {
            base.KeyDown(e, k);

            if (e.Key == OpenTK.Input.Key.BackSpace)
            {
                if(_text.SourceText.Length > 0)
                {
                    _text.SourceText = _text.SourceText.Substring(0, _text.SourceText.Length - 1);
                }
            }
            else if (e.Key >= OpenTK.Input.Key.A && e.Key <= OpenTK.Input.Key.Z)
            {
                string c = e.Key.ToString();
                if ((k & WidgetController.ModifierKeys.Shift) == 0)
                {
                    c = c.ToLower();
                }

                _text.SourceText += c;
            }
            else if (e.Key == OpenTK.Input.Key.Space)
            {
                _text.SourceText += " ";
            }
            else if (e.Key >= OpenTK.Input.Key.Number0 && e.Key <= OpenTK.Input.Key.Number9)
            {
                int digit = e.Key - OpenTK.Input.Key.Number0;
                _text.SourceText += digit.ToString();
            }
            else if (e.Key >= OpenTK.Input.Key.Keypad0 && e.Key <= OpenTK.Input.Key.Keypad9)
            {
                int digit = e.Key - OpenTK.Input.Key.Keypad0;
                _text.SourceText += digit.ToString();
            }
            else 
            {
                switch(e.Key)
                {
                    case OpenTK.Input.Key.Plus:
                    case OpenTK.Input.Key.KeypadPlus:
                        _text.SourceText += "+";
                        break;

                    case OpenTK.Input.Key.Minus:
                    case OpenTK.Input.Key.KeypadMinus:
                        _text.SourceText += "-";
                        break;

                    case OpenTK.Input.Key.KeypadMultiply:
                        _text.SourceText += "*";
                        break;

                    case OpenTK.Input.Key.Comma:
                        _text.SourceText += ",";
                        break;

                    case OpenTK.Input.Key.Period:
                    case OpenTK.Input.Key.KeypadPeriod:
                        _text.SourceText += ".";
                        break;
                }
            }
        }
    }
}
