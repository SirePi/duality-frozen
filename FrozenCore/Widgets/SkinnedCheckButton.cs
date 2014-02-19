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
using Duality.ColorFormat;

namespace FrozenCore.Widgets
{
    [Serializable]
    public class SkinnedCheckButton : SkinnedWidget<BaseSkin>
    {
        private ContentRef<Script> _onChecked;
        private ContentRef<Script> _onUnchecked;
        private FormattedText _text;
        private ColorRgba _textColor;
        private bool _isChecked;

        private object _checkedArgument;
        private object _uncheckedArgument;

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

        public bool IsChecked
        {
            get { return _isChecked; }
            set
            {
                _isChecked = value;
                OnCheckUncheck();
            }
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

        [EditorHintFlags(MemberFlags.Invisible)]
        public object CheckedArgument
        {
            private get { return _checkedArgument; }
            set { _checkedArgument = value; }
        }

        [EditorHintFlags(MemberFlags.Invisible)]
        public object UncheckedArgument
        {
            private get { return _uncheckedArgument; }
            set { _uncheckedArgument = value; }
        }

        public SkinnedCheckButton()
        {
            ActiveArea = Widgets.ActiveArea.LeftBorder;

            _text = new FormattedText();
            _textColor = Colors.White;
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

        internal override void MouseLeave()
        {
            _isMouseOver = false;

            if (_widgetEnabled)
            {
                if (IsChecked)
                {
                    SetTextureTopLeft(Skin.Res.Origin.Active);
                }
                else
                {
                    SetTextureTopLeft(Skin.Res.Origin.Normal);
                }
            }
        }

        protected override void OnInit(Component.InitContext context)
        {
            if(Skin.Res != null)
            {
                SetTextureTopLeft(IsChecked ? Skin.Res.Origin.Active : Skin.Res.Origin.Normal);
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
