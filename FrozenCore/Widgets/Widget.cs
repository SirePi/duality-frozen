// This code is provided under the MIT license. Originally by Alessandro Pilati.

using System;
using Duality;
using Duality.Components;
using Duality.Drawing;
using Duality.Editor;
using Duality.Resources;
using OpenTK;

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
        protected Vector3[] _tempActiveAreaOnScreen;

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

        [NonSerialized]
        protected bool _isInOverlay;

        [NonSerialized]
        private bool _widgetActive;

        #endregion NonSerialized fields

        private WidgetStatus _status;

        private ActiveArea _activeArea;

        private bool _overrideAutoZ;

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

        public bool OverrideAutomaticZ
        {
            get { return _overrideAutoZ; }
            set { _overrideAutoZ = value; }
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
            _tempActiveAreaOnScreen = new Vector3[4];

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
            if (context == InitContext.AddToGameObject)
            {
                this.GameObj.EventParentChanged += new EventHandler<GameObjectParentChangedEventArgs>(GameObj_EventParentChanged);
                FixRelativeZ();
            }

            if (Status != WidgetStatus.Disabled)
            {
                Status = WidgetStatus.Normal;
            }

            OnInit(context);
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
            _isInOverlay = (this.VisibilityGroup & VisibilityFlag.ScreenOverlay) != VisibilityFlag.None;

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
            if (_isInOverlay)
            {
                _areaOnScreen[0] = _points[0].WorldCoords.Xy;
                _areaOnScreen[1] = _points[3].WorldCoords.Xy;
                _areaOnScreen[2] = _points[15].WorldCoords.Xy;
                _areaOnScreen[3] = _points[12].WorldCoords.Xy;
            }
            else
            {
                _areaOnScreen[0] = inCamera.GetScreenCoord(_points[0].WorldCoords).Xy;
                _areaOnScreen[1] = inCamera.GetScreenCoord(_points[3].WorldCoords).Xy;
                _areaOnScreen[2] = inCamera.GetScreenCoord(_points[15].WorldCoords).Xy;
                _areaOnScreen[3] = inCamera.GetScreenCoord(_points[12].WorldCoords).Xy;
            }

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

        private void FixRelativeZ()
        {
            if (this.GameObj.Parent != null && this.GameObj.Parent.Transform != null)
            {
                Vector3 pos = this.GameObj.Transform.Pos;
                pos.Z = this.GameObj.Parent.Transform.Pos.Z + DELTA_Z;

                this.GameObj.Transform.Pos = pos;
            }
        }

        private void GameObj_EventParentChanged(object sender, GameObjectParentChangedEventArgs e)
        {
            if (!_overrideAutoZ)
            {
                FixRelativeZ();
            }
        }

        private Polygon GetDefaultActiveAreaOnScreen(Camera inCamera)
        {
            switch (ActiveArea)
            {
                case Widgets.ActiveArea.LeftBorder:
                    _tempActiveAreaOnScreen[0] = _points[0].WorldCoords;
                    _tempActiveAreaOnScreen[1] = _points[1].WorldCoords;
                    _tempActiveAreaOnScreen[2] = _points[13].WorldCoords;
                    _tempActiveAreaOnScreen[3] = _points[12].WorldCoords;
                    break;

                case Widgets.ActiveArea.TopBorder:
                    _tempActiveAreaOnScreen[0] = _points[0].WorldCoords;
                    _tempActiveAreaOnScreen[1] = _points[3].WorldCoords;
                    _tempActiveAreaOnScreen[2] = _points[7].WorldCoords;
                    _tempActiveAreaOnScreen[3] = _points[4].WorldCoords;
                    break;

                case Widgets.ActiveArea.RightBorder:
                    _tempActiveAreaOnScreen[0] = _points[2].WorldCoords;
                    _tempActiveAreaOnScreen[1] = _points[3].WorldCoords;
                    _tempActiveAreaOnScreen[2] = _points[15].WorldCoords;
                    _tempActiveAreaOnScreen[3] = _points[14].WorldCoords;
                    break;

                case Widgets.ActiveArea.BottomBorder:
                    _tempActiveAreaOnScreen[0] = _points[8].WorldCoords;
                    _tempActiveAreaOnScreen[1] = _points[11].WorldCoords;
                    _tempActiveAreaOnScreen[2] = _points[15].WorldCoords;
                    _tempActiveAreaOnScreen[3] = _points[12].WorldCoords;
                    break;

                case Widgets.ActiveArea.Center:
                    _tempActiveAreaOnScreen[0] = _points[5].WorldCoords;
                    _tempActiveAreaOnScreen[1] = _points[6].WorldCoords;
                    _tempActiveAreaOnScreen[2] = _points[10].WorldCoords;
                    _tempActiveAreaOnScreen[3] = _points[9].WorldCoords;
                    break;

                default: //All or others.. but it should never happen
                    _tempActiveAreaOnScreen[0] = _points[0].WorldCoords;
                    _tempActiveAreaOnScreen[1] = _points[3].WorldCoords;
                    _tempActiveAreaOnScreen[2] = _points[15].WorldCoords;
                    _tempActiveAreaOnScreen[3] = _points[12].WorldCoords;
                    break;
            }

            if (_isInOverlay)
            {
                _activeAreaOnScreen[0] = _tempActiveAreaOnScreen[0].Xy;
                _activeAreaOnScreen[1] = _tempActiveAreaOnScreen[1].Xy;
                _activeAreaOnScreen[2] = _tempActiveAreaOnScreen[2].Xy;
                _activeAreaOnScreen[3] = _tempActiveAreaOnScreen[3].Xy;
            }
            else
            {
                _activeAreaOnScreen[0] = inCamera.GetScreenCoord(_tempActiveAreaOnScreen[0]).Xy;
                _activeAreaOnScreen[1] = inCamera.GetScreenCoord(_tempActiveAreaOnScreen[1]).Xy;
                _activeAreaOnScreen[2] = inCamera.GetScreenCoord(_tempActiveAreaOnScreen[2]).Xy;
                _activeAreaOnScreen[3] = inCamera.GetScreenCoord(_tempActiveAreaOnScreen[3]).Xy;
            }

            return _activeAreaOnScreen;
        }
    }
}