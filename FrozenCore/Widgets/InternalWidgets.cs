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
        private bool _isDragged;

        internal override void MouseDown(OpenTK.Input.MouseButtonEventArgs e)
        {
            _isDragged = e.Button == OpenTK.Input.MouseButton.Left;
        }

        internal override void MouseUp(OpenTK.Input.MouseButtonEventArgs e)
        {
            _isDragged = false;
        }

        internal override void MouseMove(OpenTK.Input.MouseMoveEventArgs e)
        {
            base.MouseMove(e);

            if (_isDragged)
            {
                this.GameObj.Transform.Pos += (new Vector3(e.XDelta, e.YDelta, 0));
            }
        }
    }
}
