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

namespace FrozenCore.Widgets
{
    [Serializable]
    public class SkinnedButton : SkinnedWidget<BaseSkin>
    {
        private ContentRef<Script> _onLeftClick;
        private ContentRef<Script> _onRightClick;
        private float _repeatLeftClickEvery;
        private FormattedText _text;
        private ColorRgba _textColor;

        [NonSerialized]
        private bool _leftButtonDown;
        [NonSerialized]
        private float _secondsFromLastTick;

        [NonSerialized]
        private object _leftClickArgument;
        [NonSerialized]
        private object _rightClickArgument;

        [EditorHintFlags(MemberFlags.Invisible)]
        public object LeftClickArgument
        {
            private get { return _leftClickArgument; }
            set { _leftClickArgument = value; }
        }

        [EditorHintFlags(MemberFlags.Invisible)]
        public object RightClickArgument
        {
            private get { return _rightClickArgument; }
            set { _rightClickArgument = value; }
        }

        [EditorHintDecimalPlaces(1)]
        public float RepeatLeftClickEvery
        {
            get { return _repeatLeftClickEvery; }
            set { _repeatLeftClickEvery = value; }
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

        public SkinnedButton()
        {
            _text = new FormattedText();
            _textColor = Colors.White;
        }

        internal override void MouseDown(OpenTK.Input.MouseButtonEventArgs e)
        {
            if (e.Button == OpenTK.Input.MouseButton.Right && OnRightClick.Res != null)
            {
                OnRightClick.Res.Execute(this.GameObj, RightClickArgument);
            }
            if (e.Button == OpenTK.Input.MouseButton.Left)
            {
                SetTextureTopLeft(Skin.Res.Origin.Active);

                if (OnLeftClick.Res != null && RepeatLeftClickEvery > 0)
                {
                    _leftButtonDown = true;
                }
            }
        }

        internal override void MouseUp(OpenTK.Input.MouseButtonEventArgs e)
        {
            if (e.Button == OpenTK.Input.MouseButton.Left)
            {
                _leftButtonDown = false;

                if (_isMouseOver)
                {
                    SetTextureTopLeft(Skin.Res.Origin.Hover);

                    if (OnLeftClick.Res != null && RepeatLeftClickEvery == 0)
                    {
                        OnLeftClick.Res.Execute(this.GameObj, LeftClickArgument);
                    }
                }
            }
        }

        internal override void MouseLeave()
        {
            base.MouseLeave();
        }

        internal override void MouseEnter()
        {
            _isMouseOver = true;
            if (_widgetEnabled)
            {
                if (!_leftButtonDown)
                {
                    SetTextureTopLeft(Skin.Res.Origin.Hover);
                }
                else
                {
                    SetTextureTopLeft(Skin.Res.Origin.Active);
                }
            }
        }

        protected override void DrawCanvas(IDrawDevice inDevice, Canvas inCanvas)
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

        internal override Polygon GetActiveAreaOnScreen(Duality.Components.Camera inCamera)
        {
            _activeAreaOnScreen[0] = inCamera.GetScreenCoord(_points[0].WorldCoords).Xy;
            _activeAreaOnScreen[1] = inCamera.GetScreenCoord(_points[3].WorldCoords).Xy;
            _activeAreaOnScreen[2] = inCamera.GetScreenCoord(_points[15].WorldCoords).Xy;
            _activeAreaOnScreen[3] = inCamera.GetScreenCoord(_points[12].WorldCoords).Xy;

            return _activeAreaOnScreen;
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
