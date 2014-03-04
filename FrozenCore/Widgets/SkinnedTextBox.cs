// This code is provided under the MIT license. Originally by Alessandro Pilati.

using System;
using Duality;
using Duality.Components;
using Duality.Components.Renderers;
using Duality.Drawing;
using Duality.Resources;
using OpenTK;
using Duality.Editor;

namespace FrozenCore.Widgets
{
    [Serializable]
    public class SkinnedTextBox : SkinnedWidget
    {
        #region NonSerialized fields

        [NonSerialized]
        private static readonly float DEFAULT_KEY_REPEAT = .2f;

        [NonSerialized]
        private static readonly float CARET_TICK = .5f;

        [NonSerialized]
        private GameObject _caret;

        [NonSerialized]
        private string _lastText;

        [NonSerialized]
        private float _secondsFromLastTick;

        [NonSerialized]
        private float _secondsFromLastKey;

        [NonSerialized]
        private OpenTK.Input.Key? _keyDown;

        [NonSerialized]
        private WidgetController.ModifierKeys _modifierKeys;

        #endregion NonSerialized fields

        private FormattedText _text;
        private ColorRgba _textColor;
        private float _keyRepeatSpeed;

        public FormattedText Text
        {
            get { return _text; }
            set { _text = value; }
        }
        public ColorRgba TextColor
        {
            get { return _textColor; }
            set { _textColor = value; }
        }
        [EditorHintRange(0.1f, 1)]
        public float KeyRepeatSpeed
        {
            get { return _keyRepeatSpeed; }
            set { _keyRepeatSpeed = value; }
        }

        public SkinnedTextBox()
        {
            ActiveArea = Widgets.ActiveArea.Center;

            _text = new FormattedText();
            _textColor = Colors.White;
            _keyRepeatSpeed = DEFAULT_KEY_REPEAT;
        }

        internal override void KeyDown(OpenTK.Input.KeyboardKeyEventArgs e, WidgetController.ModifierKeys k)
        {
            base.KeyDown(e, k);

            _keyDown = e.Key;
            _modifierKeys = k;
            _secondsFromLastKey = 0;

            ManageKey();
        }

        internal override void KeyUp(OpenTK.Input.KeyboardKeyEventArgs e, WidgetController.ModifierKeys k)
        {
            base.KeyUp(e, k);

            _keyDown = null;
            _modifierKeys = k;
        }

        private void ManageKey()
        {
            if (_keyDown.HasValue)
            {
                OpenTK.Input.Key key = _keyDown.Value;

                if (key >= OpenTK.Input.Key.A && key <= OpenTK.Input.Key.Z)
                {
                    string c = key.ToString();
                    if ((_modifierKeys & WidgetController.ModifierKeys.Shift) == 0)
                    {
                        c = c.ToLower();
                    }

                    _text.SourceText += c;
                }
                else if (key >= OpenTK.Input.Key.Number0 && key <= OpenTK.Input.Key.Number9)
                {
                    int digit = key - OpenTK.Input.Key.Number0;
                    _text.SourceText += digit.ToString();
                }
                else if (key >= OpenTK.Input.Key.Keypad0 && key <= OpenTK.Input.Key.Keypad9)
                {
                    int digit = key - OpenTK.Input.Key.Keypad0;
                    _text.SourceText += digit.ToString();
                }
                else
                {
                    switch (key)
                    {
                        case OpenTK.Input.Key.BackSpace:
                            if (_text.SourceText.Length > 0)
                            {
                                _text.SourceText = _text.SourceText.Substring(0, _text.SourceText.Length - 1);
                            }
                            break;

                        case OpenTK.Input.Key.Space:
                            _text.SourceText += " ";
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
                if (_keyRepeatSpeed <= 0)
                {
                    _keyRepeatSpeed = DEFAULT_KEY_REPEAT;
                }

                if (_caret == null)
                {
                    AddCaret();
                }

                UpdateCaret();
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
                _secondsFromLastKey += inSecondsPast;

                if (_secondsFromLastTick > CARET_TICK)
                {
                    _secondsFromLastTick = 0;
                    _caret.Active = !_caret.Active;
                }
                if (_secondsFromLastKey > _keyRepeatSpeed && _keyDown != null)
                {
                    _secondsFromLastKey = 0;
                    ManageKey();
                }
            }
            else
            {
                _caret.Active = false;
            }
        }

        private void AddCaret()
        {
            string textBackup = _text.SourceText;
            _text.SourceText = "Wq";

            _caret = new GameObject("caret", this.GameObj);

            Transform t = _caret.AddComponent<Transform>();
            t.RelativePos = new Vector3(Rect.W / 2, Rect.H / 2, DELTA_Z);
            t.RelativeAngle = 0;

            SpriteRenderer sr = new SpriteRenderer();
            sr.VisibilityGroup = this.VisibilityGroup;
            sr.Rect = Rect.AlignCenter(0, 0, 3, _text.TextMetrics.Size.Y);
            sr.SharedMaterial = Material.InvertWhite;

            _caret.AddComponent<SpriteRenderer>(sr);
            _caret.Active = false;

            Scene.Current.AddObject(_caret);

            _text.SourceText = textBackup;
        }

        private void UpdateCaret()
        {
            if (_caret != null && Skin.Res != null)
            {
                Vector3 caretPosition = _caret.Transform.RelativePos;
                caretPosition.X = Skin.Res.Border.X + _text.Size.X;

                _caret.Transform.RelativePos = caretPosition;
            }
        }
    }
}