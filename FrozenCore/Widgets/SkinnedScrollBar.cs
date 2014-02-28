// This code is provided under the MIT license. Originally by Alessandro Pilati.

using System;
using Duality;
using Duality.Components;
using Duality.Resources;
using OpenTK;
using Duality.Drawing;
using Duality.Editor;
using FrozenCore.Widgets.Resources;

namespace FrozenCore.Widgets
{
    [Serializable]
    public class SkinnedScrollBar : SkinnedWidget
    {
        #region NonSerialized fields

        [NonSerialized]
        private object _valueChangedArgument;

        [NonSerialized]
        private GameObject _cursor;

        [NonSerialized]
        private GameObject _downButton;

        [NonSerialized]
        private GameObject _upButton;

        #endregion NonSerialized fields

        private ContentRef<Script> _onValueChanged;

        private ContentRef<WidgetSkin> _cursorSkin;
        private ContentRef<WidgetSkin> _upButtonSkin;
        private ContentRef<WidgetSkin> _downButtonSkin;

        private Vector2 _cursorSize;
        private Vector2 _buttonsSize;

        public Vector2 CursorSize
        {
            get { return _cursorSize; }
            set { _cursorSize = value; }
        }

        public Vector2 ButtonsSize
        {
            get { return _buttonsSize; }
            set { _buttonsSize = value; }
        }

        public ContentRef<WidgetSkin> CursorSkin
        {
            get { return _cursorSkin; }
            set { _cursorSkin = value; }
        }
        public ContentRef<WidgetSkin> UpButtonSkin
        {
            get { return _upButtonSkin; }
            set { _upButtonSkin = value; }
        }
        public ContentRef<WidgetSkin> DownButtonSkin
        {
            get { return _downButtonSkin; }
            set { _downButtonSkin = value; }
        }

        private int _max;
        private int _min;
        private int _scrollSpeed;
        private int _value;

        public int Maximum
        {
            get { return _max; }
            set
            {
                _max = value;
                UpdateCursor();
            }
        }
        public int Minimum
        {
            get { return _min; }
            set
            {
                _min = value;
                UpdateCursor();
            }
        }
        public ContentRef<Script> OnValueChanged
        {
            get { return _onValueChanged; }
            set { _onValueChanged = value; }
        }

        public int ScrollSpeed
        {
            get { return _scrollSpeed; }
            set { _scrollSpeed = value; }
        }
        public int Value
        {
            get { return _value; }
            set
            {
                if (_value != value)
                {
                    _value = value;
                    UpdateCursor();

                    if (_onValueChanged.Res != null)
                    {
                        _onValueChanged.Res.Execute(GameObj, _valueChangedArgument);
                    }
                }
            }
        }
        [EditorHintFlags(MemberFlags.Invisible)]
        public object ValueChangedArgument
        {
            private get { return _valueChangedArgument; }
            set { _valueChangedArgument = value; }
        }

        public SkinnedScrollBar()
        {
            ActiveArea = Widgets.ActiveArea.LeftBorder;

            _min = 0;
            _max = 10;
            _value = _min;
            _scrollSpeed = 1;
        }

        [EditorHintFlags(MemberFlags.Invisible)]
        internal float ValueDelta
        {
            get
            {
                float length = Rect.H - (ButtonsSize.Y * 2) - (CursorSize.Y);
                return length / (Maximum - Minimum);
            }
        }

        protected override void OnInit(Component.InitContext inContext)
        {
            base.OnInit(inContext);

            if (inContext == InitContext.Activate && !FrozenUtilities.IsDualityEditor)
            {
                if (_cursor == null)
                {
                    AddScrollUpButton();
                    AddScrollDownButton();
                    AddScrollCursor();
                }

                UpdateCursor();
            }
        }

        private void AddScrollCursor()
        {
            _cursor = new GameObject("cursor", this.GameObj);

            Transform t = _cursor.AddComponent<Transform>();
            t.RelativePos = new Vector3(Rect.W / 2, Rect.H / 2, DELTA_Z);
            t.RelativeAngle = 0;

            ScrollCursor sc = new ScrollCursor();
            sc.VisibilityGroup = this.VisibilityGroup;
            sc.Skin = CursorSkin;
            sc.Rect = Rect.AlignCenter(0, 0, CursorSize.X, CursorSize.Y);

            _cursor.AddComponent<ScrollCursor>(sc);
            Scene.Current.AddObject(_cursor);
        }

        private void AddScrollDownButton()
        {
            _downButton = new GameObject("downButton", this.GameObj);

            Transform t = _downButton.AddComponent<Transform>();
            t.RelativePos = new Vector3(Rect.W / 2, Rect.H - ButtonsSize.Y / 2, DELTA_Z);
            t.RelativeAngle = 0;

            ScrollDownButton sdb = new ScrollDownButton();
            sdb.VisibilityGroup = this.VisibilityGroup;
            sdb.Skin = DownButtonSkin;
            sdb.Rect = Rect.AlignCenter(0, 0, ButtonsSize.X, ButtonsSize.Y);
            sdb.LeftClickArgument = _scrollSpeed;

            _downButton.AddComponent<ScrollDownButton>(sdb);
            Scene.Current.AddObject(_downButton);
        }

        private void AddScrollUpButton()
        {
            _upButton = new GameObject("upButton", this.GameObj);

            Transform t = _upButton.AddComponent<Transform>();
            t.RelativePos = new Vector3(Rect.W / 2, ButtonsSize.Y / 2, DELTA_Z);
            t.RelativeAngle = 0;

            ScrollUpButton sub = new ScrollUpButton();
            sub.VisibilityGroup = this.VisibilityGroup;
            sub.Skin = UpButtonSkin;
            sub.Rect = Rect.AlignCenter(0, 0, ButtonsSize.X, ButtonsSize.Y);
            sub.LeftClickArgument = _scrollSpeed;

            _upButton.AddComponent<ScrollUpButton>(sub);
            Scene.Current.AddObject(_upButton);
        }

        private void UpdateCursor()
        {
            if (_cursor != null)
            {
                _value = Math.Min(Value, _max);
                _value = Math.Max(Value, _min);

                float length = Rect.H - (ButtonsSize.Y * 2) - (CursorSize.Y);
                Vector3 direction = _downButton.Transform.Pos - _upButton.Transform.Pos;

                Vector3 origin = _upButton.Transform.Pos + (direction / 2) - (direction.Normalized * length / 2);

                _cursor.Transform.Pos = origin + (direction.Normalized * (Value - Minimum) * length / (Maximum - Minimum));
            }
        }
    }
}