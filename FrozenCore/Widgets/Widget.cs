// This code is provided under the MIT license. Originally by Alessandro Pilati.

using System;
using Duality;
using Duality.Components;
using Duality.Drawing;
using Duality.Editor;
using Duality.Resources;

namespace FrozenCore.Widgets
{
    [Serializable]
    [RequiredComponent(typeof(Transform))]
    public abstract class Widget : Component, ICmpRenderer, ICmpUpdatable, ICmpInitializable
    {
        public enum WidgetStatus
        {
            Normal,
            Hover,
            Active,
            Disabled
        }

        #region NonSerialized fields

        [NonSerialized]
        protected static readonly float DELTA_Z = -.001f;

        [NonSerialized]
        protected Polygon _activeAreaOnScreen;

        [NonSerialized]
        protected Polygon _areaOnScreen;

        [NonSerialized]
        protected MultiSpacePoint[] _points;

        [NonSerialized]
        protected bool _resized;

        [NonSerialized]
        protected VertexC1P3T2[] _vertices;

        private WidgetStatus _status;

        [NonSerialized]
        private bool _widgetActive;

        #endregion NonSerialized fields

        private ActiveArea _activeArea;

        private Rect _rect;

        private Rect _visibleRect;

        private VisibilityFlag _visiblityFlag;

        /// <summary>
        /// [GET / SET] The ActiveArea of the Widget that can react to mouse input such as
        /// Hover, Click, etc..
        /// </summary>
        public ActiveArea ActiveArea
        {
            get { return _activeArea; }
            set { _activeArea = value; }
        }
        [EditorHintFlags(MemberFlags.Invisible)]
        float ICmpRenderer.BoundRadius
        {
            get { return Rect.BoundingRadius; }
        }

        [EditorHintFlags(MemberFlags.Invisible)]
        public bool IsWidgetActive
        {
            get { return _widgetActive; }
        }

        [EditorHintDecimalPlaces(1)]
        public Rect Rect
        {
            get { return _rect; }
            set
            {
                _rect = value;
                _resized = true;
            }
        }
        //[EditorHintFlags(MemberFlags.Invisible)]
        public WidgetStatus Status
        {
            get { return _status; }
            set
            {
                _status = value;
                OnStatusChange();
            }
        }
        public VisibilityFlag VisibilityGroup
        {
            get { return _visiblityFlag; }
            set { _visiblityFlag = value; }
        }
        [EditorHintDecimalPlaces(1)]
        public Rect VisibleRect
        {
            get { return _visibleRect; }
            set { _visibleRect = value; }
        }

        public Widget()
        {
            VisibilityGroup = VisibilityFlag.Group0;

            _areaOnScreen = new Polygon(4);
            _activeAreaOnScreen = new Polygon(4);

            _rect = new Duality.Rect(0, 0, 50, 50);
            _visibleRect = Rect.Empty;

            _vertices = new VertexC1P3T2[36];
            _points = new MultiSpacePoint[16];

            for (int i = 0; i < _vertices.Length; i++)
            {
                _vertices[i] = new VertexC1P3T2();
            }
        }

        public void Close()
        {
            Scene.Current.RemoveObject(this.GameObj);
        }

        void ICmpInitializable.OnInit(Component.InitContext context)
        {
            OnInit(context);

            if (Status != WidgetStatus.Disabled)
            {
                Status = WidgetStatus.Normal;
            }
        }

        void ICmpInitializable.OnShutdown(Component.ShutdownContext context)
        {
        }

        void ICmpRenderer.Draw(IDrawDevice device)
        {
            Draw(device);
            DrawCanvas(device, new Canvas(device));
        }

        bool ICmpRenderer.IsVisible(IDrawDevice device)
        {
            bool result = true;

            // Differing ScreenOverlay flag? Don't render!
            if ((device.VisibilityMask & VisibilityFlag.ScreenOverlay) != (VisibilityGroup & VisibilityFlag.ScreenOverlay))
            {
                result = false;
            }
            // No match in any VisibilityGroup? Don't render!
            if ((VisibilityGroup & device.VisibilityMask & VisibilityFlag.AllGroups) == VisibilityFlag.None)
            {
                result = false;
            }

            return result;
        }

        void ICmpUpdatable.OnUpdate()
        {
            OnUpdate(Time.LastDelta / 1000f);
        }

        internal virtual void Activate()
        {
            _widgetActive = true;
        }

        internal virtual void Deactivate()
        {
            _widgetActive = false;
        }

        internal Polygon GetActiveAreaOnScreen(Camera inCamera)
        {
            switch (ActiveArea)
            {
                case Widgets.ActiveArea.None:
                    return Polygon.NO_POLYGON;

                case Widgets.ActiveArea.Custom:
                    return GetCustomAreaOnScreen(inCamera);

                default:
                    return GetDefaultActiveAreaOnScreen(inCamera);
            }
        }

