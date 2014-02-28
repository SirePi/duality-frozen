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

        internal override void MouseDown(OpenTK.Input.MouseButtonEventArgs e)
        {
            if (e.Button == OpenTK.Input.MouseButton.Left)
            {
                Status = WidgetStatus.Active;

                _leftButtonDown = true;
                _currentDelta = 0;
            }
        }

        internal override void MouseMove(OpenTK.Input.MouseMoveEventArgs e)
        {
            base.MouseMove(e);

            if (_leftButtonDown)
            {
                float angle = MathF.DegToRad(this.GameObj.Transform.Angle);

                _currentDelta += (e.XDelta * MathF.Sin(angle));
                _currentDelta += (e.YDelta * MathF.Cos(angle));

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

    internal class ScrollDownButton : SkinnedButton
    {
        public ScrollDownButton()
        {
            OnLeftClick = InternalScripts.GetScript<InternalScripts.ScrollDownButtonLeftMouseDown>();
            RepeatLeftClickEvery = 0.1f;
            LeftClickArgument = 1;
        }
    }

    internal class ScrollUpButton : SkinnedButton
    {
        public ScrollUpButton()
        {
            OnLeftClick = InternalScripts.GetScript<InternalScripts.ScrollUpButtonLeftMouseDown>();
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