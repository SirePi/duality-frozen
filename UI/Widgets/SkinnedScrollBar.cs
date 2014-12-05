﻿// This code is provided under the MIT license. Originally by Alessandro Pilati.

using System;
using Duality;
using Duality.Components;
using Duality.Editor;
using Duality.Resources;
using OpenTK;
using SnowyPeak.Duality.Plugin.Frozen.UI.Properties;
using SnowyPeak.Duality.Plugin.Frozen.UI.Resources;

namespace SnowyPeak.Duality.Plugin.Frozen.UI.Widgets
{
    /// <summary>
    /// A Scrollbar Widget
    /// </summary>
    [Serializable]
    [EditorHintImage(typeof(Res), ResNames.ImageScrollBar)]
    [EditorHintCategory(typeof(Res), ResNames.CategoryWidgets)]
    public class SkinnedScrollBar : SkinnedWidget
    {
        #region NonSerialized fields

        [NonSerialized]
        private GameObject _cursor;

        [NonSerialized]
        private GameObject _decreaseButton;

        [NonSerialized]
        private GameObject _increaseButton;

        #endregion NonSerialized fields

        private Vector2 _buttonsSize;
        private Vector2 _cursorSize;
        private ContentRef<WidgetSkin> _cursorSkin;
        private ContentRef<WidgetSkin> _decreaseButtonSkin;
        private ContentRef<WidgetSkin> _increaseButtonSkin;
        private int _max;
        private int _min;
        private ContentRef<Script> _onValueChanged;
        private int _scrollSpeed;
        private int _value;
        private object _valueChangedArgument;

        /// <summary>
        /// Constructor
        /// </summary>
        public SkinnedScrollBar()
        {
            ActiveArea = Widgets.ActiveArea.LeftBorder;

            _min = 0;
            _max = 10;
            _value = _min;
            _scrollSpeed = 1;
        }

        /// <summary>
        /// [GET / SET] the size of the Scrollbar buttons
        /// </summary>
        public Vector2 ButtonsSize
        {
            get { return _buttonsSize; }
            set { _buttonsSize = value; }
        }

        /// <summary>
        /// [GET / SET] the size of the Scrollbar cursor
        /// </summary>
        public Vector2 CursorSize
        {
            get { return _cursorSize; }
            set { _cursorSize = value; }
        }

        /// <summary>
        /// [GET / SET] the Skin used for the Scrollbar Cursor
        /// </summary>
        public ContentRef<WidgetSkin> CursorSkin
        {
            get { return _cursorSkin; }
            set
            {
                _cursorSkin = value;
                _dirtyFlags |= DirtyFlags.Custom1;
            }
        }

        /// <summary>
        /// [GET / SET] the Skin used for the Scrollbar Decrease button
        /// </summary>
        public ContentRef<WidgetSkin> DecreaseButtonSkin
        {
            get { return _decreaseButtonSkin; }
            set
            {
                _decreaseButtonSkin = value;
                _dirtyFlags |= DirtyFlags.Custom2;
            }
        }

        /// <summary>
        /// [GET / SET] the Skin used for the Scrollbar Increase button
        /// </summary>
        public ContentRef<WidgetSkin> IncreaseButtonSkin
        {
            get { return _increaseButtonSkin; }
            set
            {
                _increaseButtonSkin = value;
                _dirtyFlags |= DirtyFlags.Custom3;
            }
        }

        /// <summary>
        /// [GET / SET] the Maximum value
        /// </summary>
        public int Maximum
        {
            get { return _max; }
            set
            {
                _max = value;
                _dirtyFlags |= DirtyFlags.Value;
            }
        }

        /// <summary>
        /// [GET / SET] the Minimum value
        /// </summary>
        public int Minimum
        {
            get { return _min; }
            set
            {
                _min = value;
                _dirtyFlags |= DirtyFlags.Value;
            }
        }

        /// <summary>
        ///
        /// </summary>
        public ContentRef<Script> OnValueChanged
        {
            get { return _onValueChanged; }
            set { _onValueChanged = value; }
        }

        /// <summary>
        /// [GET / SET] the speed, in pixels/second of scrolling
        /// </summary>
        public int ScrollSpeed
        {
            get { return _scrollSpeed; }
            set { _scrollSpeed = value; }
        }

        /// <summary>
        /// [GET / SET] the current Value
        /// </summary>
        public int Value
        {
            get { return _value; }
            set
            {
                if (_value != value)
                {
                    _value = value;
                    _dirtyFlags |= DirtyFlags.Value;

                    if (_onValueChanged.Res != null)
                    {
                        _onValueChanged.Res.Execute(GameObj, _valueChangedArgument);
                    }
                }
            }
        }

        /// <summary>
        ///
        /// </summary>
        public object ValueChangedArgument
        {
            private get { return _valueChangedArgument; }
            set { _valueChangedArgument = value; }
        }