        internal Polygon GetAreaOnScreen(Camera inCamera)
        {
            _areaOnScreen[0] = inCamera.GetScreenCoord(_points[0].WorldCoords).Xy;
            _areaOnScreen[1] = inCamera.GetScreenCoord(_points[3].WorldCoords).Xy;
            _areaOnScreen[2] = inCamera.GetScreenCoord(_points[15].WorldCoords).Xy;
            _areaOnScreen[3] = inCamera.GetScreenCoord(_points[12].WorldCoords).Xy;

            return _areaOnScreen;
        }

        internal virtual Polygon GetCustomAreaOnScreen(Camera inCamera)
        {
            return Polygon.NO_POLYGON;
        }

        internal abstract void KeyDown(OpenTK.Input.KeyboardKeyEventArgs e, WidgetController.ModifierKeys k);

        internal abstract void KeyUp(OpenTK.Input.KeyboardKeyEventArgs e, WidgetController.ModifierKeys k);

        internal abstract void MouseDown(OpenTK.Input.MouseButtonEventArgs e);

        internal abstract void MouseEnter();

        internal abstract void MouseLeave();

        internal abstract void MouseMove(OpenTK.Input.MouseMoveEventArgs e);

        internal abstract void MouseUp(OpenTK.Input.MouseButtonEventArgs e);

        internal abstract void MouseWheel(OpenTK.Input.MouseWheelEventArgs e);

        protected abstract void Draw(IDrawDevice inDevice);

        protected abstract void DrawCanvas(IDrawDevice inDevice, Canvas inCanvas);

        protected abstract void OnInit(Component.InitContext inContext);

        protected abstract void OnStatusChange();

        protected abstract void OnUpdate(float inSecondsPast);

        private Polygon GetDefaultActiveAreaOnScreen(Camera inCamera)
        {
            switch (ActiveArea)
            {
                case Widgets.ActiveArea.LeftBorder:
                    _activeAreaOnScreen[0] = inCamera.GetScreenCoord(_points[0].WorldCoords).Xy;
                    _activeAreaOnScreen[1] = inCamera.GetScreenCoord(_points[1].WorldCoords).Xy;
                    _activeAreaOnScreen[2] = inCamera.GetScreenCoord(_points[13].WorldCoords).Xy;
                    _activeAreaOnScreen[3] = inCamera.GetScreenCoord(_points[12].WorldCoords).Xy;
                    break;

                case Widgets.ActiveArea.TopBorder:
                    _activeAreaOnScreen[0] = inCamera.GetScreenCoord(_points[0].WorldCoords).Xy;
                    _activeAreaOnScreen[1] = inCamera.GetScreenCoord(_points[3].WorldCoords).Xy;
                    _activeAreaOnScreen[2] = inCamera.GetScreenCoord(_points[7].WorldCoords).Xy;
                    _activeAreaOnScreen[3] = inCamera.GetScreenCoord(_points[4].WorldCoords).Xy;
                    break;

                case Widgets.ActiveArea.RightBorder:
                    _activeAreaOnScreen[0] = inCamera.GetScreenCoord(_points[2].WorldCoords).Xy;
                    _activeAreaOnScreen[1] = inCamera.GetScreenCoord(_points[3].WorldCoords).Xy;
                    _activeAreaOnScreen[2] = inCamera.GetScreenCoord(_points[15].WorldCoords).Xy;
                    _activeAreaOnScreen[3] = inCamera.GetScreenCoord(_points[14].WorldCoords).Xy;
                    break;

                case Widgets.ActiveArea.BottomBorder:
                    _activeAreaOnScreen[0] = inCamera.GetScreenCoord(_points[8].WorldCoords).Xy;
                    _activeAreaOnScreen[1] = inCamera.GetScreenCoord(_points[11].WorldCoords).Xy;
                    _activeAreaOnScreen[2] = inCamera.GetScreenCoord(_points[15].WorldCoords).Xy;
                    _activeAreaOnScreen[3] = inCamera.GetScreenCoord(_points[12].WorldCoords).Xy;
                    break;

                case Widgets.ActiveArea.Center:
                    _activeAreaOnScreen[0] = inCamera.GetScreenCoord(_points[5].WorldCoords).Xy;
                    _activeAreaOnScreen[1] = inCamera.GetScreenCoord(_points[6].WorldCoords).Xy;
                    _activeAreaOnScreen[2] = inCamera.GetScreenCoord(_points[10].WorldCoords).Xy;
                    _activeAreaOnScreen[3] = inCamera.GetScreenCoord(_points[9].WorldCoords).Xy;
                    break;

                default: //All or others.. but it should never happen
                    _activeAreaOnScreen[0] = inCamera.GetScreenCoord(_points[0].WorldCoords).Xy;
                    _activeAreaOnScreen[1] = inCamera.GetScreenCoord(_points[3].WorldCoords).Xy;
                    _activeAreaOnScreen[2] = inCamera.GetScreenCoord(_points[15].WorldCoords).Xy;
                    _activeAreaOnScreen[3] = inCamera.GetScreenCoord(_points[12].WorldCoords).Xy;
                    break;
            }

            return _activeAreaOnScreen;
        }
    }
}