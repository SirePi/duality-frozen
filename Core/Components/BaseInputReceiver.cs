// This code is provided under the MIT license. Originally by Alessandro Pilati.

using System;
using Duality;

namespace SnowyPeak.Duality.Plugin.Frozen.Core.Components
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
        /// <summary>
        /// Virtual KeyDown
        /// </summary>
        /// <param name="e"></param>
        /// <param name="k"></param>
        public virtual void KeyDown(OpenTK.Input.KeyboardKeyEventArgs e, InputController.ModifierKeys k)
        {
        }

        /// <summary>
        /// Virtual KeyUp
        /// </summary>
        /// <param name="e"></param>
        /// <param name="k"></param>
        public virtual void KeyUp(OpenTK.Input.KeyboardKeyEventArgs e, InputController.ModifierKeys k)
        {
        }

        /// <summary>
        /// Virtual MouseDown
        /// </summary>
        /// <param name="e"></param>
        public virtual void MouseDown(OpenTK.Input.MouseButtonEventArgs e)
        {
        }

        /// <summary>
        /// Virtual MouseEnter
        /// </summary>
        public virtual void MouseEnter()
        {
        }

        /// <summary>
        /// Virtual MouseLeave
        /// </summary>
        public virtual void MouseLeave()
        {
        }

        /// <summary>
        /// Virtual MouseUp
        /// </summary>
        /// <param name="e"></param>
        public virtual void MouseUp(OpenTK.Input.MouseButtonEventArgs e)
        {
        }

        /// <summary>
        /// Virtual MouseWheel
        /// </summary>
        /// <param name="e"></param>
        public virtual void MouseWheel(OpenTK.Input.MouseWheelEventArgs e)
        {
        }
    }
}