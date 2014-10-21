// This code is provided under the MIT license. Originally by Alessandro Pilati.

using System;
using System.Collections.Generic;
using Duality;
using Duality.Components;
using Duality.Drawing;
using Duality.Resources;
using FrozenCore.Resources.Widgets;
using OpenTK;

namespace FrozenCore.Widgets
{
    [Serializable]
    public class SkinnedWindow : SkinnedWidget
    {
        #region NonSerialized fields

        [NonSerialized]
        private GameObject _closeButton;

        [NonSerialized]
        private Dictionary<Widget, WidgetStatus> _childrenStatus;

        [NonSerialized]
        private bool _isDragged;

        [NonSerialized]
        private WindowStatus _wStatus;

        [NonSerialized]
        private GameObject _maximizeButton;

        [NonSerialized]
        private GameObject _minimizeButton;

        [NonSerialized]
        private Rect _normalRect;

        [NonSerialized]
        private GameObject _restoreButton;

        [NonSerialized]
        private FormattedText _fText;

        #endregion NonSerialized fields

        private Vector2 _buttonsSize;
        private bool _canClose;
        private bool _canMaximize;
        private bool _canMinimize;
        private Vector2 _closeButtonSize;
        private ContentRef<WidgetSkin> _closeButtonSkin;
        private bool _isDraggable;
        private ContentRef<WidgetSkin> _maximizeButtonSkin;
        private Vector2 _maximizedSize;
        private ContentRef<WidgetSkin> _minimizeButtonSkin;
        private ContentRef<WidgetSkin> _restoreButtonSkin;
        private ContentRef<Font> _titleFont;
        private string _title;
        private ColorRgba _titleColor;

        public Vector2 ButtonsSize
        {
            get { return _buttonsSize; }
            set 
            { 
                _buttonsSize = value;
                _dirtyFlags |= DirtyFlags.Custom6;
            }
        }

        public bool CanClose
        {
            get { return _canClose; }
            set { _canClose = value; }
        }

        public bool CanMaximize
        {
            get { return _canMaximize; }
            set { _canMaximize = value; }
        }

        public bool CanMinimize
        {
            get { return _canMinimize; }
            set { _canMinimize = value; }
        }

        public Vector2 CloseButtonSize
        {
            get { return _closeButtonSize; }
            set 
            { 
                _closeButtonSize = value;
                _dirtyFlags |= DirtyFlags.Custom5;
            }
        }

        public ContentRef<WidgetSkin> CloseButtonSkin
        {
            get { return _closeButtonSkin; }
            set 
            { 
                _closeButtonSkin = value;
                _dirtyFlags |= DirtyFlags.Custom1;
            }
        }

        public bool IsDraggable
        {
            get { return _isDraggable; }
            set { _isDraggable = value; }
        }

        public ContentRef<WidgetSkin> MaximizeButtonSkin
        {
            get { return _maximizeButtonSkin; }
            set 
            { 
                _maximizeButtonSkin = value;
                _dirtyFlags |= DirtyFlags.Custom2;
            }
        }

        public ContentRef<Font> TitleFont
        {
            get { return _titleFont; }
            set { _titleFont = value; }
        }

        public Vector2 MaximizedSize
        {
            get { return _maximizedSize; }
            set { _maximizedSize = value; }
        }

        public ContentRef<WidgetSkin> MinimizeButtonSkin
        {
            get { return _minimizeButtonSkin; }
            set 
            { 
                _minimizeButtonSkin = value;
                _dirtyFlags |= DirtyFlags.Custom3;
            }
        }

        public ContentRef<WidgetSkin> RestoreButtonSkin
        {
            get { return _restoreButtonSkin; }
            set
            {
                _restoreButtonSkin = value;
                _dirtyFlags |= DirtyFlags.Custom4;
            }
        }

        public string Title
        {
            get { return _title; }
            set { _title = value; }
        }

        public ColorRgba TitleColor
        {
            get { return _titleColor; }
            set { _titleColor = value; }
        }

        public SkinnedWindow()
        {
            ActiveArea = Widgets.ActiveArea.TopBorder;

            _childrenStatus = new Dictionary<Widget, WidgetStatus>();
            _fText = new FormattedText();
            _titleColor = Colors.White;
            _isDragged = false;

            _wStatus = WindowStatus.Normal;
        }

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
                newRect.H = Skin.Res.Border.Y + Skin.Res.Border.W;
            }
            Rect = newRect;

