using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Duality;
using FrozenCore.Components;
using Duality.Components.Renderers;
using OpenTK;
using Duality.Resources;

namespace FrozenCore.Widgets
{
    [Serializable]
    public class Button : Widget
    {
        public ContentRef<Script> OnLeftClick { get; set; }
        public ContentRef<Script> OnRightClick { get; set; }

        public FormattedText Text { get; set; }

        public Button()
        {
            Text = new FormattedText();
        }

        public override void MouseDown(OpenTK.Input.MouseButtonEventArgs e)
        {
            if (e.Button == OpenTK.Input.MouseButton.Right && OnRightClick.Res != null)
            {
                OnRightClick.Res.Execute(this.GameObj);
            }
        }

        public override void MouseUp(OpenTK.Input.MouseButtonEventArgs e)
        {
            if (e.Button == OpenTK.Input.MouseButton.Left && OnLeftClick.Res != null)
            {
                OnLeftClick.Res.Execute(this.GameObj);
            }
        }

        protected override void Draw(IDrawDevice device)
        {
            if (Text != null)
            {
                Canvas c = new Canvas(device);
                Vector2 textCenter = Text.Size / 2;

                Vector3 buttonCenter = (_points[5].WorldCoords + _points[10].WorldCoords) / 2;
                //Vector3 buttonCenter = (_points[0].WorldCoords + _points[15].WorldCoords) / 2;

                c.CurrentState.TransformHandle = textCenter;
                c.CurrentState.TransformAngle = GameObj.Transform.Angle;

                c.DrawText(Text, buttonCenter.X + textCenter.X, buttonCenter.Y + textCenter.Y, buttonCenter.Z - .1f, null, Alignment.Center);
            }
        }

        public override void MouseEnter()
        {
            base.MouseEnter();

            if (_widgetEnabled)
            {
                SetTextureTopLeft(Skin.Res.HoverOrigin);
            }
        }

        public override void MouseLeave()
        {
            base.MouseLeave();

            if (_widgetEnabled)
            {
                SetTextureTopLeft(Skin.Res.NormalOrigin);
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
    }
}
