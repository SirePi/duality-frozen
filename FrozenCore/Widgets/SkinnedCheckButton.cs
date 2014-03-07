// This code is provided under the MIT license. Originally by Alessandro Pilati.

using System;
using Duality;
using Duality.Drawing;
using OpenTK;

namespace FrozenCore.Widgets
{
    [Serializable]
    public class SkinnedCheckButton : SkinnedWidget
    {
        private object _checkedArgument;
        private bool _isChecked;
        private ContentRef<Script> _onChecked;
        private ContentRef<Script> _onUnchecked;
        private FormattedText _text;
        private ColorRgba _textColor;
        private object _uncheckedArgument;

        public object CheckedArgument
        {
            get { return _checkedArgument; }
            set { _checkedArgument = value; }
        }

        public bool IsChecked
        {
            get { return _isChecked; }
            set
            {
                _isChecked = value;
                OnCheckUncheck();
            }
        }

        public ContentRef<Script> OnChecked
        {
            get { return _onChecked; }
            set { _onChecked = value; }
        }

        public ContentRef<Script> OnUnchecked
        {
            get { return _onUnchecked; }
            set { _onUnchecked = value; }
        }

        public FormattedText Text
        {
            get { return _text; }
            set { _text = value; }
        }

        public ColorRgba TextColor
        {
            get { return _textColor; }
            set { _textColor = value; }
        }

        public object UncheckedArgument
        {
            get { return _uncheckedArgument; }
            set { _uncheckedArgument = value; }
        }

        public SkinnedCheckButton()
        {
            ActiveArea = Widgets.ActiveArea.LeftBorder;

            _text = new FormattedText();
            _textColor = Colors.White;
        }

        internal override void MouseLeave()
        {
            _isMouseOver = false;

            if (Status != WidgetStatus.Disabled)
            {
                Status = IsChecked ? WidgetStatus.Active : WidgetStatus.Normal;
            }
        }

        internal override void MouseUp(OpenTK.Input.MouseButtonEventArgs e)
        {
            if (e.Button == OpenTK.Input.MouseButton.Left && _isMouseOver)
            {
                IsChecked = !IsChecked;

                if (IsChecked && OnChecked.Res != null)
                {
                    OnChecked.Res.Execute(this.GameObj, CheckedArgument);
                }
                if (!IsChecked && OnUnchecked.Res != null)
                {
                    OnUnchecked.Res.Execute(this.GameObj, UncheckedArgument);
                }
            }
        }

        protected override void DrawCanvas(IDrawDevice inDevice, Canvas inCanvas)
        {
            if (Text != null)
            {
                Vector3 buttonCenter = (_points[5].WorldCoords + _points[10].WorldCoords) / 2;

                inCanvas.PushState();
                inCanvas.State.ColorTint = _textColor;
                inCanvas.State.TransformAngle = GameObj.Transform.Angle;
                inCanvas.DrawText(Text, buttonCenter.X, buttonCenter.Y, buttonCenter.Z + DELTA_Z, null, Alignment.Center);
                inCanvas.PopState();
            }
        }

        protected override void OnInit(Component.InitContext context)
        {
            base.OnInit(context);

            if (Status != WidgetStatus.Disabled)
            {
                Status = IsChecked ? WidgetStatus.Active : WidgetStatus.Normal;
            }
        }

        private void OnCheckUncheck()
        {
            if (IsChecked)
            {
                if (OnChecked.Res != null)
                {
                    OnChecked.Res.Execute(this.GameObj, UncheckedArgument);
                }
            }
            else
            {
                if (OnUnchecked.Res != null)
                {
                    OnUnchecked.Res.Execute(this.GameObj, UncheckedArgument);
                }
            }
            MouseLeave();
        }
    }
}