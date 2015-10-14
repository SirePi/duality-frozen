// This code is provided under the MIT license. Originally by Alessandro Pilati.

using Duality;
using Duality.Components;
using Duality.Drawing;
using Duality.Editor;
using Duality.Input;
using Duality.Resources;
using SnowyPeak.Duality.Plugin.Frozen.Core;
using SnowyPeak.Duality.Plugin.Frozen.Core.Geometry;
using SnowyPeak.Duality.Plugin.Frozen.UI.Properties;
using SnowyPeak.Duality.Plugin.Frozen.UI.Resources;
using System;

namespace SnowyPeak.Duality.Plugin.Frozen.UI.Widgets
{
    /// <summary>
    ///
    /// </summary>

    [RequiredComponent(typeof(Transform))]
    [EditorHintImage(ResNames.ImageWidget)]
    [EditorHintCategory(ResNames.CategoryWidgets)]
    public abstract class Widget : Component, ICmpRenderer, ICmpUpdatable, ICmpInitializable
    {
        private Widget _next, _previous;

        private Rect _rect, _clipRect;

        private WidgetStatus _status;

        private VisibilityFlag _visiblityFlag;

        private Skin _currentSkin;

        private ActiveArea _activeArea;

        private float _zOffset;

        internal float ZOffset
        {
            get { return _zOffset; }
        }

        protected ColorRgba _tint;

        public ColorRgba Tint
        {
            get { return _tint; }
            set { _tint = value; }
        }

        /// <summary>
        /// [GET / SET] The ActiveArea of the Widget that can react to mouse input such as
        /// Hover, Click, etc..
        /// </summary>
        public ActiveArea ActiveArea
        {
            get { return _activeArea; }
            set { _activeArea = value; }
        }

        [DontSerialize]
        private ContentRef<Appearance> _appearance;

        [DontSerialize]
        protected bool _isMouseOver;

        [EditorHintFlags(MemberFlags.Invisible)]
        public ContentRef<Appearance> BaseAppearance
        {
            get { return _appearance; }
        }

        /// <summary>
        ///
        /// </summary>
        public Widget()
        {
            _visiblityFlag = VisibilityFlag.Group0;
            _status = WidgetStatus.Normal;

            _areaOnScreen = new Polygon(4);
            _activeAreaOnScreen = new Polygon(4);
            _tempActiveAreaOnScreen = new Vector3[4];

            _rect = new Rect(0, 0, 50, 50);
            _clipRect = Rect.Empty;
            _tint = ColorRgba.White;

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
            Appearance = 0x0002,
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
            Normal = 1,
            Hover = 2,
            Active = 3,
            Disabled = 0
#pragma warning restore 1591
        }

        #region NonSerialized fields

        /// <summary>
        ///
        /// </summary>
        [DontSerialize]
        protected static readonly float DELTA_Z = -.01f;

        /// <summary>
        ///
        /// </summary>
        [DontSerialize]
        protected Polygon _activeAreaOnScreen;

        /// <summary>
        ///
        /// </summary>
        [DontSerialize]
        protected Polygon _areaOnScreen;

        /// <summary>
        ///
        /// </summary>
        [DontSerialize]
        protected DirtyFlags _dirtyFlags;

        /// <summary>
        ///
        /// </summary>
        [DontSerialize]
        protected bool _isInOverlay;

        /// <summary>
        ///
        /// </summary>
        [DontSerialize]
        protected MultiSpacePoint[] _points;

        /// <summary>
        ///
        /// </summary>
        [DontSerialize]
        protected bool _resized;

        /// <summary>
        ///
        /// </summary>
        [DontSerialize]
        protected Vector3[] _tempActiveAreaOnScreen;

        /// <summary>
        ///
        /// </summary>
        [DontSerialize]
        protected VertexC1P3T2[] _vertices;

        [DontSerialize]
        private bool _widgetActive;

        #endregion NonSerialized fields

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
        public Rect ClippingRect
        {
            get { return _clipRect; }
            set { _clipRect = value; }
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
                RecalcZOffset();
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
            if (Frozen.Core.Utilities.IsDualityEditor)
            {
                _appearance = GetAppearance();
            }

            if (_appearance != null)
            {
                _currentSkin = _appearance.Res.GetSkin(this.Status).Res;

                PrepareVertices(device);
                DrawVertices(device);
                DrawCustom(device);
                DrawCanvas(new Canvas(device));
            }
        }

        protected virtual void DrawCustom(IDrawDevice device)
        { }

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

