﻿using System;
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

namespace FrozenCore.Widgets
{
    [Serializable]
    public class Window : Widget
    {
        private ContentRef<WindowSkin> _skin;

        public ContentRef<WindowSkin> Skin 
        {
            get { return _skin ; }
            set 
            {
                _skin = value;
                BaseSkinRes = value.Res;
            }
        }

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

        [NonSerialized]
        private bool _isDragged;

        public Window()
        {
            Title = new FormattedText();
            _isDragged = false;
        }

        private void AddCloseButton()
        {
            GameObject button = new GameObject("closeButton", this.GameObj);

            Transform t = button.AddComponent<Transform>();
            t.RelativePos = new Vector3(Rect.W - Skin.Res.ButtonsSize.X, 0, -DELTA_Z);
            t.RelativeAngle = 0;

            CloseButton cb = button.AddComponent<CloseButton>();
            cb.VisibilityGroup = this.VisibilityGroup;
            cb.Skin = new ContentRef<BaseSkin>(Skin.Res.CloseButtonSkin);
            cb.Rect = new Rect(0, 0, Skin.Res.ButtonsSize.X, Skin.Res.ButtonsSize.Y);

            Scene.Current.AddObject(button);
        }

        private void AddMinimizeButton()
        {
            GameObject button = new GameObject("minimizeButton", this.GameObj);

            Transform t = button.AddComponent<Transform>();
            t.RelativePos = new Vector3(Rect.W - (Skin.Res.ButtonsSize.X * 3), 0, -DELTA_Z);
            t.RelativeAngle = 0;

            MinimizeButton mb = button.AddComponent<MinimizeButton>();
            mb.VisibilityGroup = this.VisibilityGroup;
            mb.Skin = new ContentRef<BaseSkin>(Skin.Res.MinimizeButtonSkin);
            mb.Rect = new Rect(0, 0, Skin.Res.ButtonsSize.X, Skin.Res.ButtonsSize.Y);

            Scene.Current.AddObject(button);
        }

        private void AddMaximizeButton()
        {
            GameObject button = new GameObject("maximizeButton", this.GameObj);

            Transform t = button.AddComponent<Transform>();
            t.RelativePos = new Vector3(Rect.W - (Skin.Res.ButtonsSize.X * 2), 0, -DELTA_Z);
            t.RelativeAngle = 0;

            MaximizeButton mb = button.AddComponent<MaximizeButton>();
            mb.VisibilityGroup = this.VisibilityGroup;
            mb.Skin = new ContentRef<BaseSkin>(Skin.Res.MaximizeButtonSkin);
            mb.Rect = new Rect(0, 0, Skin.Res.ButtonsSize.X, Skin.Res.ButtonsSize.Y);

            Scene.Current.AddObject(button);
        }

        private void AddRestoreButton()
        {
            GameObject button = new GameObject("restoreButton", this.GameObj);

            Transform t = button.AddComponent<Transform>();
            t.RelativePos = new Vector3(Rect.W - (Skin.Res.ButtonsSize.X * 2), 0, -DELTA_Z);
            t.RelativeAngle = 0;

            RestoreButton rb = button.AddComponent<RestoreButton>();
            rb.VisibilityGroup = this.VisibilityGroup;
            rb.Skin = new ContentRef<BaseSkin>(Skin.Res.RestoreButtonSkin);
            rb.Rect = new Rect(0, 0, Skin.Res.ButtonsSize.X, Skin.Res.ButtonsSize.Y);
            rb.IsWidgetEnabled = false;

            Scene.Current.AddObject(button);
        }

        public override void MouseDown(OpenTK.Input.MouseButtonEventArgs e)
        {
            base.MouseDown(e);

            if (_widgetEnabled && IsDraggable)
            {
                SetTextureTopLeft(Skin.Res.Origin.Active);
                _isDragged = true;
            }
        }

        public override void MouseUp(OpenTK.Input.MouseButtonEventArgs e)
        {
            base.MouseUp(e);

            if (_widgetEnabled && IsDraggable)
            {
                SetTextureTopLeft(Skin.Res.Origin.Normal);
                _isDragged = false;
            }
        }

        protected override void Draw(IDrawDevice inDevice, Canvas inCanvas)
        {
            if (Title != null)
            {
                Vector2 textOrigin = new Vector2(0, Title.Size.Y / 2);
                Vector3 titleLeft = (_points[1].WorldCoords + _points[5].WorldCoords) / 2;

                inCanvas.CurrentState.TransformHandle = textOrigin;
                inCanvas.CurrentState.TransformAngle = GameObj.Transform.Angle;

                inCanvas.DrawText(Title, titleLeft.X + textOrigin.X, titleLeft.Y + textOrigin.Y, titleLeft.Z - DELTA_Z, null, Alignment.Left);
            }
        }
          
        public override void MouseEnter()
        {
            base.MouseEnter();

            if (_widgetEnabled && Skin.Res != null && !_isDragged)
            {
                SetTextureTopLeft(Skin.Res.Origin.Hover);
            }
        }

        public override void MouseLeave()
        {
            base.MouseLeave();

            if (_widgetEnabled && Skin.Res != null && !_isDragged)
            {
                SetTextureTopLeft(Skin.Res.Origin.Normal);
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

        public override Polygon GetActiveAreaOnScreen(Duality.Components.Camera inCamera)
        {
            _activeAreaOnScreen[0] = inCamera.GetScreenCoord(_points[0].WorldCoords).Xy;
            _activeAreaOnScreen[1] = inCamera.GetScreenCoord(_points[3].WorldCoords).Xy;
            _activeAreaOnScreen[2] = inCamera.GetScreenCoord(_points[7].WorldCoords).Xy;
            _activeAreaOnScreen[3] = inCamera.GetScreenCoord(_points[4].WorldCoords).Xy;

            return _activeAreaOnScreen;
        }

        protected override void Initialize(Component.InitContext inContext)
        {
            BaseSkinRes = Skin.Res;

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