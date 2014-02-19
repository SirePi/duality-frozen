// This code is provided under the MIT license. Originally by Alessandro Pilati.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Duality;
using FrozenCore.Components;
using Duality.Components.Renderers;
using OpenTK;
using Duality.Resources;
using Duality.Components;
using FrozenCore.Widgets.Skin;
using Duality.ColorFormat;

namespace FrozenCore.Widgets
{
    [Serializable]
    public class SkinnedWindow : SkinnedWidget<WindowSkin>
    {
        private FormattedText _title;
        public FormattedText Title
        {
            get { return _title; }
            set { _title = value; }
        }
        private bool _isDraggable;
        public bool IsDraggable
        {
            get { return _isDraggable; }
            set { _isDraggable = value; }
        }

        private bool _canClose;
        private bool _canMaximize;
        private bool _canMinimize;

        private ColorRgba _titleColor;

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

        public ColorRgba TitleColor
        {
            get { return _titleColor; }
            set { _titleColor = value; }
        }

        [NonSerialized]
        private bool _isDragged;

        public SkinnedWindow()
        {
            ActiveArea = Widgets.ActiveArea.TopBorder;
            
            _title = new FormattedText();
            _titleColor = Colors.White;
            _isDragged = false;
        }

        private void AddCloseButton()
        {
            GameObject button = new GameObject("closeButton", this.GameObj);

            Transform t = button.AddComponent<Transform>();
            t.RelativePos = new Vector3(Rect.W - Skin.Res.ButtonsSize.X, 0, DELTA_Z);
            t.RelativeAngle = 0;

            CloseButton cb = new CloseButton();
            cb.VisibilityGroup = this.VisibilityGroup;
            cb.Skin = Skin.Res.CloseButtonSkin;
            cb.Rect = Rect.AlignTopLeft(0, 0, Skin.Res.ButtonsSize.X, Skin.Res.ButtonsSize.Y);

            button.AddComponent<CloseButton>(cb);
            Scene.Current.AddObject(button);
        }

        private void AddMinimizeButton()
        {
            GameObject button = new GameObject("minimizeButton", this.GameObj);

            Transform t = button.AddComponent<Transform>();
            t.RelativePos = new Vector3(Rect.W - (Skin.Res.ButtonsSize.X * 3), 0, DELTA_Z);
            t.RelativeAngle = 0;

            MinimizeButton mb = new MinimizeButton();
            mb.VisibilityGroup = this.VisibilityGroup;
            mb.Skin = Skin.Res.MinimizeButtonSkin;
            mb.Rect = Rect.AlignTopLeft(0, 0, Skin.Res.ButtonsSize.X, Skin.Res.ButtonsSize.Y);

            button.AddComponent<MinimizeButton>(mb);
            Scene.Current.AddObject(button);
        }

        private void AddMaximizeButton()
        {
            GameObject button = new GameObject("maximizeButton", this.GameObj);

            Transform t = button.AddComponent<Transform>();
            t.RelativePos = new Vector3(Rect.W - (Skin.Res.ButtonsSize.X * 2), 0, DELTA_Z);
            t.RelativeAngle = 0;

            MaximizeButton mb = new MaximizeButton();
            mb.VisibilityGroup = this.VisibilityGroup;
            mb.Skin = Skin.Res.MaximizeButtonSkin;
            mb.Rect = Rect.AlignTopLeft(0, 0, Skin.Res.ButtonsSize.X, Skin.Res.ButtonsSize.Y);

            button.AddComponent<MaximizeButton>(mb);
            Scene.Current.AddObject(button);
        }

        private void AddRestoreButton()
        {
            GameObject button = new GameObject("restoreButton", this.GameObj);

            Transform t = button.AddComponent<Transform>();
            t.RelativePos = new Vector3(Rect.W - (Skin.Res.ButtonsSize.X * 2), 0, DELTA_Z);
            t.RelativeAngle = 0;

            RestoreButton rb = new RestoreButton();
            rb.VisibilityGroup = this.VisibilityGroup;
            rb.Skin = Skin.Res.RestoreButtonSkin;
            rb.Rect = Rect.AlignTopLeft(0, 0, Skin.Res.ButtonsSize.X, Skin.Res.ButtonsSize.Y);
            rb.Active = false;

            button.AddComponent<RestoreButton>(rb);
            Scene.Current.AddObject(button);
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

        protected override void OnInit(Component.InitContext inContext)
        {
            base.OnInit(inContext);

            if (inContext == InitContext.Activate && !FrozenUtilities.IsDualityEditor)
            {
                if (CanClose)
                {
                    AddCloseButton();
                }
                if (CanMaximize)
                {
                    AddMaximizeButton();
                }
                if (CanMinimize)
                {
                    AddMinimizeButton();
                }
                if (CanMaximize || CanMinimize)
                {
                    AddRestoreButton();
                }
            }
        }
    }
}
