// This code is provided under the MIT license. Originally by Alessandro Pilati.

using System;
using Duality;
using Duality.Components;
using Duality.Drawing;
using Duality.Editor;
using Duality.Resources;
using OpenTK;
using SnowyPeak.Duality.Plugin.Frozen.Core;
using SnowyPeak.Duality.Plugin.Frozen.Core.Geometry;
using SnowyPeak.Duality.Plugin.Frozen.UI.Properties;

namespace SnowyPeak.Duality.Plugin.Frozen.UI.Widgets
{
    /// <summary>
    ///
    /// </summary>
    [Serializable]
    [RequiredComponent(typeof(Transform))]
    [EditorHintImage(typeof(Res), ResNames.ImageWidget)]
    [EditorHintCategory(typeof(Res), ResNames.CategoryWidgets)]
    public abstract class Widget : Component, ICmpRenderer, ICmpUpdatable, ICmpInitializable
    {
        private ActiveArea _activeArea;

        private Widget _next;

        private bool _overrideAutoZ;

        private Widget _previous;

        private Rect _rect;

        private WidgetStatus _status;

        private Rect _visibleRect;

        private VisibilityFlag _visiblityFlag;

        /// <summary>
        ///
        /// </summary>
        public Widget()
        {
            VisibilityGroup = VisibilityFlag.Group0;

            _areaOnScreen = new Polygon(4);
            _activeAreaOnScreen = new Polygon(4);
            _tempActiveAreaOnScreen = new Vector3[4];

            _rect = new Rect(0, 0, 50, 50);
            _visibleRect = Rect.Empty;

            _vertices = new VertexC1P3T2[36];
            _points = new MultiSpacePoint[16];

            for (int i = 0; i < _vertices.Length; i++)
            {
                _vertices[i] = new VertexC1P3T2();
            }
        }

        /// <summary>
        ///
        /// </summary>
        [Flags]
        public enum DirtyFlags
        {
#pragma warning disable 1591
            None = 0x0000,
            Status = 0x0001,
            Skin = 0x0002,
            Value = 0x0004,
            Custom1 = 0x0008,
            Custom2 = 0x0010,
            Custom3 = 0x0020,
            Custom4 = 0x0040,
            Custom5 = 0x0080,
            Custom6 = 0x0100,
            Custom7 = 0x0200,
            Custom8 = 0x0400,
            Custom9 = 0x0800
#pragma warning restore 1591
        }

        /// <summary>
        ///
        /// </summary>
        public enum WidgetStatus
        {
#pragma warning disable 1591
            Normal,
            Hover,
            Active,
            Disabled
#pragma warning restore 1591
        }

        #region NonSerialized fields

        /// <summary>
        ///
        /// </summary>
        [NonSerialized]
        protected static readonly float DELTA_Z = -.001f;

        /// <summary>
        ///
        /// </summary>
        [NonSerialized]
        protected Polygon _activeAreaOnScreen;

        /// <summary>
        ///
        /// </summary>
        [NonSerialized]
        protected Polygon _areaOnScreen;

        /// <summary>
        ///
        /// </summary>
        [NonSerialized]
        protected DirtyFlags _dirtyFlags;

        /// <summary>
        ///
        /// </summary>
        [NonSerialized]
        protected bool _isInOverlay;

        /// <summary>
        ///
        /// </summary>
        [NonSerialized]
        protected MultiSpacePoint[] _points;

        /// <summary>
        ///
        /// </summary>
        [NonSerialized]
        protected bool _resized;

        /// <summary>
        ///
        /// </summary>
        [NonSerialized]
        protected Vector3[] _tempActiveAreaOnScreen;

        /// <summary>
        ///
        /// </summary>
        [NonSerialized]
        protected VertexC1P3T2[] _vertices;

        [NonSerialized]
        private bool _widgetActive;

        #endregion NonSerialized fields

        /// <summary>
        /// [GET / SET] The ActiveArea of the Widget that can react to mouse input such as
        /// Hover, Click, etc..
        /// </summary>
        public ActiveArea ActiveArea
        {
            get { return _activeArea; }
            set { _activeArea = value; }
        }

        /// <summary>
        ///
        /// </summary>
        [EditorHintFlags(MemberFlags.Invisible)]
        float ICmpRenderer.BoundRadius
        {
            get { return Rect.BoundingRadius; }
        }

        [EditorHintFlags(MemberFlags.Invisible)]
        public bool IsInOverlay
        {
            get { return _isInOverlay; }
        }

        /// <summary>
        ///
        /// </summary>
        [EditorHintFlags(MemberFlags.Invisible)]
        public bool IsWidgetActive
        {
            get { return _widgetActive; }
        }

