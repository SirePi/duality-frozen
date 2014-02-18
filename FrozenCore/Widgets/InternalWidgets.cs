// This code is provided under the MIT license. Originally by Alessandro Pilati.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Duality.Resources;
using Duality;
using Duality.Components;
using OpenTK;

namespace FrozenCore.Widgets
{
    internal class CloseButton : SkinnedButton
    {
        public CloseButton()
        {
            OnLeftClick = InternalScripts.GetScript<InternalScripts.CloseButtonLeftMouseDown>();
        }
    }

    internal class MinimizeButton: SkinnedButton
    {
        public MinimizeButton()
        {
            OnLeftClick = InternalScripts.GetScript<InternalScripts.MinimizeButtonLeftMouseDown>();
        }
    }

    internal class MaximizeButton : SkinnedButton
    {
        public MaximizeButton()
        {
            OnLeftClick = InternalScripts.GetScript<InternalScripts.MaximizeButtonLeftMouseDown>();
        }
    }

    internal class RestoreButton : SkinnedButton
    {
        public RestoreButton()
        {
            OnLeftClick = InternalScripts.GetScript<InternalScripts.RestoreButtonLeftMouseDown>();
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

    internal class ScrollDownButton : SkinnedButton
    {
        public ScrollDownButton()
        {
            OnLeftClick = InternalScripts.GetScript<InternalScripts.ScrollDownButtonLeftMouseDown>();
            RepeatLeftClickEvery = 0.1f;
            LeftClickArgument = 1;
        }
    }

    internal class ScrollCursor : SkinnedButton
    {
        private SkinnedScrollBar _parent;
        private Vector2 _currentDelta;

        internal override void MouseDown(OpenTK.Input.MouseButtonEventArgs e)
        {
            if (e.Button == OpenTK.Input.MouseButton.Left)
            {
                SetTextureTopLeft(Skin.Res.Origin.Active);

                _leftButtonDown = true;
                _currentDelta = Vector2.Zero;
            }
        }

        internal override void MouseMove(OpenTK.Input.MouseMoveEventArgs e)
        {
            base.MouseMove(e);

            if (_leftButtonDown)
            {
                float angle = MathF.DegToRad(this.GameObj.Transform.Angle);

                _currentDelta.X += (e.XDelta * MathF.Sin(angle));
                _currentDelta.Y += (e.YDelta * MathF.Cos(angle));

                if (_parent == null)
                {
                    _parent = this.GameObj.Parent.GetComponent<SkinnedScrollBar>();
                }

                int valueChange = (int)(_currentDelta.Y / _parent.GetValueDelta());
                if (valueChange != 0)
                {
                    _parent.Value += valueChange;
                    _currentDelta = Vector2.Zero;
                }
            }
        }
    }
}
