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
    public class Window : Widget
    {
        public FormattedText Title { get; set; }
        public bool IsDraggable { get; set; }

        [NonSerialized]
        private bool _isDragged;

        public Window()
        {
            Title = new FormattedText();
            _isDragged = false;
        }

        public override void MouseDown(OpenTK.Input.MouseButtonEventArgs e)
        {
            base.MouseDown(e);

            if (IsDraggable)
            {
                _isDragged = true;
            }
        }

        public override void MouseUp(OpenTK.Input.MouseButtonEventArgs e)
        {
            base.MouseUp(e);

            if (IsDraggable)
            {
                _isDragged = false;
            }
        }

        protected override void Draw(IDrawDevice device)
        {
            Canvas c = new Canvas(device);
            Vector2 textCenter = Title.Size / 2;
            Vector3 titleLeft = (_points[1].WorldCoords + _points[5].WorldCoords) / 2;
            
            c.CurrentState.TransformHandle = textCenter;
            c.CurrentState.TransformAngle = GameObj.Transform.Angle;

            c.DrawText(Title, titleLeft.X + textCenter.X, titleLeft.Y + textCenter.Y, titleLeft.Z, null, Alignment.Left);
        }

        public override void MouseEnter()
        {
            base.MouseEnter();

            SetTextureTopLeft(Skin.Res.HoverTopLeft);
        }

        public override void MouseLeave()
        {
            base.MouseLeave();

            SetTextureTopLeft(Skin.Res.NormalTopLeft);
        }

        public override Polygon GetActiveAreaOnScreen(Duality.Components.Camera inCamera)
        {
            _activeAreaOnScreen[0] = inCamera.GetScreenCoord(_points[0].WorldCoords).Xy;
            _activeAreaOnScreen[1] = inCamera.GetScreenCoord(_points[3].WorldCoords).Xy;
            _activeAreaOnScreen[2] = inCamera.GetScreenCoord(_points[8].WorldCoords).Xy;
            _activeAreaOnScreen[3] = inCamera.GetScreenCoord(_points[4].WorldCoords).Xy;

            return _activeAreaOnScreen;
        }
    }
}
