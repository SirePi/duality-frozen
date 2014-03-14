﻿// This code is provided under the MIT license. Originally by Alessandro Pilati.

using System;
using Duality;
using Duality.Drawing;
using Duality.Editor;
using OpenTK;

namespace FrozenCore.Widgets
{
    [Serializable]
    public class SkinnedButton : SkinnedWidget
    {
        #region NonSerialized fields

        [NonSerialized]
        protected bool _leftButtonDown;

        [NonSerialized]
        private float _secondsFromLastTick;

        #endregion NonSerialized fields

        private ContentRef<Script> _onLeftClick;
        private ContentRef<Script> _onRightClick;
        private float _repeatLeftClickEvery;
        private FormattedText _text;
        private ColorRgba _textColor;
        private object _leftClickArgument;

        private object _rightClickArgument;

        public object LeftClickArgument
        {
            get { return _leftClickArgument; }
            set { _leftClickArgument = value; }
        }

        public ContentRef<Script> OnLeftClick
        {
            get { return _onLeftClick; }
            set { _onLeftClick = value; }
        }

        public ContentRef<Script> OnRightClick
        {
            get { return _onRightClick; }
            set { _onRightClick = value; }
        }

        [EditorHintDecimalPlaces(1)]
        public float RepeatLeftClickEvery
        {
            get { return _repeatLeftClickEvery; }
            set { _repeatLeftClickEvery = value; }
        }

        public object RightClickArgument
        {
            get { return _rightClickArgument; }
            set { _rightClickArgument = value; }
        }

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

        public SkinnedButton()
        {
            ActiveArea = Widgets.ActiveArea.All;

            _text = new FormattedText();
            _textColor = Colors.White;
        }

        internal override void MouseDown(OpenTK.Input.MouseButtonEventArgs e)
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

        internal override void MouseEnter()
        {
            _isMouseOver = true;
            if (Status != WidgetStatus.Disabled)
            {
                Status = _leftButtonDown ? WidgetStatus.Active : WidgetStatus.Hover;
            }
        }

        internal override void MouseLeave()
        {
            _isMouseOver = false;

            if (Status != WidgetStatus.Disabled && !_leftButtonDown)
            {
                Status = WidgetStatus.Normal;
            }
        }

        internal override void MouseUp(OpenTK.Input.MouseButtonEventArgs e)
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

        protected override void DrawCanvas(Canvas inCanvas)
        {
            if (Text != null)
            {
                Vector3 buttonCenter = (_points[5].WorldCoords + _points[10].WorldCoords) / 2;

                inCanvas.PushState();
                inCanvas.State.ColorTint = _textColor;
                inCanvas.State.TransformAngle = GameObj.Transform.Angle;
                inCanvas.DrawText(Text, buttonCenter.X, buttonCenter.Y, buttonCenter.Z + DELTA_Z, null, Alignment.Center);
                inCanvas.PopState();
            }
        }

        protected override void OnUpdate(float inSecondsPast)
        {
            _secondsFromLastTick += inSecondsPast;
            if (_secondsFromLastTick > RepeatLeftClickEvery && _leftButtonDown && OnLeftClick.Res != null)
            {
                _secondsFromLastTick = 0;
                OnLeftClick.Res.Execute(this.GameObj, _leftClickArgument);
            }
        }
    }
}