using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Duality;
using FrozenCore.Components;
using Duality.Components.Renderers;
using OpenTK;
using Duality.Resources;
using Duality.Components;

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

            AddCloseButton();
        }

        private void AddCloseButton()
        {
            GameObject closeButton = new GameObject("closeButton", this.GameObj);

            Transform t = closeButton.AddComponent<Transform>();

            Button b = closeButton.AddComponent<Button>();
            b.VisibilityGroup = this.VisibilityGroup;

            Scene.Current.AddObject(closeButton);
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
            if (Title != null)
            {
                Canvas c = new Canvas(device);
                Vector2 textOrigin = new Vector2(0, Title.Size.Y / 2);
                Vector3 titleLeft = (_points[1].WorldCoords + _points[5].WorldCoords) / 2;

                c.CurrentState.TransformHandle = textOrigin;
                c.CurrentState.TransformAngle = GameObj.Transform.Angle;

                c.DrawText(Title, titleLeft.X + textOrigin.X, titleLeft.Y + textOrigin.Y, titleLeft.Z - .1f, null, Alignment.Left);
            }
        }

        public override void MouseEnter()
        {
            base.MouseEnter();

            if (_widgetEnabled && Skin.Res != null)
            {
                SetTextureTopLeft(Skin.Res.HoverOrigin);
            }
        }

        public override void MouseLeave()
        {
            base.MouseLeave();

            if (_widgetEnabled && Skin.Res != null)
            {
                SetTextureTopLeft(Skin.Res.NormalOrigin);
            }
        }

        public override void MouseMove(OpenTK.Input.MouseMoveEventArgs e)
        {
            base.MouseMove(e);

            if (_isDragged)
            {
                this.GameObj.Transform.Pos += (new Vector3(e.XDelta, e.YDelta, 0));
            }
        }

        public override Polygon GetActiveAreaOnScreen(Duality.Components.Camera inCamera)
        {
            _activeAreaOnScreen[0] = inCamera.GetScreenCoord(_points[0].WorldCoords).Xy;
            _activeAreaOnScreen[1] = inCamera.GetScreenCoord(_points[3].WorldCoords).Xy;
            _activeAreaOnScreen[2] = inCamera.GetScreenCoord(_points[7].WorldCoords).Xy;
            _activeAreaOnScreen[3] = inCamera.GetScreenCoord(_points[4].WorldCoords).Xy;

            return _activeAreaOnScreen;
        }
    }
}
