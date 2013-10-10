// This code is provided under the MIT license. Originally by Alessandro Pilati.

using System;
using Duality;

namespace FrozenCore.Components
{
    /// <summary>
    /// The BaseInputReceiver is an abstract Component that needs to be derived and its methods overridded in order
    /// to implement the necessary logic by your components.
    /// The type of methods called depend on the associated InputController and its configuration.
    /// </summary>
    /// <seealso cref="InputController"/>
    [Serializable]
    public abstract class BaseInputReceiver : Component
    {
        public virtual void KeyDown(OpenTK.Input.KeyboardKeyEventArgs e, InputController.ModifierKeys k)
        {
        }

        public virtual void KeyUp(OpenTK.Input.KeyboardKeyEventArgs e, InputController.ModifierKeys k)
        {
        }

        public virtual void MouseDown(OpenTK.Input.MouseButtonEventArgs e)
        {
        }

        public virtual void MouseEnter()
        {
        }

        public virtual void MouseLeave()
        {
        }

        public virtual void MouseUp(OpenTK.Input.MouseButtonEventArgs e)
        {
        }

        public virtual void MouseWheel(OpenTK.Input.MouseWheelEventArgs e)
        {
        }
    }
}