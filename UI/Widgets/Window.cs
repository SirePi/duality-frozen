// This code is provided under the MIT license. Originally by Alessandro Pilati.

using Duality;
using Duality.Components;
using Duality.Drawing;
using Duality.Editor;
using Duality.Input;
using Duality.Resources;
using SnowyPeak.Duality.Plugin.Frozen.Core;
using SnowyPeak.Duality.Plugin.Frozen.UI.Properties;
using SnowyPeak.Duality.Plugin.Frozen.UI.Resources;
using System;
using System.Collections.Generic;

namespace SnowyPeak.Duality.Plugin.Frozen.UI.Widgets
{
    /// <summary>
    ///
    /// </summary>
    internal enum WindowStatus
    {
#pragma warning disable 1591
        Normal = 0x00,
        Minimized = 0x01,
        Maximized = 0x02
#pragma warning restore 1591
    }

    /// <summary>
    /// A Window Widget
    /// </summary>

    [EditorHintImage(ResNames.ImageWindow)]
    [EditorHintCategory(ResNames.CategoryWidgets)]
    public class Window : Widget
    {
        #region NonSerialized fields

        [DontSerialize]
        private Dictionary<Widget, WidgetStatus> _childrenStatus;

        [DontSerialize]
        private GameObject _closeButton;

        [DontSerialize]
        private FormattedText _fText;

        [DontSerialize]
        private bool _isDragged;

        [DontSerialize]
        private GameObject _maximizeButton;

        [DontSerialize]
        private GameObject _minimizeButton;

        [DontSerialize]
        private Rect _normalRect;

        [DontSerialize]
        private GameObject _restoreButton;

        [DontSerialize]
        private WindowStatus _wStatus;

        #endregion NonSerialized fields

        private bool _canClose;
        private bool _canMaximize;
        private bool _canMinimize;
        private ContentRef<WindowAppearance> _windowAppearance;
        private bool _isDraggable;
        private Vector2 _maximizedSize;
        private string _title;
        private ColorRgba _titleColor;
        private ContentRef<Font> _titleFont;

        /// <summary>
        /// Constructor
        /// </summary>
        public Window()
        {
            ActiveArea = ActiveArea.TopBorder;

            _childrenStatus = new Dictionary<Widget, WidgetStatus>();
            _fText = new FormattedText();
            _titleColor = Colors.White;
            _isDragged = false;

            _wStatus = WindowStatus.Normal;

            Appearance = DefaultGradientSkin.WINDOW;
        }

        /// <summary>
        /// [GET / SET] if the Close button should be visible
        /// </summary>
        public bool CanClose
        {
            get { return _canClose; }
            set { _canClose = value; }
        }

        /// <summary>
        /// [GET / SET] if the Maximize button should be visible
        /// </summary>
        public bool CanMaximize
        {
            get { return _canMaximize; }
            set { _canMaximize = value; }
        }

        /// <summary>
        /// [GET / SET] if the Minimize button should be visible
        /// </summary>
        public bool CanMinimize
        {
            get { return _canMinimize; }
            set { _canMinimize = value; }
        }

        /// <summary>
        /// [GET / SET] the skin used for the Close button
        /// </summary>
        public ContentRef<WindowAppearance> Appearance
        {
            get { return _windowAppearance; }
            set
            {
                _windowAppearance = value;
                _dirtyFlags |= DirtyFlags.Appearance;
            }
        }

        /// <summary>
        /// [GET / SET] if the Window can be dragged around
        /// </summary>
        public bool IsDraggable
        {
            get { return _isDraggable; }
            set { _isDraggable = value; }
        }

        /// <summary>
        /// [GET / SET] the size of the Window when Maximized
        /// </summary>
        public Vector2 MaximizedSize
        {
            get { return _maximizedSize; }
            set { _maximizedSize = value; }
        }

        /// <summary>
        /// [GET / SET] The Text of the Title
        /// </summary>
        public string Title
        {
            get { return _title; }
            set { _title = value; }
        }

        /// <summary>
        /// [GET / SET] the Color of the Title Text
        /// </summary>
        public ColorRgba TitleColor
        {
            get { return _titleColor; }
            set { _titleColor = value; }
        }

