// This code is provided under the MIT license. Originally by Alessandro Pilati.

using System;
using Duality;
using Duality.Drawing;
using Duality.Components;
using Duality.Resources;
using FrozenCore.Widgets.Skin;
using OpenTK;

namespace FrozenCore.Widgets
{
    [Serializable]
    public class SkinnedWindow : SkinnedWidget<WindowSkin>
    {
        #region NonSerialized fields

        [NonSerialized]
        private GameObject _closeButton;

        [NonSerialized]
        private bool _isDragged;

        [NonSerialized]
        private GameObject _maximizeButton;

        [NonSerialized]
        private GameObject _minimizeButton;

        [NonSerialized]
        private GameObject _restoreButton;

        [NonSerialized]
        private bool _isMinimized;

        [NonSerialized]
        private Rect _normalRect;

        #endregion NonSerialized fields

        private bool _canClose;
        private bool _canMaximize;
        private bool _canMinimize;
        private bool _isDraggable;
        private FormattedText _title;
        private ColorRgba _titleColor;
        private Vector2 _maximizedSize;

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
        public bool IsDraggable
        {
            get { return _isDraggable; }
            set { _isDraggable = value; }
        }
        public FormattedText Title
        {
            get { return _title; }
            set { _title = value; }
        }
        public ColorRgba TitleColor
        {
            get { return _titleColor; }
            set { _titleColor = value; }
        }
        public Vector2 MaximizedSize
        {
            get { return _maximizedSize; }
            set { _maximizedSize = value; }
        }

        public SkinnedWindow()
        {
            ActiveArea = Widgets.ActiveArea.TopBorder;

            _title = new FormattedText();
            _titleColor = Colors.White;
            _isDragged = false;
        }

        internal override void MouseDown(OpenTK.Input.MouseButtonEventArgs e)
        {
            base.MouseDown(e);

            if (IsWidgetEnabled && IsDraggable)
            {
                SetTextureTopLeft(Skin.Res.Origin.Active);
                _isDragged = true;
            }
        }

        internal override void MouseEnter()
        {
            if (IsWidgetEnabled && Skin.Res != null && !_isDragged)
            {
                SetTextureTopLeft(Skin.Res.Origin.Hover);
            }
        }

        internal override void MouseLeave()
        {
            if (IsWidgetEnabled && Skin.Res != null && !_isDragged)
            {
                SetTextureTopLeft(Skin.Res.Origin.Normal);
            }
        }

        internal override void MouseMove(OpenTK.Input.MouseMoveEventArgs e)
        {
            base.MouseMove(e);

            if (_isDragged)
            {
                this.GameObj.Transform.Pos += (new Vector3(e.XDelta, e.YDelta, 0));
            }
        }

        internal override void MouseUp(OpenTK.Input.MouseButtonEventArgs e)
        {
            base.MouseUp(e);

            if (IsWidgetEnabled && IsDraggable)
            {
                SetTextureTopLeft(Skin.Res.Origin.Hover);
                _isDragged = false;
            }
        }

        protected override void DrawCanvas(IDrawDevice inDevice, Canvas inCanvas)
        {
            if (Title != null)
            {
                Vector3 titleLeft = (_points[1].WorldCoords + _points[5].WorldCoords) / 2;

                inCanvas.PushState();
                inCanvas.State.ColorTint = _titleColor;
                inCanvas.State.TransformAngle = GameObj.Transform.Angle;
                inCanvas.DrawText(Title, titleLeft.X, titleLeft.Y, titleLeft.Z + DELTA_Z, null, Alignment.Left);
                inCanvas.PopState();
            }
        }

        protected override void OnInit(Component.InitContext inContext)
        {
            base.OnInit(inContext);
            _normalRect = Rect;

            if (inContext == InitContext.Activate && !FrozenUtilities.IsDualityEditor)
            {
                if (CanClose && _closeButton == null)
                {
                    AddCloseButton();
                }
                if (CanMaximize && _maximizeButton == null && _maximizedSize.X >= Rect.W && _maximizedSize.Y >= Rect.H)
                {
                    AddMaximizeButton();
                }
                if (CanMinimize && _minimizeButton == null)
                {
                    AddMinimizeButton();
                }
                if ((CanMaximize || CanMinimize) && _restoreButton == null)
                {
                    AddRestoreButton();
                }
            }
        }