        /// <summary>
        ///
        /// </summary>
        [EditorHintFlags(MemberFlags.Invisible)]
        internal float ValueDelta
        {
            get
            {
                float length = Rect.H - (ButtonsSize.Y * 2) - (CursorSize.Y);
                return length / (Maximum - Minimum);
            }
        }

        /// <summary>
        ///
        /// </summary>
        protected override void OnStatusChange()
        {
            base.OnStatusChange();

            if (_cursor != null)
            {
                _cursor.GetComponent<Widget>().Status = Status;
            }
            if (_decreaseButton != null)
            {
                _decreaseButton.GetComponent<Widget>().Status = Status;
            }
            if (_increaseButton != null)
            {
                _increaseButton.GetComponent<Widget>().Status = Status;
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="inSecondsPast"></param>
        protected override void OnUpdate(float inSecondsPast)
        {
            if (_cursor == null && _cursorSkin != null)
            {
                AddScrollCursor();
            }
            if (_increaseButton == null && _increaseButtonSkin != null)
            {
                AddScrollIncreaseButton();
            }
            if (_decreaseButton == null && _decreaseButtonSkin != null)
            {
                AddScrollDecreaseButton();
            }

            if ((_dirtyFlags & DirtyFlags.Custom1) != DirtyFlags.None && _cursor != null)
            {
                _cursor.GetComponent<SkinnedWidget>().Skin = _cursorSkin;
            }
            if ((_dirtyFlags & DirtyFlags.Custom2) != DirtyFlags.None && _decreaseButton != null)
            {
                _decreaseButton.GetComponent<SkinnedWidget>().Skin = _decreaseButtonSkin;
            }
            if ((_dirtyFlags & DirtyFlags.Custom3) != DirtyFlags.None && _increaseButton != null)
            {
                _increaseButton.GetComponent<SkinnedWidget>().Skin = _increaseButtonSkin;
            }
            if ((_dirtyFlags & DirtyFlags.Value) != DirtyFlags.None)
            {
                UpdateCursor();
            }

            base.OnUpdate(inSecondsPast);
        }

        private void AddScrollCursor()
        {
            _cursor = new GameObject("cursor", this.GameObj);

            Transform t = _cursor.AddComponent<Transform>();
            t.RelativePos = new Vector3(Rect.W / 2, Rect.H / 2, 0);
            t.RelativeAngle = 0;

            ScrollCursor sc = new ScrollCursor();
            sc.VisibilityGroup = this.VisibilityGroup;
            sc.Skin = CursorSkin;
            sc.Rect = Rect.AlignCenter(0, 0, CursorSize.X, CursorSize.Y);

            _cursor.AddComponent<ScrollCursor>(sc);
            Scene.Current.AddObject(_cursor);
        }

        private void AddScrollDecreaseButton()
        {
            _decreaseButton = new GameObject("decreaseButton", this.GameObj);

            Transform t = _decreaseButton.AddComponent<Transform>();
            t.RelativePos = new Vector3(Rect.W / 2, ButtonsSize.Y / 2, 0);
            t.RelativeAngle = 0;

            ScrollDecreaseButton sdb = new ScrollDecreaseButton();
            sdb.VisibilityGroup = this.VisibilityGroup;
            sdb.Skin = DecreaseButtonSkin;
            sdb.Rect = Rect.AlignCenter(0, 0, ButtonsSize.X, ButtonsSize.Y);
            sdb.LeftClickArgument = _scrollSpeed;

            _decreaseButton.AddComponent<ScrollDecreaseButton>(sdb);
            Scene.Current.AddObject(_decreaseButton);
        }

        private void AddScrollIncreaseButton()
        {
            _increaseButton = new GameObject("increaseButton", this.GameObj);

            Transform t = _increaseButton.AddComponent<Transform>();
            t.RelativePos = new Vector3(Rect.W / 2, Rect.H - ButtonsSize.Y / 2, 0);
            t.RelativeAngle = 0;

            ScrollIncreaseButton sib = new ScrollIncreaseButton();
            sib.VisibilityGroup = this.VisibilityGroup;
            sib.Skin = IncreaseButtonSkin;
            sib.Rect = Rect.AlignCenter(0, 0, ButtonsSize.X, ButtonsSize.Y);
            sib.LeftClickArgument = _scrollSpeed;

            _increaseButton.AddComponent<ScrollIncreaseButton>(sib);
            Scene.Current.AddObject(_increaseButton);
        }

        private void UpdateCursor()
        {
            if (_cursor != null)
            {
                _value = Math.Min(Value, _max);
                _value = Math.Max(Value, _min);

                float length = Rect.H - (ButtonsSize.Y * 2) - (CursorSize.Y);
                Vector3 direction = _increaseButton.Transform.Pos - _decreaseButton.Transform.Pos;

                Vector3 origin = _decreaseButton.Transform.Pos + (direction / 2) - (direction.Normalized * length / 2);

                _cursor.Transform.Pos = origin + (direction.Normalized * (Value - Minimum) * length / (Maximum - Minimum));
            }
        }
    }
}