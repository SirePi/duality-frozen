// This code is provided under the MIT license. Originally by Alessandro Pilati.

using System;
using Duality;
using Duality.Components;
using OpenTK;
using Duality.Drawing;

namespace FrozenCore.Components
{
    [Serializable]
    [RequiredComponent(typeof(Camera))]
    public class MouseDebugger : Component, ICmpInitializable, ICmpRenderer
    {
        [NonSerialized]
        private Vector2 _mousePosition;

        [NonSerialized]
        private Vector3 _worldPosition;

        float ICmpRenderer.BoundRadius
        {
            get { return 0; }
        }

        void ICmpInitializable.OnInit(Component.InitContext context)
        {
            if (context == InitContext.Activate)
                DualityApp.Mouse.Move += new EventHandler<OpenTK.Input.MouseMoveEventArgs>(Mouse_Move);
        }

        void ICmpInitializable.OnShutdown(Component.ShutdownContext context)
        {
            if (context == ShutdownContext.Deactivate)
                DualityApp.Mouse.Move -= new EventHandler<OpenTK.Input.MouseMoveEventArgs>(Mouse_Move);
        }

        void ICmpRenderer.Draw(IDrawDevice device)
        {
            Canvas c = new Canvas(device);
            c.DrawText(String.Format("Mouse: {0}", _mousePosition), 0, 0, 0, Alignment.TopLeft);
            c.DrawText(String.Format("World: {0}", _worldPosition), 0, 20, 0, Alignment.TopLeft);
        }

        bool ICmpRenderer.IsVisible(IDrawDevice device)
        {
            return ((device.VisibilityMask & VisibilityFlag.ScreenOverlay) != VisibilityFlag.None);
        }

        private void Mouse_Move(object sender, OpenTK.Input.MouseMoveEventArgs e)
        {
            _mousePosition.X = e.X;
            _mousePosition.Y = e.Y;

            _worldPosition = GameObj.Camera.GetSpaceCoord(_mousePosition);
        }
    }
}