        private void AddCloseButton()
        {
            _closeButton = new GameObject("closeButton", this.GameObj);

            Transform t = _closeButton.AddComponent<Transform>();
            t.RelativePos = new Vector3(Rect.W - Skin.Res.ButtonsSize.X, 0, DELTA_Z);
            t.RelativeAngle = 0;

            CloseButton cb = new CloseButton();
            cb.VisibilityGroup = this.VisibilityGroup;
            cb.Skin = Skin.Res.CloseButtonSkin;
            cb.Rect = Rect.AlignTopLeft(0, 0, Skin.Res.ButtonsSize.X, Skin.Res.ButtonsSize.Y);

            _closeButton.AddComponent<CloseButton>(cb);
            Scene.Current.AddObject(_closeButton);
        }

        private void AddMaximizeButton()
        {
            _maximizeButton = new GameObject("maximizeButton", this.GameObj);

            Transform t = _maximizeButton.AddComponent<Transform>();
            t.RelativePos = new Vector3(Rect.W - (Skin.Res.ButtonsSize.X * 2), 0, DELTA_Z);
            t.RelativeAngle = 0;

            MaximizeButton mb = new MaximizeButton();
            mb.VisibilityGroup = this.VisibilityGroup;
            mb.Skin = Skin.Res.MaximizeButtonSkin;
            mb.Rect = Rect.AlignTopLeft(0, 0, Skin.Res.ButtonsSize.X, Skin.Res.ButtonsSize.Y);

            _maximizeButton.AddComponent<MaximizeButton>(mb);
            Scene.Current.AddObject(_maximizeButton);
        }

        private void AddMinimizeButton()
        {
            _minimizeButton = new GameObject("minimizeButton", this.GameObj);

            Transform t = _minimizeButton.AddComponent<Transform>();
            t.RelativePos = new Vector3(Rect.W - (Skin.Res.ButtonsSize.X * 3), 0, DELTA_Z);
            t.RelativeAngle = 0;

            MinimizeButton mb = new MinimizeButton();
            mb.VisibilityGroup = this.VisibilityGroup;
            mb.Skin = Skin.Res.MinimizeButtonSkin;
            mb.Rect = Rect.AlignTopLeft(0, 0, Skin.Res.ButtonsSize.X, Skin.Res.ButtonsSize.Y);

            _minimizeButton.AddComponent<MinimizeButton>(mb);
            Scene.Current.AddObject(_minimizeButton);
        }

        private void AddRestoreButton()
        {
            _restoreButton = new GameObject("restoreButton", this.GameObj);

            Transform t = _restoreButton.AddComponent<Transform>();
            t.RelativePos = new Vector3(Rect.W - (Skin.Res.ButtonsSize.X * 4), 0, DELTA_Z);
            t.RelativeAngle = 0;

            RestoreButton rb = new RestoreButton();
            rb.VisibilityGroup = this.VisibilityGroup;
            rb.Skin = Skin.Res.RestoreButtonSkin;
            rb.Rect = Rect.AlignTopLeft(0, 0, Skin.Res.ButtonsSize.X, Skin.Res.ButtonsSize.Y);

            _restoreButton.Active = false;
            _restoreButton.AddComponent<RestoreButton>(rb);
            Scene.Current.AddObject(_restoreButton);
        }

        internal void Minimize()
        {
            _isMinimized = true;

            Rect newRect = _normalRect;
            newRect.H = Skin.Res.Border.Y + Skin.Res.Border.W;
            Rect = newRect;

            _minimizeButton.Active = false;
            _restoreButton.Active = true;
            if (_maximizeButton != null)
            {
                _maximizeButton.Active = true;
            }

            _restoreButton.Transform.Pos = _minimizeButton.Transform.Pos;

            EnableChildren(false);
            OnResize();
        }

        internal void Maximize()
        {
            _isMinimized = false;

            Rect newRect = Rect;
            newRect.W = _maximizedSize.X;
            newRect.H = _maximizedSize.Y;
            Rect = newRect;

            _maximizeButton.Active = false;
            _restoreButton.Active = true;
            if (_minimizeButton != null)
            {
                _minimizeButton.Active = true;
            }

            _restoreButton.Transform.Pos = _maximizeButton.Transform.Pos;

            float deltaX = _maximizedSize.X - Rect.W;
            LayoutButtons(deltaX);

            EnableChildren(true);
            OnResize();
        }

        internal void Restore()
        {
            if (!_isMinimized)
            {
                float deltaX = _maximizedSize.X - Rect.W;
                LayoutButtons(-deltaX);
            }

            _isMinimized = false;

            Rect = _normalRect;

            _restoreButton.Active = false;
            if (_minimizeButton != null)
            {
                _minimizeButton.Active = true;
            }
            if (_maximizeButton != null)
            {
                _maximizeButton.Active = true;
            }

            EnableChildren(true);
            OnResize();
        }

        private void LayoutButtons(float inDeltaX)
        {
            if(CanClose && _closeButton != null) 
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

        protected virtual void OnResize()
        {

        }
    }
}