            if ((_dirtyFlags & DirtyFlags.Appearance) != DirtyFlags.None)
            {
				_appearance = GetAppearance();
            }

            OnUpdate(Time.LastDelta / 1000f);

            _dirtyFlags = DirtyFlags.None;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="e"></param>
        /// <param name="k"></param>
        public virtual void KeyDown(KeyboardKeyEventArgs e, WidgetController.ModifierKeys k)
        { }

        /// <summary>
        ///
        /// </summary>
        /// <param name="e"></param>
        /// <param name="k"></param>
        public virtual void KeyUp(KeyboardKeyEventArgs e, WidgetController.ModifierKeys k)
        { }

        /// <summary>
        ///
        /// </summary>
        /// <param name="e"></param>
        public virtual void MouseDown(MouseButtonEventArgs e)
        { }

        /// <summary>
        ///
        /// </summary>
        public virtual void MouseEnter()
        {
            _isMouseOver = true;

            if (Status != WidgetStatus.Disabled)
            {
                Status = WidgetStatus.Hover;
            }
        }

        /// <summary>
        ///
        /// </summary>
        public virtual void MouseLeave()
        {
            _isMouseOver = false;

            if (Status != WidgetStatus.Disabled)
            {
                Status = WidgetStatus.Normal;
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="e"></param>
        public virtual void MouseMove(MouseMoveEventArgs e)
        { }

        /// <summary>
        ///
        /// </summary>
        /// <param name="e"></param>
        public virtual void MouseUp(MouseButtonEventArgs e)
        { }

        /// <summary>
        ///
        /// </summary>
        /// <param name="e"></param>
        public virtual void MouseWheel(MouseWheelEventArgs e)
        { }

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
            switch (_activeArea)
            {
                case ActiveArea.None:
                    return Polygon.NO_POLYGON;

                case ActiveArea.Custom:
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
        protected virtual void DrawVertices(IDrawDevice device)
        {
            // Repositioning the pieces
            /*****************************
             *  0     3| 4     7| 8    11
             *
             *  1     2| 5     6| 9    10
             * --    --+--    --+--    --
             * 12    15|16    19|20    23
             *
             * 13    14|17    18|21    22
             * --    --+--    --+--    --
             * 24    27|28    31|32    35
             *
             * 25    26|29    30|33    34
             *****************************/

            _vertices[0].Pos = _points[0].SceneCoords;
            _vertices[0].TexCoord = _points[0].UVCoords;
            _vertices[0].Color = _points[0].Tint;

            _vertices[1].Pos = _points[4].SceneCoords;
            _vertices[1].TexCoord = _points[4].UVCoords;
            _vertices[1].Color = _points[4].Tint;

            _vertices[2].Pos = _points[5].SceneCoords;
            _vertices[2].TexCoord = _points[5].UVCoords;
            _vertices[2].Color = _points[5].Tint;

            _vertices[3].Pos = _points[1].SceneCoords;
            _vertices[3].TexCoord = _points[1].UVCoords;
            _vertices[3].Color = _points[1].Tint;

            _vertices[4].Pos = _points[1].SceneCoords;
            _vertices[4].TexCoord = _points[1].UVCoords;
            _vertices[4].Color = _points[1].Tint;

            _vertices[5].Pos = _points[5].SceneCoords;
            _vertices[5].TexCoord = _points[5].UVCoords;
            _vertices[5].Color = _points[5].Tint;

            _vertices[6].Pos = _points[6].SceneCoords;
            _vertices[6].TexCoord = _points[6].UVCoords;
            _vertices[6].Color = _points[6].Tint;

            _vertices[7].Pos = _points[2].SceneCoords;
            _vertices[7].TexCoord = _points[2].UVCoords;
            _vertices[7].Color = _points[2].Tint;

            _vertices[8].Pos = _points[2].SceneCoords;
            _vertices[8].TexCoord = _points[2].UVCoords;
            _vertices[8].Color = _points[2].Tint;

            _vertices[9].Pos = _points[6].SceneCoords;
            _vertices[9].TexCoord = _points[6].UVCoords;
            _vertices[9].Color = _points[6].Tint;

            _vertices[10].Pos = _points[7].SceneCoords;
            _vertices[10].TexCoord = _points[7].UVCoords;
            _vertices[10].Color = _points[7].Tint;

            _vertices[11].Pos = _points[3].SceneCoords;
            _vertices[11].TexCoord = _points[3].UVCoords;
            _vertices[11].Color = _points[3].Tint;

            _vertices[12].Pos = _points[4].SceneCoords;
            _vertices[12].TexCoord = _points[4].UVCoords;
            _vertices[12].Color = _points[4].Tint;

            _vertices[13].Pos = _points[8].SceneCoords;
            _vertices[13].TexCoord = _points[8].UVCoords;
            _vertices[13].Color = _points[8].Tint;

            _vertices[14].Pos = _points[9].SceneCoords;
            _vertices[14].TexCoord = _points[9].UVCoords;
            _vertices[14].Color = _points[9].Tint;

            _vertices[15].Pos = _points[5].SceneCoords;
            _vertices[15].TexCoord = _points[5].UVCoords;
            _vertices[15].Color = _points[5].Tint;

            _vertices[16].Pos = _points[5].SceneCoords;
            _vertices[16].TexCoord = _points[5].UVCoords;
            _vertices[16].Color = _points[5].Tint;

            _vertices[17].Pos = _points[9].SceneCoords;
            _vertices[17].TexCoord = _points[9].UVCoords;
            _vertices[17].Color = _points[9].Tint;

            _vertices[18].Pos = _points[10].SceneCoords;
            _vertices[18].TexCoord = _points[10].UVCoords;
            _vertices[18].Color = _points[10].Tint;

            _vertices[19].Pos = _points[6].SceneCoords;
            _vertices[19].TexCoord = _points[6].UVCoords;
            _vertices[19].Color = _points[6].Tint;

            _vertices[20].Pos = _points[6].SceneCoords;
            _vertices[20].TexCoord = _points[6].UVCoords;
            _vertices[20].Color = _points[6].Tint;

            _vertices[21].Pos = _points[10].SceneCoords;
            _vertices[21].TexCoord = _points[10].UVCoords;
            _vertices[21].Color = _points[10].Tint;

            _vertices[22].Pos = _points[11].SceneCoords;
            _vertices[22].TexCoord = _points[11].UVCoords;
            _vertices[22].Color = _points[11].Tint;

            _vertices[23].Pos = _points[7].SceneCoords;
            _vertices[23].TexCoord = _points[7].UVCoords;
            _vertices[23].Color = _points[7].Tint;

            _vertices[24].Pos = _points[8].SceneCoords;
            _vertices[24].TexCoord = _points[8].UVCoords;
            _vertices[24].Color = _points[8].Tint;

            _vertices[25].Pos = _points[12].SceneCoords;
            _vertices[25].TexCoord = _points[12].UVCoords;
            _vertices[25].Color = _points[12].Tint;

            _vertices[26].Pos = _points[13].SceneCoords;
            _vertices[26].TexCoord = _points[13].UVCoords;
            _vertices[26].Color = _points[13].Tint;

            _vertices[27].Pos = _points[9].SceneCoords;
            _vertices[27].TexCoord = _points[9].UVCoords;
            _vertices[27].Color = _points[9].Tint;

            _vertices[28].Pos = _points[9].SceneCoords;
            _vertices[28].TexCoord = _points[9].UVCoords;
            _vertices[28].Color = _points[9].Tint;

            _vertices[29].Pos = _points[13].SceneCoords;
            _vertices[29].TexCoord = _points[13].UVCoords;
            _vertices[29].Color = _points[13].Tint;

            _vertices[30].Pos = _points[14].SceneCoords;
            _vertices[30].TexCoord = _points[14].UVCoords;
            _vertices[30].Color = _points[14].Tint;

            _vertices[31].Pos = _points[10].SceneCoords;
            _vertices[31].TexCoord = _points[10].UVCoords;
            _vertices[31].Color = _points[10].Tint;

            _vertices[32].Pos = _points[10].SceneCoords;
            _vertices[32].TexCoord = _points[10].UVCoords;
            _vertices[32].Color = _points[10].Tint;

            _vertices[33].Pos = _points[14].SceneCoords;
            _vertices[33].TexCoord = _points[14].UVCoords;
            _vertices[33].Color = _points[14].Tint;

            _vertices[34].Pos = _points[15].SceneCoords;
            _vertices[34].TexCoord = _points[15].UVCoords;
            _vertices[34].Color = _points[15].Tint;

            _vertices[35].Pos = _points[11].SceneCoords;
            _vertices[35].TexCoord = _points[11].UVCoords;
            _vertices[35].Color = _points[11].Tint;

            _currentSkin.Clip(ref _vertices, _appearance.Res.Border, _rect, _clipRect);
            _currentSkin.Draw(device, _vertices);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="inCanvas"></param>
        protected virtual void DrawCanvas(Canvas inCanvas)
        { }

        /// <summary>
        ///
        /// </summary>
        /// <param name="inContext"></param>
        protected virtual void OnInit(Component.InitContext inContext)
        {
            _dirtyFlags |= DirtyFlags.Appearance;
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void OnStatusChange()
        { }

        /// <summary>
        ///
        /// </summary>
        /// <param name="inSecondsPast"></param>
        protected virtual void OnUpdate(float inSecondsPast)
        {
            /*
            for(int i = 0; i < _points.Length; i++)
            {
                VisualLog.Default.DrawCircle(_points[i].WorldCoords.X, _points[i].WorldCoords.Y, _points[i].WorldCoords.Z - 10, 5);
            }
            */
        }

        private void RecalcZOffset()
        {
            GameObject go = this.GameObj;
            _zOffset = 0;

            while (go.Parent != null)
            {
                _zOffset += DELTA_Z;
                go = go.Parent;
            }
        }

        private void GameObj_EventParentChanged(object sender, GameObjectParentChangedEventArgs e)
        {
            RecalcZOffset();
        }

        private Polygon GetDefaultActiveAreaOnScreen(Camera inCamera)
        {
            switch (_activeArea)
            {
                case ActiveArea.LeftBorder:
                    _tempActiveAreaOnScreen[0] = _points[0].WorldCoords;
                    _tempActiveAreaOnScreen[1] = _points[1].WorldCoords;
                    _tempActiveAreaOnScreen[2] = _points[13].WorldCoords;
                    _tempActiveAreaOnScreen[3] = _points[12].WorldCoords;
                    break;

                case ActiveArea.TopBorder:
                    _tempActiveAreaOnScreen[0] = _points[0].WorldCoords;
                    _tempActiveAreaOnScreen[1] = _points[3].WorldCoords;
                    _tempActiveAreaOnScreen[2] = _points[7].WorldCoords;
                    _tempActiveAreaOnScreen[3] = _points[4].WorldCoords;
                    break;

                case ActiveArea.RightBorder:
                    _tempActiveAreaOnScreen[0] = _points[2].WorldCoords;
                    _tempActiveAreaOnScreen[1] = _points[3].WorldCoords;
                    _tempActiveAreaOnScreen[2] = _points[15].WorldCoords;
                    _tempActiveAreaOnScreen[3] = _points[14].WorldCoords;
                    break;

                case ActiveArea.BottomBorder:
                    _tempActiveAreaOnScreen[0] = _points[8].WorldCoords;
                    _tempActiveAreaOnScreen[1] = _points[11].WorldCoords;
                    _tempActiveAreaOnScreen[2] = _points[15].WorldCoords;
                    _tempActiveAreaOnScreen[3] = _points[12].WorldCoords;
                    break;

                case ActiveArea.Center:
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

        protected virtual void PrepareVertices(IDrawDevice inDevice)
        {
            Vector3 posTemp = GameObj.Transform.Pos;
            float scaleTemp = 1.0f;
            inDevice.PreprocessCoords(ref posTemp, ref scaleTemp);

            Vector2 xDot, yDot;
            Vector2 xDotWorld, yDotWorld;
            MathF.GetTransformDotVec(GameObj.Transform.Angle, scaleTemp, out xDot, out yDot);
            MathF.GetTransformDotVec(GameObj.Transform.Angle, GameObj.Transform.Scale, out xDotWorld, out yDotWorld);

            /********************
             *  0    1    2    3
             *  4    5    6    7
             *  8    9   10   11
             * 12   13   14   15
             ********************/
            _currentSkin.PrepareVertices(ref _points, _appearance.Res.Border, _rect, GameObj.Transform.Scale);
            Vector4 tintVector = _tint.ToVector4();

            for (int i = 0; i < _points.Length; i++)
            {
                _points[i].SceneCoords.Z = _zOffset;
                _points[i].WorldCoords = _points[i].SceneCoords;

                MathF.TransformDotVec(ref _points[i].SceneCoords, ref xDot, ref yDot);
                MathF.TransformDotVec(ref _points[i].WorldCoords, ref xDotWorld, ref yDotWorld);

                _points[i].SceneCoords += posTemp;
                _points[i].WorldCoords += GameObj.Transform.Pos;
                _points[i].Tint = Colors.FromBase255Vector4((_points[i].Tint.ToVector4() * tintVector) / 255f);
            }
        }

		private Appearance GetAppearance()
		{
			try { return GetBaseAppearance(); }
			catch { return DefaultGradientSkin.WIDGET.Widget.Res; }
		}

        protected abstract Appearance GetBaseAppearance();
    }
}