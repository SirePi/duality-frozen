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

namespace FrozenCore.Widgets
{
    [Serializable]
    public class Button : Widget
    {
        private ContentRef<BaseSkin> _skin;
        private ContentRef<Script> _onLeftClick;
        private ContentRef<Script> _onRightClick;
        private FormattedText _text;

        [NonSerialized]
        private object _leftClickArgument;
        [NonSerialized]
        private object _rightClickArgument;

        [EditorHintFlags(MemberFlags.Invisible)]
        public object LeftClickArgument
        {
            private get { return _leftClickArgument; }
            set {_leftClickArgument = value;}
        }

        [EditorHintFlags(MemberFlags.Invisible)]
        public object RightClickArgument
        {
            private get { return _rightClickArgument; }
            set {_rightClickArgument = value;}
        }

        public ContentRef<BaseSkin> Skin
        {
            get { return _skin; }
            set 
            { 
                _skin = value;
                BaseSkinRes = value.Res;
            }
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

        public FormattedText Text
        {
            get { return _text; }
            set { _text = value; }
        }

        public Button()
        {
            _text = new FormattedText();
        }

        public override void MouseDown(OpenTK.Input.MouseButtonEventArgs e)
        {
            if (e.Button == OpenTK.Input.MouseButton.Right && OnRightClick.Res != null)
            {
                OnRightClick.Res.Execute(this.GameObj, RightClickArgument);
            }
        }

        public override void MouseUp(OpenTK.Input.MouseButtonEventArgs e)
        {
            if (e.Button == OpenTK.Input.MouseButton.Left && OnLeftClick.Res != null)
            {
                OnLeftClick.Res.Execute(this.GameObj, LeftClickArgument);
            }
        }

        protected override void Draw(IDrawDevice inDevice, Canvas inCanvas)
        {
            if (Text != null)
            {
                Vector2 textCenter = Text.Size / 2;

                Vector3 buttonCenter = (_points[5].WorldCoords + _points[10].WorldCoords) / 2;
                //Vector3 buttonCenter = (_points[0].WorldCoords + _points[15].WorldCoords) / 2;

                inCanvas.CurrentState.TransformHandle = textCenter;
                inCanvas.CurrentState.TransformAngle = GameObj.Transform.Angle;

                inCanvas.DrawText(Text, buttonCenter.X + textCenter.X, buttonCenter.Y + textCenter.Y, buttonCenter.Z - DELTA_Z, null, Alignment.Center);
            }
        }

        public override void MouseEnter()
        {
            base.MouseEnter();

            if (_widgetEnabled)
            {
                SetTextureTopLeft(Skin.Res.Origin.Hover);
            }
        }

        public override void MouseLeave()
        {
            base.MouseLeave();

            if (_widgetEnabled)
            {
                SetTextureTopLeft(Skin.Res.Origin.Normal);
            }
        }

        public override Polygon GetActiveAreaOnScreen(Duality.Components.Camera inCamera)
        {
            _activeAreaOnScreen[0] = inCamera.GetScreenCoord(_points[0].WorldCoords).Xy;
            _activeAreaOnScreen[1] = inCamera.GetScreenCoord(_points[3].WorldCoords).Xy;
            _activeAreaOnScreen[2] = inCamera.GetScreenCoord(_points[15].WorldCoords).Xy;
            _activeAreaOnScreen[3] = inCamera.GetScreenCoord(_points[12].WorldCoords).Xy;

            return _activeAreaOnScreen;
        }

        protected override void Initialize(Component.InitContext context)
        {
            BaseSkinRes = Skin.Res;
        }
    }
}
