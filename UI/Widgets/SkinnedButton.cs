﻿// This code is provided under the MIT license. Originally by Alessandro Pilati.

using System;
using Duality;
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
    /// A Button Widget
    /// </summary>
    [Serializable]
    [EditorHintImage(typeof(Res), ResNames.ImageButton)]
    [EditorHintCategory(typeof(Res), ResNames.CategoryWidgets)]
    public class SkinnedButton : SkinnedWidget
    {
        #region NonSerialized fields

        /// <summary>
        ///
        /// </summary>
        [NonSerialized]
        protected bool _leftButtonDown;

        [NonSerialized]
        private FormattedText _fText;

        [NonSerialized]
        private float _secondsFromLastTick;

        #endregion NonSerialized fields

        private object _leftClickArgument;
        private ContentRef<Script> _onLeftClick;
        private ContentRef<Script> _onRightClick;
        private float _repeatLeftClickEvery;
        private object _rightClickArgument;
        private string _text;
        private ColorRgba _textColor;
        private ContentRef<Font> _textFont;

        /// <summary>
        /// Constructor
        /// </summary>
        public SkinnedButton()
        {
            ActiveArea = Widgets.ActiveArea.All;

            _fText = new FormattedText();
            _textColor = Colors.White;
        }

        /// <summary>
        ///
        /// </summary>
        public object LeftClickArgument
        {
            get { return _leftClickArgument; }
            set { _leftClickArgument = value; }
        }

        /// <summary>
        ///
        /// </summary>
        public ContentRef<Script> OnLeftClick
        {
            get { return _onLeftClick; }
            set { _onLeftClick = value; }
        }

        /// <summary>
        ///
        /// </summary>
        public ContentRef<Script> OnRightClick
        {
            get { return _onRightClick; }
            set { _onRightClick = value; }
        }

        /// <summary>
        /// [GET / SET] If set to a value different than 0, the OnLeftClick event will be fired every
        /// RepeatLeftClickEvery seconds
        /// </summary>
        [EditorHintDecimalPlaces(1)]
        public float RepeatLeftClickEvery
        {
            get { return _repeatLeftClickEvery; }
            set { _repeatLeftClickEvery = value; }
        }

        /// <summary>
        ///
        /// </summary>
        public object RightClickArgument
        {
            get { return _rightClickArgument; }
            set { _rightClickArgument = value; }
        }

        /// <summary>
        /// [GET / SET] the Text of the Button
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
        public override void MouseDown(OpenTK.Input.MouseButtonEventArgs e)
        {
            if (Status != WidgetStatus.Disabled)
            {
                if (e.Button == OpenTK.Input.MouseButton.Right && OnRightClick.Res != null)
                {
                    OnRightClick.Res.Execute(this.GameObj, RightClickArgument);
                }
                if (e.Button == OpenTK.Input.MouseButton.Left)
                {
                    Status = WidgetStatus.Active;

                    if (OnLeftClick.Res != null && RepeatLeftClickEvery > 0)
                    {
                        _leftButtonDown = true;
                    }
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
                Status = _leftButtonDown ? WidgetStatus.Active : WidgetStatus.Hover;
            }
        }

        /// <summary>
        ///
        /// </summary>
        public override void MouseLeave()
        {
            _isMouseOver = false;

            if (Status != WidgetStatus.Disabled && !_leftButtonDown)
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
            if (Status != WidgetStatus.Disabled)
            {
                if (e.Button == OpenTK.Input.MouseButton.Left)
                {
                    _leftButtonDown = false;

                    if (_isMouseOver)
                    {
                        Status = WidgetStatus.Hover;

                        if (OnLeftClick.Res != null && RepeatLeftClickEvery == 0)
                        {
                            OnLeftClick.Res.Execute(this.GameObj, LeftClickArgument);
                        }
                    }
                    else
                    {
                        Status = WidgetStatus.Normal;
                    }
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

        /// <summary>
        ///
        /// </summary>
        /// <param name="inSecondsPast"></param>
        protected override void OnUpdate(float inSecondsPast)
        {
            _secondsFromLastTick += inSecondsPast;
            if (_secondsFromLastTick > RepeatLeftClickEvery && _leftButtonDown && OnLeftClick.Res != null)
            {
                _secondsFromLastTick = 0;
                OnLeftClick.Res.Execute(this.GameObj, _leftClickArgument);
            }

            base.OnUpdate(inSecondsPast);
        }
    }
}