            _wStatus ^= WindowStatus.Minimized;
            EnableChildren(isMinimized);
            OnResize();
        }

        public override void MouseDown(OpenTK.Input.MouseButtonEventArgs e)
        {
            base.MouseDown(e);

            if (Status != WidgetStatus.Disabled && IsDraggable)
            {
                Status = WidgetStatus.Active;
                _isDragged = true;
            }
        }

        public override void MouseEnter()
        {
            if (Status != WidgetStatus.Disabled && Skin.Res != null && !_isDragged)
            {
                Status = WidgetStatus.Hover;
            }
        }

        public override void MouseLeave()
        {
            if (Status != WidgetStatus.Disabled && Skin.Res != null && !_isDragged)
            {
                Status = WidgetStatus.Normal;
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

        public override void MouseUp(OpenTK.Input.MouseButtonEventArgs e)
        {
            base.MouseUp(e);

            if (Status != WidgetStatus.Disabled && IsDraggable)
            {
                Status = WidgetStatus.Hover;
                _isDragged = false;
            }
        }

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

        protected override void OnInit(Component.InitContext inContext)
        {
            base.OnInit(inContext);

            _normalRect = Rect;
        }

        protected virtual void OnResize()
        { }

        private void AddCloseButton()
        {
            _closeButton = new GameObject("closeButton", this.GameObj);

            Transform t = _closeButton.AddComponent<Transform>();
            t.RelativePos = new Vector3(Rect.W - CloseButtonSize.X, 0, DELTA_Z);
            t.RelativeAngle = 0;

            CloseButton cb = new CloseButton();
            cb.VisibilityGroup = this.VisibilityGroup;
            cb.Skin = CloseButtonSkin;
            cb.Rect = Rect.AlignTopLeft(0, 0, CloseButtonSize.X, CloseButtonSize.Y);

            _closeButton.AddComponent<CloseButton>(cb);
            Scene.Current.AddObject(_closeButton);
        }

        private void AddMaximizeButton()
        {
            _maximizeButton = new GameObject("maximizeButton", this.GameObj);

            Transform t = _maximizeButton.AddComponent<Transform>();
            t.RelativePos = new Vector3(Rect.W - (ButtonsSize.X * 2), 0, DELTA_Z);
            t.RelativeAngle = 0;

            MaximizeButton mb = new MaximizeButton();
            mb.VisibilityGroup = this.VisibilityGroup;
            mb.Skin = MaximizeButtonSkin;
            mb.Rect = Rect.AlignTopLeft(0, 0, ButtonsSize.X, ButtonsSize.Y);

            _maximizeButton.AddComponent<MaximizeButton>(mb);
            Scene.Current.AddObject(_maximizeButton);
        }

        private void AddMinimizeButton()
        {
            _minimizeButton = new GameObject("minimizeButton", this.GameObj);

            Transform t = _minimizeButton.AddComponent<Transform>();
            t.RelativePos = new Vector3(Rect.W - (ButtonsSize.X * 3), 0, DELTA_Z);
            t.RelativeAngle = 0;

            MinimizeButton mb = new MinimizeButton();
            mb.VisibilityGroup = this.VisibilityGroup;
            mb.Skin = MinimizeButtonSkin;
            mb.Rect = Rect.AlignTopLeft(0, 0, ButtonsSize.X, ButtonsSize.Y);

            _minimizeButton.AddComponent<MinimizeButton>(mb);
            Scene.Current.AddObject(_minimizeButton);
        }

        private void AddRestoreButton()
        {
            _restoreButton = new GameObject("restoreButton", this.GameObj);

            Transform t = _restoreButton.AddComponent<Transform>();
            t.RelativePos = new Vector3(Rect.W - (ButtonsSize.X * 4), 0, DELTA_Z);
            t.RelativeAngle = 0;

            RestoreButton rb = new RestoreButton();
            rb.VisibilityGroup = this.VisibilityGroup;
            rb.Skin = RestoreButtonSkin;
            rb.Rect = Rect.AlignTopLeft(0, 0, ButtonsSize.X, ButtonsSize.Y);

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

        protected override void OnUpdate(float inSecondsPast)
        {
            base.OnUpdate(inSecondsPast);

            if (CanClose && _closeButton == null && _closeButtonSkin != null)
            {
                AddCloseButton();
            }
            if (CanMaximize && _maximizeButton == null && _maximizedSize.X >= Rect.W && _maximizedSize.Y >= Rect.H && _maximizeButtonSkin != null)
            {
                AddMaximizeButton();
            }
            if (CanMinimize && _minimizeButton == null && _minimizeButtonSkin != null)
            {
                AddMinimizeButton();
            }
            if ((CanMaximize || CanMinimize) && _restoreButton == null && _restoreButtonSkin != null)
            {
                AddRestoreButton();
            }

            if ((_dirtyFlags & DirtyFlags.Custom1) != DirtyFlags.None && _closeButton != null)
            {
                _closeButton.GetComponent<SkinnedWidget>().Skin = _closeButtonSkin;
            }
            if ((_dirtyFlags & DirtyFlags.Custom2) != DirtyFlags.None && _maximizeButton != null)
            {
                _maximizeButton.GetComponent<SkinnedWidget>().Skin = _maximizeButtonSkin;
            }
            if ((_dirtyFlags & DirtyFlags.Custom3) != DirtyFlags.None && _minimizeButton != null)
            {
                _minimizeButton.GetComponent<SkinnedWidget>().Skin = _minimizeButtonSkin;
            }
            if ((_dirtyFlags & DirtyFlags.Custom4) != DirtyFlags.None && _restoreButton != null)
            {
                _restoreButton.GetComponent<SkinnedWidget>().Skin = _restoreButtonSkin;
            }
            if ((_dirtyFlags & DirtyFlags.Custom5) != DirtyFlags.None && _closeButton != null)
            {
                _closeButton.GetComponent<SkinnedWidget>().Rect = Rect.AlignTopLeft(0, 0, _closeButtonSize.X, _closeButtonSize.Y);
            }
            if ((_dirtyFlags & DirtyFlags.Custom6) != DirtyFlags.None)
            {
                Rect newRect = Rect.AlignTopLeft(0, 0, _buttonsSize.X, _buttonsSize.Y);
                if (_maximizeButton != null)
                {
                    _maximizeButton.GetComponent<SkinnedWidget>().Rect = newRect;
                }
                if (_minimizeButton != null)
                {
                    _minimizeButton.GetComponent<SkinnedWidget>().Rect = newRect;
                }
                if (_restoreButton != null)
                {
                    _restoreButton.GetComponent<SkinnedWidget>().Rect = newRect;
                }
            }
        }

        private void MoveButton(GameObject inButton, float inDeltaX)
        {
            Vector3 pos = inButton.Transform.RelativePos;
            pos.X += inDeltaX;
            inButton.Transform.RelativePos = pos;
        }
    }

    internal enum WindowStatus
    {
        Normal = 0x00,
        Minimized = 0x01,
        Maximized = 0x02
    }
}