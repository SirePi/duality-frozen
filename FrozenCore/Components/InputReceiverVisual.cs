// This code is provided under the MIT license. Originally by Alessandro Pilati.

using System;
using Duality;
using OpenTK;
using Duality.Components;

namespace FrozenCore.Components
{
    /// <summary>
    /// The InputReceiverVisual is a derived BaseInputReceiver that allows to send events directly to an ICmpRenderer
    /// GameObject. This allows to implement GUI elements such as Buttons, etc or to directly control a GameObject
    /// such as a Player character.
    /// </summary>
    /// <seealso cref="InputController"/>
    /// <seealso cref="BaseInputReceiver"/>
    [Serializable]
    [RequiredComponent(typeof(ICmpRenderer))]
    public abstract class InputReceiverVisual : BaseInputReceiver
    {
        /// <summary>
        /// [GET/SET] If the associated ICmpRenderer can be dragged by holding the Left Mouse Button
        /// </summary>
        public bool Draggable { get; set; }
        /// <summary>
        /// [GET/SET] If the Component is notified of KeyUp and KeyDown events
        /// </summary>
        public bool ReceiveKeys { get; set; }
        /// <summary>
        /// [GET/SET] If the Component is notified of MouseUp and MouseDown events
        /// </summary>
        public bool ReceiveMouseClicks { get; set; }
        /// <summary>
        /// [GET/SET] If the Component is notified of MouseWheel events
        /// </summary>
        public bool ReceiveMouseWheel { get; set; }

        public virtual void Dragged(Vector2 inDelta)
        {
            GameObj.Transform.MoveBy(inDelta);
        }
    }
}