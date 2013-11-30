using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Duality;
using Duality.Components;
using OpenTK;

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

        void ICmpInitializable.OnInit(Component.InitContext context)
        {
            if (context == InitContext.Activate)
                DualityApp.Mouse.Move += new EventHandler<OpenTK.Input.MouseMoveEventArgs>(Mouse_Move);
        }

        void Mouse_Move(object sender, OpenTK.Input.MouseMoveEventArgs e)
        {
            _mousePosition.X = e.X;
            _mousePosition.Y = e.Y;

            _worldPosition = GameObj.Camera.GetSpaceCoord(_mousePosition);
        }

        void ICmpInitializable.OnShutdown(Component.ShutdownContext context)
        {
            if (context == ShutdownContext.Deactivate)
                DualityApp.Mouse.Move -= new EventHandler<OpenTK.Input.MouseMoveEventArgs>(Mouse_Move);
        }

        float ICmpRenderer.BoundRadius
        {
            get { return 0; }
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
    }
}
