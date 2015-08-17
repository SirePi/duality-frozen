// This code is provided under the MIT license. Originally by Alessandro Pilati.

using Duality;

namespace SnowyPeak.Duality.Plugin.Frozen.UI.Widgets
{
    internal class CloseButton : Button
    {
        public CloseButton()
        {
            OnLeftClick = InternalScripts.GetScript<InternalScripts.CloseButtonLeftMouseDown>();
        }
    }

    internal class MaximizeButton : Button
    {
        public MaximizeButton()
        {
            OnLeftClick = InternalScripts.GetScript<InternalScripts.MaximizeButtonLeftMouseDown>();
        }
    }

    internal class MinimizeButton : Button
    {
        public MinimizeButton()
        {
            OnLeftClick = InternalScripts.GetScript<InternalScripts.MinimizeButtonLeftMouseDown>();
        }
    }

    internal class RestoreButton : Button
    {
        public RestoreButton()
        {
            OnLeftClick = InternalScripts.GetScript<InternalScripts.RestoreButtonLeftMouseDown>();
        }
    }

    internal class ScrollCursor : Button
    {
        private float _currentDelta;
        private ScrollBar _parent;

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
            if (_leftButtonDown)
            {
                float angle = -this.GameObj.Transform.Angle;

                _currentDelta += (e.YDelta * MathF.Cos(angle)) + (e.XDelta * MathF.Sin(angle));

                if (_parent == null)
                {
                    _parent = this.GameObj.Parent.GetComponent<ScrollBar>();
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

    internal class ScrollDecreaseButton : Button
    {
        public ScrollDecreaseButton()
        {
            OnLeftClick = InternalScripts.GetScript<InternalScripts.ScrollDecreaseButtonLeftMouseDown>();
            RepeatLeftClickEvery = 0.1f;
            LeftClickArgument = 1;
        }
    }

    internal class ScrollIncreaseButton : Button
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