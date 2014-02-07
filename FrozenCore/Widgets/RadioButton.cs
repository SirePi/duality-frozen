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

namespace FrozenCore.Widgets
{
    [Serializable]
    public class RadioButton : Widget
    {
        private ContentRef<BaseSkin> _skin;
        private ContentRef<Script> _onChecked;
        private ContentRef<Script> _onUnchecked;
        private FormattedText _text;
        private bool _isChecked;
        private string _radioGroup;

        private object _checkedArgument;
        private object _uncheckedArgument;

        public ContentRef<BaseSkin> Skin
        {
            get { return _skin; }
            set
            {
                _skin = value;
                BaseSkinRes = value.Res;
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

        public bool IsChecked
        {
            get { return _isChecked; }
            set { _isChecked = value; }
        }

        public string RadioGroup
        {
            get { return _radioGroup; }
            set { _radioGroup = value; }
        }

        public FormattedText Text
        {
            get { return _text; }
            set { _text = value; }
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

        public RadioButton()
        {
            _text = new FormattedText();
        }

        public override void MouseUp(OpenTK.Input.MouseButtonEventArgs e)
        {
            if (e.Button == OpenTK.Input.MouseButton.Left && !IsChecked)
            {
                IsChecked = true;

                if (!String.IsNullOrWhiteSpace(RadioGroup))
                {
                    foreach (RadioButton button in Scene.Current.FindComponents<RadioButton>().Where(rb => rb.RadioGroup == this.RadioGroup && rb != this))
                    {
                        button.Uncheck();
                    }
                }

                if (OnChecked.Res != null)
                {
                    OnChecked.Res.Execute(this.GameObj, CheckedArgument);
                }
            }
        }

        protected override void Draw(IDrawDevice inDevice, Canvas inCanvas)
        {
            if (Text != null)
            {
                Vector2 textCenter = Text.Size / 2;

                Vector3 buttonCenter = (_points[5].WorldCoords + _points[10].WorldCoords) / 2;
                //Vector3 buttonCenter = (_points[0].WorldCoords + _points[15].WorldCoords) / 2;

                inCanvas.CurrentState.TransformHandle = textCenter;
                inCanvas.CurrentState.TransformAngle = GameObj.Transform.Angle;

                inCanvas.DrawText(Text, buttonCenter.X + textCenter.X, buttonCenter.Y + textCenter.Y, buttonCenter.Z - DELTA_Z, null, Alignment.Center);
            }
        }

        public override void MouseEnter()
        {
            base.MouseEnter();

            if (_widgetEnabled)
            {
                SetTextureTopLeft(Skin.Res.Origin.Hover);
            }
        }

        public override void MouseLeave()
        {
            base.MouseLeave();

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

        public override Polygon GetActiveAreaOnScreen(Duality.Components.Camera inCamera)
        {
            _activeAreaOnScreen[0] = inCamera.GetScreenCoord(_points[0].WorldCoords).Xy;
            _activeAreaOnScreen[1] = inCamera.GetScreenCoord(_points[1].WorldCoords).Xy;
            _activeAreaOnScreen[2] = inCamera.GetScreenCoord(_points[13].WorldCoords).Xy;
            _activeAreaOnScreen[3] = inCamera.GetScreenCoord(_points[12].WorldCoords).Xy;

            return _activeAreaOnScreen;
        }

        protected override void Initialize(Component.InitContext context)
        {
            BaseSkinRes = Skin.Res;
        }

        private void Uncheck()
        {
            if (IsChecked)
            {
                IsChecked = false;
                if (OnUnchecked.Res != null)
                {
                    OnUnchecked.Res.Execute(this.GameObj, UncheckedArgument);
                }
                MouseLeave();
            }
        }
    }
}
