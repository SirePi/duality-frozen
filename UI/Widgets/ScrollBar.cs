// This code is provided under the MIT license. Originally by Alessandro Pilati.

using System;
using Duality;
using Duality.Components;
using Duality.Editor;
using Duality.Resources;

using SnowyPeak.Duality.Plugin.Frozen.UI.Properties;
using SnowyPeak.Duality.Plugin.Frozen.UI.Resources;

namespace SnowyPeak.Duality.Plugin.Frozen.UI.Widgets
{
    /// <summary>
    /// A Scrollbar Widget
    /// </summary>
    
    [EditorHintImage(ResNames.ImageScrollBar)]
    [EditorHintCategory(ResNames.CategoryWidgets)]
    public class ScrollBar : Widget
    {
        #region NonSerialized fields

        [DontSerialize]
        private GameObject _cursor;

        [DontSerialize]
        private GameObject _decreaseButton;

        [DontSerialize]
        private GameObject _increaseButton;

        #endregion NonSerialized fields

        private ContentRef<ScrollBarAppearance> _scrollAppearance;
        private int _max;
        private int _min;
        private ContentRef<Script> _onValueChanged;
        private int _scrollSpeed;
        private int _value;
        private object _valueChangedArgument;

        /// <summary>
        /// Constructor
        /// </summary>
        public ScrollBar()
        {
            ActiveArea = ActiveArea.None;

            _min = 0;
            _max = 10;
            _value = _min;
            _scrollSpeed = 1;
        }

        /// <summary>
        /// [GET / SET] the Skin used for the Scrollbar Cursor
        /// </summary>
        public ContentRef<ScrollBarAppearance> Appearance
        {
            get { return _scrollAppearance; }
            set
            {
                _scrollAppearance = value;
                _dirtyFlags |= DirtyFlags.Appearance;
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
                float cursorY = _scrollAppearance.Res.CursorSize.Y;

                float length = Rect.H - (cursorY * 2) - (cursorY);
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
            if (_cursor == null && !_scrollAppearance.Res.Cursor.IsExplicitNull)
            {
                AddScrollCursor();
                _dirtyFlags |= DirtyFlags.Value;
            }
            if (_increaseButton == null && !_scrollAppearance.Res.Increase.IsExplicitNull)
            {
                AddScrollIncreaseButton();
            }
            if (_decreaseButton == null && !_scrollAppearance.Res.Decrease.IsExplicitNull)
            {
                AddScrollDecreaseButton();
            }

            if ((_dirtyFlags & DirtyFlags.Appearance) != DirtyFlags.None)
            {
                if (_cursor != null)
                {
                    _cursor.GetComponent<ScrollCursor>().Appearance = AppearanceManager.RequestAppearanceContentRef(_scrollAppearance.Res.Cursor);
                }
                if(_decreaseButton != null)
                {
                    _decreaseButton.GetComponent<ScrollDecreaseButton>().Appearance = AppearanceManager.RequestAppearanceContentRef(_scrollAppearance.Res.Decrease);
                }
                if(_increaseButton != null)
                {
                    _increaseButton.GetComponent<ScrollIncreaseButton>().Appearance = AppearanceManager.RequestAppearanceContentRef(_scrollAppearance.Res.Increase);
                }
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
            sc.Appearance = AppearanceManager.RequestAppearanceContentRef(_scrollAppearance.Res.Cursor);
            sc.Rect = Rect.Align(Alignment.Center, 0, 0, _scrollAppearance.Res.CursorSize.X, _scrollAppearance.Res.CursorSize.Y);

            _cursor.AddComponent<ScrollCursor>(sc);
            Scene.Current.AddObject(_cursor);
        }

        private void AddScrollDecreaseButton()
        {
            _decreaseButton = new GameObject("decreaseButton", this.GameObj);

            Transform t = _decreaseButton.AddComponent<Transform>();
            t.RelativePos = new Vector3(Rect.W / 2, _scrollAppearance.Res.ButtonSize.Y / 2, 0);
            t.RelativeAngle = 0;

            ScrollDecreaseButton sdb = new ScrollDecreaseButton();
            sdb.VisibilityGroup = this.VisibilityGroup;
            sdb.Appearance = AppearanceManager.RequestAppearanceContentRef(_scrollAppearance.Res.Decrease);
            sdb.Rect = Rect.Align(Alignment.Center, 0, 0, _scrollAppearance.Res.ButtonSize.X, _scrollAppearance.Res.ButtonSize.Y);
            sdb.LeftClickArgument = _scrollSpeed;

            _decreaseButton.AddComponent<ScrollDecreaseButton>(sdb);
            Scene.Current.AddObject(_decreaseButton);
        }

        private void AddScrollIncreaseButton()
        {
            _increaseButton = new GameObject("increaseButton", this.GameObj);

            Transform t = _increaseButton.AddComponent<Transform>();
            t.RelativePos = new Vector3(Rect.W / 2, Rect.H - _scrollAppearance.Res.ButtonSize.Y / 2, 0);
            t.RelativeAngle = 0;

            ScrollIncreaseButton sib = new ScrollIncreaseButton();
            sib.VisibilityGroup = this.VisibilityGroup;
            sib.Appearance = AppearanceManager.RequestAppearanceContentRef(_scrollAppearance.Res.Increase);
            sib.Rect = Rect.Align(Alignment.Center, 0, 0, _scrollAppearance.Res.ButtonSize.X, _scrollAppearance.Res.ButtonSize.Y);
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

                float length = Rect.H - (_scrollAppearance.Res.ButtonSize.Y * 2) - (_scrollAppearance.Res.CursorSize.Y);
                Vector3 direction = _increaseButton.Transform.Pos - _decreaseButton.Transform.Pos;

                Vector3 origin = _decreaseButton.Transform.Pos + (direction / 2) - (direction.Normalized * length / 2);

                _cursor.Transform.Pos = origin + (direction.Normalized * (Value - Minimum) * length / (Maximum - Minimum));
            }
        }

        protected override Appearance GetBaseAppearance()
        {
            return _scrollAppearance.Res.Widget.Res;
        }
    }
}