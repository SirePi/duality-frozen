// This code is provided under the MIT license. Originally by Alessandro Pilati.

using System;
using Duality;
using Duality.Components;
using Duality.Resources;
using Duality.VertexFormat;
using FrozenCore.Widgets.Skin;
using OpenTK;
using Duality.EditorHints;

namespace FrozenCore.Widgets
{
    [Serializable]
    [RequiredComponent(typeof(Transform))]
    public abstract class Widget : Component, ICmpRenderer, ICmpUpdatable, ICmpInitializable 
    {
        [NonSerialized]
        protected static readonly float DELTA_Z = -.001f;

        [NonSerialized]
        protected Polygon _activeAreaOnScreen;

        [NonSerialized]
        protected MultiSpacePoint[] _points;

        [NonSerialized]
        protected VertexC1P3T2[] _vertices;

        [NonSerialized]
        private bool _widgetEnabled;

        [NonSerialized]
        private bool _widgetActive;

        [EditorHintFlags(MemberFlags.Invisible)]
        float ICmpRenderer.BoundRadius
        {
            get { return Rect.BoundingRadius; }
        }

        [EditorHintFlags(MemberFlags.Invisible)]
        public bool IsWidgetEnabled
        {
            get { return _widgetEnabled; }
            set { _widgetEnabled = value; }
        }

        [EditorHintFlags(MemberFlags.Invisible)]
        public bool IsWidgetActive
        {
            get { return _widgetActive; }
            set { _widgetActive = value; }
        }
        private Rect _rect;

        [EditorHintDecimalPlaces(1)]
        public Rect Rect 
        {
            get { return _rect; }
            set { _rect = value; }
        }

        private VisibilityFlag _visiblityFlag;

        public VisibilityFlag VisibilityGroup
        {
            get { return _visiblityFlag; }
            set { _visiblityFlag = value; }
        }

        private ActiveArea _activeArea;

        public ActiveArea ActiveArea
        {
            get { return _activeArea; }
            set { _activeArea = value; }
        }

        public Widget()
        {
            VisibilityGroup = VisibilityFlag.Group0;

            _activeAreaOnScreen = new Polygon(4);
            _widgetEnabled = true;

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

        internal virtual Polygon GetCustomAreaOnScreen(Camera inCamera)
        {
            return Polygon.NO_POLYGON;
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

        internal abstract void KeyDown(OpenTK.Input.KeyboardKeyEventArgs e, WidgetController.ModifierKeys k);

        internal abstract void KeyUp(OpenTK.Input.KeyboardKeyEventArgs e, WidgetController.ModifierKeys k);

        internal abstract void MouseDown(OpenTK.Input.MouseButtonEventArgs e);

        internal abstract void MouseEnter();

        internal abstract void MouseLeave();

        internal abstract void MouseMove(OpenTK.Input.MouseMoveEventArgs e);

        internal abstract void MouseUp(OpenTK.Input.MouseButtonEventArgs e);

        internal abstract void MouseWheel(OpenTK.Input.MouseWheelEventArgs e);

        internal virtual void Activate()
        {
            _widgetActive = true;
        }

        internal virtual void Deactivate()
        {
            _widgetActive = false;
        }

        public void SetEnabled(bool inEnabled)
        {
            _widgetEnabled = inEnabled;
            OnEnabledChanged();
        }

        protected abstract void Draw(IDrawDevice inDevice);
        protected abstract void DrawCanvas(IDrawDevice inDevice, Canvas inCanvas);
        protected abstract void OnInit(Component.InitContext inContext);
        protected abstract void OnEnabledChanged();
        protected abstract void OnUpdate(float inSecondsPast);
        protected abstract Polygon GetDefaultActiveAreaOnScreen(Camera inCamera);

        void ICmpInitializable.OnInit(Component.InitContext context)
        {
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
    }
}