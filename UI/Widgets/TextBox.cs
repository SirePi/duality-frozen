// This code is provided under the MIT license. Originally by Alessandro Pilati.

using Duality;
using Duality.Components;
using Duality.Components.Renderers;
using Duality.Drawing;
using Duality.Editor;
using Duality.Resources;
using OpenTK;
using SnowyPeak.Duality.Plugin.Frozen.Core;
using SnowyPeak.Duality.Plugin.Frozen.UI.Properties;
using SnowyPeak.Duality.Plugin.Frozen.UI.Resources;
using System;

namespace SnowyPeak.Duality.Plugin.Frozen.UI.Widgets
{
    /// <summary>
    /// A TextBox Widget
    /// </summary>
    [Serializable]
    [EditorHintImage(typeof(Res), ResNames.ImageTextBox)]
    [EditorHintCategory(typeof(Res), ResNames.CategoryWidgets)]
    public class TextBox : Widget
    {
        #region NonSerialized fields

        [NonSerialized]
        private static readonly float CARET_TICK = .5f;

        [NonSerialized]
        private static readonly float DEFAULT_KEY_REPEAT = .2f;

        [NonSerialized]
        private GameObject _caret;

        [NonSerialized]
        private FormattedText _fText;

        [NonSerialized]
        private OpenTK.Input.Key? _keyDown;

        [NonSerialized]
        private string _lastText;

        [NonSerialized]
        private WidgetController.ModifierKeys _modifierKeys;

        [NonSerialized]
        private float _secondsFromLastKey;

        [NonSerialized]
        private float _secondsFromLastTick;

        #endregion NonSerialized fields

        private float _keyRepeatSpeed;
        private string _text;
        private ColorRgba _textColor;
        private ContentRef<Font> _textFont;

        private ContentRef<WidgetAppearance> _widgetAppearance;

        /// <summary>
        /// Constructor
        /// </summary>
        public TextBox()
        {
            ActiveArea = ActiveArea.Center;

            _fText = new FormattedText();
            _textColor = Colors.White;
            _keyRepeatSpeed = DEFAULT_KEY_REPEAT;
        }

        public ContentRef<WidgetAppearance> Appearance
        {
            get { return _widgetAppearance; }
            set
            {
                _widgetAppearance = value;
                _dirtyFlags |= DirtyFlags.Appearance;
            }
        }

        /// <summary>
        /// [GET / SET] The speed of key repeat, if key is presset
        /// </summary>
        [EditorHintRange(0.1f, 1)]
        public float KeyRepeatSpeed
        {
            get { return _keyRepeatSpeed; }
            set { _keyRepeatSpeed = value; }
        }

        /// <summary>
        /// [GET / SET] the Text of the TextBox
        /// </summary>
        public String Text
        {
            get { return _text; }
            set { _text = value; }
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
        /// <param name="e"></param>
        /// <param name="k"></param>
        public override void KeyDown(OpenTK.Input.KeyboardKeyEventArgs e, WidgetController.ModifierKeys k)
        {
            _keyDown = e.Key;
            _modifierKeys = k;
            _secondsFromLastKey = 0;

            ManageKey();
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="e"></param>
        /// <param name="k"></param>
        public override void KeyUp(OpenTK.Input.KeyboardKeyEventArgs e, WidgetController.ModifierKeys k)
        {
            _keyDown = null;
            _modifierKeys = k;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="inCanvas"></param>
        protected override void DrawCanvas(Canvas inCanvas)
        {
            if (!String.IsNullOrWhiteSpace(_text))
            {
                Vector3 textLeft = (_points[5].WorldCoords + _points[9].WorldCoords) / 2;
                if (_textFont.Res != null && _fText.Fonts[0] != _textFont)
                {
                    _fText.Fonts[0] = _textFont;
                }

                _fText.SourceText = _text;

                inCanvas.PushState();
                inCanvas.State.ColorTint = _textColor;
                inCanvas.State.TransformAngle = GameObj.Transform.Angle;
                inCanvas.DrawText(_fText, textLeft.X, textLeft.Y, textLeft.Z + DELTA_Z, null, Alignment.Left);

                inCanvas.PopState();
            }
        }

        protected override Appearance GetBaseAppearance()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="inSecondsPast"></param>
        protected override void OnUpdate(float inSecondsPast)
        {
            if (_caret == null)
            {
                AddCaret();
            }

            if (IsWidgetActive)
            {
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

                UpdateCaret();
            }
            else
            {
                _caret.Active = false;
            }
        }

        private void AddCaret()
        {
            _fText.SourceText = "Wq";

            _caret = new GameObject("caret", this.GameObj);

            Transform t = _caret.AddComponent<Transform>();
            t.RelativePos = new Vector3(Rect.W / 2, Rect.H / 2, DELTA_Z);
            t.RelativeAngle = 0;

            SpriteRenderer sr = new SpriteRenderer();
            sr.VisibilityGroup = this.VisibilityGroup;
            sr.Rect = Rect.AlignCenter(0, 0, 2, _fText.TextMetrics.Size.Y);
            sr.SharedMaterial = Material.InvertWhite;

            _caret.AddComponent<SpriteRenderer>(sr);
            _caret.Active = false;

            Scene.Current.AddObject(_caret);

            _fText.SourceText = _text;
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

                    _text += c;
                }
                else if (key >= OpenTK.Input.Key.Number0 && key <= OpenTK.Input.Key.Number9)
                {
                    int digit = key - OpenTK.Input.Key.Number0;
                    _text += digit.ToString();
                }
                else if (key >= OpenTK.Input.Key.Keypad0 && key <= OpenTK.Input.Key.Keypad9)
                {
                    int digit = key - OpenTK.Input.Key.Keypad0;
                    _text += digit.ToString();
                }
                else
                {
                    switch (key)
                    {
                        case OpenTK.Input.Key.BackSpace:
                            if (_text.Length > 0)
                            {
                                _text = _text.Substring(0, _text.Length - 1);
                            }
                            break;

                        case OpenTK.Input.Key.Space:
                            _text += " ";
                            break;

                        case OpenTK.Input.Key.Comma:
                            _text += ",";
                            break;

                        case OpenTK.Input.Key.Period:
                        case OpenTK.Input.Key.KeypadPeriod:
                            _text += ".";
                            break;
                    }
                }
            }
        }

        private void UpdateCaret()
        {
            if (_caret != null)
            {
                Vector3 textLeft = (_points[5].WorldCoords + _points[9].WorldCoords) / 2;

                Vector3 caretPosition = _caret.Transform.RelativePos;
                caretPosition.X = textLeft.X + _fText.Size.X;

                _caret.Transform.RelativePos = caretPosition;
            }
        }
    }
}