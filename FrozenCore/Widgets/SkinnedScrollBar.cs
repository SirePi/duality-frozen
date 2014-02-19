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

        [NonSerialized]
        private GameObject _upButton;
        [NonSerialized]
        private GameObject _downButton;
        [NonSerialized]
        private GameObject _cursor;

        public int Minimum
        {
            get { return _min; }
            set 
            { 
                _min = value;
                UpdateCursor();
            }
        }

        public int Maximum
        {
            get { return _max; }
            set 
            { 
                _max = value;
                UpdateCursor();
            }
        }

        public int Value
        {
            get { return _value; }
            set 
            { 
                _value = value;
                UpdateCursor();
            }
        }

        public SkinnedScrollBar()
        {
            ActiveArea = Widgets.ActiveArea.LeftBorder;

            _min = 0;
            _max = 10;
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

                UpdateCursor();
            }
        }

        protected override void DrawCanvas(IDrawDevice inDevice, Canvas inCanvas)
        {
            
        }

        private void AddScrollDownButton()
        {
            _downButton = new GameObject("downButton", this.GameObj);

            Transform t = _downButton.AddComponent<Transform>();
            t.RelativePos = new Vector3(Rect.W / 2, Rect.H - Skin.Res.ButtonsSize.Y / 2, DELTA_Z);
            t.RelativeAngle = MathF.Pi;

            ScrollDownButton sdb = new ScrollDownButton();
            sdb.VisibilityGroup = this.VisibilityGroup;
            sdb.Skin = Skin.Res.ButtonsSkin;
            sdb.Rect = Rect.AlignCenter(0, 0, Skin.Res.ButtonsSize.X, Skin.Res.ButtonsSize.Y);

            _downButton.AddComponent<ScrollDownButton>(sdb);
            Scene.Current.AddObject(_downButton);
        }

        private void AddScrollUpButton()
        {
            _upButton = new GameObject("upButton", this.GameObj);

            Transform t = _upButton.AddComponent<Transform>();
            t.RelativePos = new Vector3(Rect.W / 2, Skin.Res.ButtonsSize.Y / 2, DELTA_Z);
            t.RelativeAngle = 0;

            ScrollUpButton sub = new ScrollUpButton();
            sub.VisibilityGroup = this.VisibilityGroup;
            sub.Skin = Skin.Res.ButtonsSkin;
            sub.Rect = Rect.AlignCenter(0, 0, Skin.Res.ButtonsSize.X, Skin.Res.ButtonsSize.Y);

            _upButton.AddComponent<ScrollUpButton>(sub);
            Scene.Current.AddObject(_upButton);
        }

        private void AddScrollCursor()
        {
            _cursor = new GameObject("cursor", this.GameObj);

            Transform t = _cursor.AddComponent<Transform>();
            t.RelativePos = new Vector3(Rect.W / 2, Rect.H / 2, DELTA_Z);
            t.RelativeAngle = 0;

            ScrollCursor sc = new ScrollCursor();
            sc.VisibilityGroup = this.VisibilityGroup;
            sc.Skin = Skin.Res.CursorSkin;
            sc.Rect = Rect.AlignCenter(0, 0, Skin.Res.CursorSize.X, Skin.Res.CursorSize.Y);

            _cursor.AddComponent<ScrollCursor>(sc);
            Scene.Current.AddObject(_cursor);
        }

        private void UpdateCursor()
        {
            if (_cursor != null)
            {
                _value = Math.Min(Value, _max);
                _value = Math.Max(Value, _min);

                float length = Rect.H - (Skin.Res.ButtonsSize.Y * 2) - (Skin.Res.CursorSize.Y);
                Vector3 direction = _downButton.Transform.Pos - _upButton.Transform.Pos;

                Vector3 origin = _upButton.Transform.Pos + (direction / 2) - (direction.Normalized * length / 2);

                _cursor.Transform.Pos = origin + (direction.Normalized * (Value - Minimum) * length / (Maximum - Minimum));
            }
        }

        internal float GetValueDelta()
        {
            float length = Rect.H - (Skin.Res.ButtonsSize.Y * 2) - (Skin.Res.CursorSize.Y);
            return length / (Maximum - Minimum);
        }
    }
}
