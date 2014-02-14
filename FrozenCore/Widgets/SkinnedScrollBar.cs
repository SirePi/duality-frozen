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
using FrozenCore.Widgets.Skin;
using Duality.EditorHints;
using Duality.Components;

namespace FrozenCore.Widgets
{
    [Serializable]
    public class SkinnedScrollBar : SkinnedWidget<ScrollBarSkin>
    {
        private int _min;
        private int _max;
        private int _value;

        public int Minimum
        {
            get { return _min; }
            set { _min = value; }
        }

        public int Maximum
        {
            get { return _max; }
            set { _max = value; }
        }

        public int Value
        {
            get { return _value; }
            set { _value = value; }
        }

        public SkinnedScrollBar()
        {
            _min = 1;
            _max = 100;
            _value = _min;
        }

        protected override void OnInit(Component.InitContext inContext)
        {
            base.OnInit(inContext);

            if (inContext == InitContext.Activate && !FrozenUtilities.IsDualityEditor)
            {
                AddScrollUpButton();
                AddScrollDownButton();
                AddScrollCursor();
            }
        }

        protected override void DrawCanvas(IDrawDevice inDevice, Canvas inCanvas)
        {
            
        }

        internal override Polygon GetActiveAreaOnScreen(Camera inCamera)
        {
            return Polygon.NO_POLYGON;
        }

        private void AddScrollDownButton()
        {
            GameObject button = new GameObject("downButton", this.GameObj);

            Transform t = button.AddComponent<Transform>();
            t.RelativePos = new Vector3(Rect.W / 2, Rect.H - Skin.Res.ButtonsSize.Y / 2, DELTA_Z);
            t.RelativeAngle = MathF.Pi;

            ScrollDownButton sdb = new ScrollDownButton();
            sdb.VisibilityGroup = this.VisibilityGroup;
            sdb.Skin = Skin.Res.ButtonsSkin;
            sdb.Rect = new Rect(Skin.Res.ButtonsSize.X / 2, Skin.Res.ButtonsSize.Y / 2, Skin.Res.ButtonsSize.X, Skin.Res.ButtonsSize.Y);

            button.AddComponent<ScrollDownButton>(sdb);
            Scene.Current.AddObject(button);
        }

        private void AddScrollUpButton()
        {
            GameObject button = new GameObject("upButton", this.GameObj);

            Transform t = button.AddComponent<Transform>();
            t.RelativePos = new Vector3(Rect.W / 2, Skin.Res.ButtonsSize.Y / 2, DELTA_Z);
            t.RelativeAngle = 0;

            ScrollUpButton sub = new ScrollUpButton();
            sub.VisibilityGroup = this.VisibilityGroup;
            sub.Skin = Skin.Res.ButtonsSkin;
            sub.Rect = new Rect(Skin.Res.ButtonsSize.X / 2, Skin.Res.ButtonsSize.Y / 2, Skin.Res.ButtonsSize.X, Skin.Res.ButtonsSize.Y);

            button.AddComponent<ScrollUpButton>(sub);
            Scene.Current.AddObject(button);
        }

        private void AddScrollCursor()
        {
            GameObject cursor = new GameObject("cursor", this.GameObj);

            Transform t = cursor.AddComponent<Transform>();
            t.RelativePos = new Vector3(Rect.W / 2, Rect.H / 2, DELTA_Z);
            t.RelativeAngle = 0;

            ScrollCursor sc = new ScrollCursor();
            sc.VisibilityGroup = this.VisibilityGroup;
            sc.Skin = Skin.Res.CursorSkin;
            sc.Rect = new Rect(Skin.Res.CursorSize.X / 2, Skin.Res.CursorSize.Y / 2, Skin.Res.CursorSize.X, Skin.Res.CursorSize.Y);

            cursor.AddComponent<ScrollCursor>(sc);
            Scene.Current.AddObject(cursor);
        }
    }
}
