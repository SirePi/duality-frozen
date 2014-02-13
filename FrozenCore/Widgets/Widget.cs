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
        public static readonly Rect NO_ACTIVE_AREA;

        [NonSerialized]
        protected static readonly float DELTA_Z = .001f;

        [NonSerialized]
        protected Rect _activeArea;

        [NonSerialized]
        protected Polygon _activeAreaOnScreen;

        [NonSerialized]
        protected MultiSpacePoint[] _points;

        [NonSerialized]
        protected VertexC1P3T2[] _vertices;

        [NonSerialized]
        protected bool _widgetEnabled;

        [EditorHintFlags(MemberFlags.Invisible)]
        float ICmpRenderer.BoundRadius
        {
            get { return 0; } //Size.BoundingRadius; }
        }

        [EditorHintFlags(MemberFlags.Invisible)]
        public bool IsWidgetEnabled
        {
            get { return _widgetEnabled; }
            set { _widgetEnabled = value; }
        }

        private Rect _rect;
        /// <summary>
        /// [GET/SET] The Previous InputReceiverVisual in the GUI (not used)
        /// </summary>
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

        public Widget()
        {
            VisibilityGroup = VisibilityFlag.Group0;

            _activeArea = NO_ACTIVE_AREA;
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

        public abstract Polygon GetActiveAreaOnScreen(Camera inCamera);

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

        public virtual void KeyDown(OpenTK.Input.KeyboardKeyEventArgs e, WidgetController.ModifierKeys k)
        {
        }

        public virtual void KeyUp(OpenTK.Input.KeyboardKeyEventArgs e, WidgetController.ModifierKeys k)
        {
        }

        public virtual void MouseDown(OpenTK.Input.MouseButtonEventArgs e)
        {
        }

        public virtual void MouseEnter()
        {
        }

        public virtual void MouseLeave()
        {
        }

        public virtual void MouseMove(OpenTK.Input.MouseMoveEventArgs e)
        {
        }

        public virtual void MouseUp(OpenTK.Input.MouseButtonEventArgs e)
        {
        }

        public virtual void MouseWheel(OpenTK.Input.MouseWheelEventArgs e)
        {
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