        /// <summary>
        ///
        /// </summary>
        public Widget NextWidget
        {
            get { return _next; }
            set { _next = value; }
        }
        /// <summary>
        ///
        /// </summary>
        public bool OverrideAutomaticZ
        {
            get { return _overrideAutoZ; }
            set { _overrideAutoZ = value; }
        }

        /// <summary>
        ///
        /// </summary>
        public Widget PreviousWidget
        {
            get { return _previous; }
            set { _previous = value; }
        }
        /// <summary>
        ///
        /// </summary>
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

        /// <summary>
        ///
        /// </summary>
        public WidgetStatus Status
        {
            get { return _status; }
            set
            {
                if (_status != value)
                {
                    _status = value;
                    _dirtyFlags |= DirtyFlags.Status;
                }
            }
        }

        /// <summary>
        ///
        /// </summary>
        public VisibilityFlag VisibilityGroup
        {
            get { return _visiblityFlag; }
            set { _visiblityFlag = value; }
        }

        /// <summary>
        ///
        /// </summary>
        [EditorHintDecimalPlaces(1)]
        public Rect VisibleRect
        {
            get { return _visibleRect; }
            set { _visibleRect = value; }
        }

        /// <summary>
        ///
        /// </summary>
        public void Close()
        {
            Scene.Current.RemoveObject(this.GameObj);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="inCamera"></param>
        /// <returns></returns>
        public virtual Polygon GetCustomAreaOnScreen(Camera inCamera)
        {
            return Polygon.NO_POLYGON;
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

            _dirtyFlags |= DirtyFlags.Status;
            OnInit(context);
        }

        void ICmpInitializable.OnShutdown(Component.ShutdownContext context)
        { }

        void ICmpRenderer.Draw(IDrawDevice device)
        {
            PrepareVertices(device);
            Draw(device);
            DrawCanvas(new Canvas(device));
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

            if ((_dirtyFlags & DirtyFlags.Status) != DirtyFlags.None)
            {
                OnStatusChange();
            }

            OnUpdate(Time.LastDelta / 1000f);

            _dirtyFlags = DirtyFlags.None;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="e"></param>
        /// <param name="k"></param>
        public abstract void KeyDown(OpenTK.Input.KeyboardKeyEventArgs e, WidgetController.ModifierKeys k);

        /// <summary>
        ///
        /// </summary>
        /// <param name="e"></param>
        /// <param name="k"></param>
        public abstract void KeyUp(OpenTK.Input.KeyboardKeyEventArgs e, WidgetController.ModifierKeys k);

        /// <summary>
        ///
        /// </summary>
        /// <param name="e"></param>
        public abstract void MouseDown(OpenTK.Input.MouseButtonEventArgs e);

        /// <summary>
        ///
        /// </summary>
        public abstract void MouseEnter();

        /// <summary>
        ///
        /// </summary>
        public abstract void MouseLeave();

        /// <summary>
        ///
        /// </summary>
        /// <param name="e"></param>
        public abstract void MouseMove(OpenTK.Input.MouseMoveEventArgs e);

        /// <summary>
        ///
        /// </summary>
        /// <param name="e"></param>
        public abstract void MouseUp(OpenTK.Input.MouseButtonEventArgs e);

        /// <summary>
        ///
        /// </summary>
        /// <param name="e"></param>
        public abstract void MouseWheel(OpenTK.Input.MouseWheelEventArgs e);

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

        /// <summary>
        ///
        /// </summary>
        /// <param name="inDevice"></param>
        protected abstract void Draw(IDrawDevice inDevice);

        /// <summary>
        ///
        /// </summary>
        /// <param name="inCanvas"></param>
        protected abstract void DrawCanvas(Canvas inCanvas);

        /// <summary>
        ///
        /// </summary>
        /// <param name="inContext"></param>
        protected abstract void OnInit(Component.InitContext inContext);

        /// <summary>
        ///
        /// </summary>
        protected abstract void OnStatusChange();

        /// <summary>
        ///
        /// </summary>
        /// <param name="inSecondsPast"></param>
        protected abstract void OnUpdate(float inSecondsPast);

        /// <summary>
        ///
        /// </summary>
        /// <param name="inDevice"></param>
        protected abstract void PrepareVertices(IDrawDevice inDevice);

        private void FixRelativeZ()
        {
            if (this.GameObj.Parent != null && this.GameObj.Parent.Transform != null && !_overrideAutoZ)
            {
                Vector3 pos = this.GameObj.Transform.Pos;
                pos.Z = this.GameObj.Parent.Transform.Pos.Z + DELTA_Z;

                this.GameObj.Transform.Pos = pos;
            }
        }

        private void GameObj_EventParentChanged(object sender, GameObjectParentChangedEventArgs e)
        {
            FixRelativeZ();
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