        /// <summary>
        /// The Font of the Title Text
        /// </summary>
        public ContentRef<Font> TitleFont
        {
            get { return _titleFont; }
            set { _titleFont = value; }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="e"></param>
        public override void MouseDown(MouseButtonEventArgs e)
        {
            base.MouseDown(e);

            if (Status != WidgetStatus.Disabled && IsDraggable)
            {
                Status = WidgetStatus.Active;
                _isDragged = true;
            }
        }

        /// <summary>
        ///
        /// </summary>
        public override void MouseEnter()
        {
            if (Status != WidgetStatus.Disabled && !_isDragged)
            {
                Status = WidgetStatus.Hover;
            }
        }

        /// <summary>
        ///
        /// </summary>
        public override void MouseLeave()
        {
            if (Status != WidgetStatus.Disabled && !_isDragged)
            {
                Status = WidgetStatus.Normal;
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="e"></param>
        public override void MouseMove(MouseMoveEventArgs e)
        {
            base.MouseMove(e);

            if (_isDragged)
            {
                this.GameObj.Transform.Pos += (new Vector3(e.DeltaX, e.DeltaY, 0));
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="e"></param>
        public override void MouseUp(MouseButtonEventArgs e)
        {
            base.MouseUp(e);

            if (Status != WidgetStatus.Disabled && IsDraggable)
            {
                Status = WidgetStatus.Hover;
                _isDragged = false;
            }
        }

        /// <summary>
        ///
        /// </summary>
        internal void Maximize()
        {
            float deltaX = _maximizedSize.X - Rect.W;
            LayoutButtons(deltaX);

            Rect newRect = Rect;
            newRect.W = _maximizedSize.X;
            newRect.H = _maximizedSize.Y;
            Rect = newRect;

            _maximizeButton.Active = false;
            _restoreButton.Active = true;

            _restoreButton.Transform.Pos = _maximizeButton.Transform.Pos;

            _wStatus |= WindowStatus.Maximized;
            EnableChildren(true);
            OnResize();
        }

        /// <summary>
        ///
        /// </summary>
        internal void Minimize()
        {
            Rect newRect = Rect;
            bool isMinimized = (_wStatus & WindowStatus.Minimized) > 0;

            if (isMinimized)
            {
                if ((_wStatus & WindowStatus.Maximized) > 0)
                {
                    newRect.H = _maximizedSize.Y;
                }
                else
                {
                    newRect.H = _normalRect.H;
                }
            }
            else
            {
                newRect.H = _windowAppearance.Res.Widget.Res.Border.Y + _windowAppearance.Res.Widget.Res.Border.W;
            }
            Rect = newRect;

            _wStatus ^= WindowStatus.Minimized;
            EnableChildren(isMinimized);
            OnResize();
        }

        /// <summary>
        ///
        /// </summary>
        internal void Restore()
        {
            if ((_wStatus & WindowStatus.Maximized) > 0)
            {
                float deltaX = _maximizedSize.X - _normalRect.W;
                LayoutButtons(-deltaX);
            }

            Rect = _normalRect;

            _restoreButton.Active = false;
            if (_maximizeButton != null)
            {
                _maximizeButton.Active = true;
            }

            _wStatus ^= WindowStatus.Maximized;

            EnableChildren(true);
            OnResize();
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="inCanvas"></param>
        protected override void DrawCanvas(Canvas inCanvas)
        {
            if (!String.IsNullOrWhiteSpace(_title))
            {
                Vector3 titleLeft = (_points[1].WorldCoords + _points[5].WorldCoords) / 2;

                if (_titleFont.Res != null && _fText.Fonts[0] != _titleFont)
                {
                    _fText.Fonts[0] = _titleFont;
                }

                _fText.SourceText = _title;

                inCanvas.PushState();
                inCanvas.State.ColorTint = _titleColor;
                inCanvas.State.TransformAngle = GameObj.Transform.Angle;
                inCanvas.DrawText(_fText, titleLeft.X, titleLeft.Y, titleLeft.Z + DELTA_Z, null, Alignment.Left);
                inCanvas.PopState();
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="inContext"></param>
        protected override void OnInit(Component.InitContext inContext)
        {
            base.OnInit(inContext);

            _normalRect = Rect;
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void OnResize()
        { }

        /// <summary>
        ///
        /// </summary>
        protected override void OnStatusChange()
        {
            base.OnStatusChange();

            if (Status == WidgetStatus.Disabled)
            {
                _childrenStatus.Clear();

                foreach (Widget w in this.GameObj.GetComponentsInChildren<Widget>())
                {
                    _childrenStatus.Add(w, w.Status);
                    w.Status = WidgetStatus.Disabled;
                }
            }
            else if (Status == WidgetStatus.Normal)
            {
                foreach (Widget w in this.GameObj.GetComponentsInChildren<Widget>())
                {
                    if (_childrenStatus.ContainsKey(w))
                    {
                        w.Status = _childrenStatus[w];
                    }
                }
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="inSecondsPast"></param>
        protected override void OnUpdate(float inSecondsPast)
        {
            if (CanClose && _closeButton == null && !_windowAppearance.Res.Close.IsExplicitNull)
            {
                AddCloseButton();
            }
            if (CanMaximize && _maximizeButton == null && _maximizedSize.X >= Rect.W && _maximizedSize.Y >= Rect.H && !_windowAppearance.Res.Maximize.IsExplicitNull)
            {
                AddMaximizeButton();
            }
            if (CanMinimize && _minimizeButton == null && !_windowAppearance.Res.Minimize.IsExplicitNull)
            {
                AddMinimizeButton();
            }
            if ((CanMaximize || CanMinimize) && _restoreButton == null && !_windowAppearance.Res.Restore.IsExplicitNull)
            {
                AddRestoreButton();
            }

            if ((_dirtyFlags & DirtyFlags.Appearance) != DirtyFlags.None)
            {
                Rect buttonRect = Rect.Align(Alignment.TopLeft, 0, 0, _windowAppearance.Res.ButtonSize.X, _windowAppearance.Res.ButtonSize.Y);
                Rect closeRect = Rect.Align(Alignment.TopLeft, 0, 0, _windowAppearance.Res.CloseButtonSize.X, _windowAppearance.Res.CloseButtonSize.Y);

                if (_closeButton != null)
                {
                    _closeButton.GetComponent<Button>().Appearance = AppearanceManager.RequestAppearanceContentRef(_windowAppearance.Res.Close);
                    _minimizeButton.GetComponent<Button>().Rect = closeRect;
                }
                if (_maximizeButton != null)
                {
                    _maximizeButton.GetComponent<Button>().Appearance = AppearanceManager.RequestAppearanceContentRef(_windowAppearance.Res.Maximize);
                    _minimizeButton.GetComponent<Button>().Rect = buttonRect;
                }
                if (_minimizeButton != null)
                {
                    _minimizeButton.GetComponent<Button>().Appearance = AppearanceManager.RequestAppearanceContentRef(_windowAppearance.Res.Minimize);
                    _minimizeButton.GetComponent<Button>().Rect = buttonRect;
                }
                if (_restoreButton != null)
                {
                    _restoreButton.GetComponent<Button>().Appearance = AppearanceManager.RequestAppearanceContentRef(_windowAppearance.Res.Restore);
                    _minimizeButton.GetComponent<Button>().Rect = buttonRect;
                }
            }

            base.OnUpdate(inSecondsPast);
        }

        private void AddCloseButton()
        {
            _closeButton = new GameObject("closeButton", this.GameObj);

            Transform t = _closeButton.AddComponent<Transform>();
            t.RelativePos = new Vector3(Rect.W - _windowAppearance.Res.CloseButtonSize.X, 0, DELTA_Z);
            t.RelativeAngle = 0;

            CloseButton cb = new CloseButton();
            cb.VisibilityGroup = this.VisibilityGroup;
            cb.Appearance = AppearanceManager.RequestAppearanceContentRef(_windowAppearance.Res.Close);
            cb.Rect = Rect.Align(Alignment.TopLeft, 0, 0, _windowAppearance.Res.CloseButtonSize.X, _windowAppearance.Res.CloseButtonSize.Y);

            _closeButton.AddComponent<CloseButton>(cb);
            Scene.Current.AddObject(_closeButton);
        }

        private void AddMaximizeButton()
        {
            _maximizeButton = new GameObject("maximizeButton", this.GameObj);

            Transform t = _maximizeButton.AddComponent<Transform>();
            t.RelativePos = new Vector3(Rect.W - (_windowAppearance.Res.ButtonSize.X * 2), 0, DELTA_Z);
            t.RelativeAngle = 0;

            MaximizeButton mb = new MaximizeButton();
            mb.VisibilityGroup = this.VisibilityGroup;
            mb.Appearance = AppearanceManager.RequestAppearanceContentRef(_windowAppearance.Res.Maximize);
            mb.Rect = Rect.Align(Alignment.TopLeft, 0, 0, _windowAppearance.Res.ButtonSize.X, _windowAppearance.Res.ButtonSize.Y);

            _maximizeButton.AddComponent<MaximizeButton>(mb);
            Scene.Current.AddObject(_maximizeButton);
        }

        private void AddMinimizeButton()
        {
            _minimizeButton = new GameObject("minimizeButton", this.GameObj);

            Transform t = _minimizeButton.AddComponent<Transform>();
            t.RelativePos = new Vector3(Rect.W - (_windowAppearance.Res.ButtonSize.X * 3), 0, DELTA_Z);
            t.RelativeAngle = 0;

            MinimizeButton mb = new MinimizeButton();
            mb.VisibilityGroup = this.VisibilityGroup;
            mb.Appearance = AppearanceManager.RequestAppearanceContentRef(_windowAppearance.Res.Minimize);
            mb.Rect = Rect.Align(Alignment.TopLeft, 0, 0, _windowAppearance.Res.ButtonSize.X, _windowAppearance.Res.ButtonSize.Y);

            _minimizeButton.AddComponent<MinimizeButton>(mb);
            Scene.Current.AddObject(_minimizeButton);
        }

        private void AddRestoreButton()
        {
            _restoreButton = new GameObject("restoreButton", this.GameObj);

            Transform t = _restoreButton.AddComponent<Transform>();
            t.RelativePos = new Vector3(Rect.W - (_windowAppearance.Res.ButtonSize.X * 4), 0, DELTA_Z);
            t.RelativeAngle = 0;

            RestoreButton rb = new RestoreButton();
            rb.VisibilityGroup = this.VisibilityGroup;
            rb.Appearance = AppearanceManager.RequestAppearanceContentRef(_windowAppearance.Res.Restore);
            rb.Rect = Rect.Align(Alignment.TopLeft, 0, 0, _windowAppearance.Res.ButtonSize.X, _windowAppearance.Res.ButtonSize.Y);

            _restoreButton.Active = false;
            _restoreButton.AddComponent<RestoreButton>(rb);
            Scene.Current.AddObject(_restoreButton);
        }

        private void EnableChildren(bool inEnable)
        {
            foreach (GameObject go in this.GameObj.Children)
            {
                if (go != _closeButton && go != _minimizeButton && go != _maximizeButton && go != _restoreButton)
                {
                    go.Active = inEnable;
                }
            }
        }

        private void LayoutButtons(float inDeltaX)
        {
            if (CanClose && _closeButton != null)
            {
                MoveButton(_closeButton, inDeltaX);
            }

            if (CanMinimize && _minimizeButton != null)
            {
                MoveButton(_minimizeButton, inDeltaX);
            }

            if (CanMaximize && _maximizeButton != null)
            {
                MoveButton(_maximizeButton, inDeltaX);
            }

            if ((CanMinimize || CanMaximize) && _restoreButton != null)
            {
                MoveButton(_restoreButton, inDeltaX);
            }
        }

        private void MoveButton(GameObject inButton, float inDeltaX)
        {
            Vector3 pos = inButton.Transform.RelativePos;
            pos.X += inDeltaX;
            inButton.Transform.RelativePos = pos;
        }

        protected override Appearance GetBaseAppearance()
        {
            return _windowAppearance.Res.Widget.Res;
        }
    }
}