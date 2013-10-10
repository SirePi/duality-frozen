// This code is provided under the MIT license. Originally by Alessandro Pilati.

using System;
using System.Collections.Generic;
using Duality;
using Duality.Components.Renderers;

namespace FrozenCore.Components
{
    [Serializable]
    [RequiredComponent(typeof(TextRenderer))]
    public class InputDebugger : Component, ICmpUpdatable
    {
        [NonSerialized]
        private List<string> _debugData;

        [NonSerialized]
        private TextRenderer _txt;

        /// <summary>
        /// [GET/SET] The InputController to monitor
        /// </summary>
        public InputController ControllerToDebug { get; set; }

        void ICmpUpdatable.OnUpdate()
        {
            if (_txt == null)
            {
                _txt = GameObj.GetComponent<TextRenderer>();
                _debugData = new List<string>();
            }

            if (ControllerToDebug != null)
            {
                _debugData.Clear();
                _debugData.Add(String.Format("Receiver:{0}", ControllerToDebug.Receiver == null ? String.Empty : ControllerToDebug.Receiver.GameObj.Name));
                _debugData.Add(String.Format("HoveredElement:{0}", ControllerToDebug.HoveredElement == null ? String.Empty : ControllerToDebug.HoveredElement.GameObj.Name));
                _debugData.Add(String.Format("FocusedElement:{0}", ControllerToDebug.FocusedElement == null ? String.Empty : ControllerToDebug.FocusedElement.GameObj.Name));

                _txt.Text.SourceText = String.Join("; ", _debugData);
            }
        }
    }
}