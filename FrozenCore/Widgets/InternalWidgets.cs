// This code is provided under the MIT license. Originally by Alessandro Pilati.

using Duality;

namespace FrozenCore.Widgets
{
    internal class CloseButton : SkinnedButton
    {
        public CloseButton()
        {
            OnLeftClick = InternalScripts.GetScript<InternalScripts.CloseButtonLeftMouseDown>();
        }
    }

    internal class MaximizeButton : SkinnedButton
    {
        public MaximizeButton()
        {
            OnLeftClick = InternalScripts.GetScript<InternalScripts.MaximizeButtonLeftMouseDown>();
        }
    }

    internal class MinimizeButton : SkinnedButton
    {
        public MinimizeButton()
        {
            OnLeftClick = InternalScripts.GetScript<InternalScripts.MinimizeButtonLeftMouseDown>();
        }
    }

    internal class RestoreButton : SkinnedButton
    {
        public RestoreButton()
        {
            OnLeftClick = InternalScripts.GetScript<InternalScripts.RestoreButtonLeftMouseDown>();
        }
    }

    internal class ScrollCursor : SkinnedButton
    {
        private float _currentDelta;
        private SkinnedScrollBar _parent;

        public override void MouseDown(OpenTK.Input.MouseButtonEventArgs e)
        {
            if (Status != WidgetStatus.Disabled)
            {
                if (e.Button == OpenTK.Input.MouseButton.Left)
                {
                    Status = WidgetStatus.Active;

                    _leftButtonDown = true;
                    _currentDelta = 0;
                }
            }
        }

        public override void MouseMove(OpenTK.Input.MouseMoveEventArgs e)
        {
            base.MouseMove(e);

            if (_leftButtonDown)
            {
                float angle = -this.GameObj.Transform.Angle;

                _currentDelta += (e.YDelta * MathF.Cos(angle)) + (e.XDelta * MathF.Sin(angle));

                if (_parent == null)
                {
                    _parent = this.GameObj.Parent.GetComponent<SkinnedScrollBar>();
                }

                int valueChange = (int)(_currentDelta / _parent.ValueDelta);
                if (valueChange != 0)
                {
                    _parent.Value += valueChange;
                    _currentDelta -= (valueChange * _parent.ValueDelta);
                }
            }
        }
    }

    internal class ScrollDecreaseButton : SkinnedButton
    {
        public ScrollDecreaseButton()
        {
            OnLeftClick = InternalScripts.GetScript<InternalScripts.ScrollDecreaseButtonLeftMouseDown>();
            RepeatLeftClickEvery = 0.1f;
            LeftClickArgument = 1;
        }
    }

    internal class ScrollIncreaseButton : SkinnedButton
    {
        public ScrollIncreaseButton()
        {
            OnLeftClick = InternalScripts.GetScript<InternalScripts.ScrollIncreaseButtonLeftMouseDown>();
            RepeatLeftClickEvery = 0.1f;
            LeftClickArgument = 1;
        }
    }

    /*
    internal class MultiLineScrollbar : SkinnedScrollBar
    {
        public MultiLineScrollbar()
        {
            OnValueChanged = InternalScripts.GetScript<InternalScripts.MultiLineScrollbarValueChanged>();
        }
    }*/
}