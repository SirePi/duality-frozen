// This code is provided under the MIT license. Originally by Alessandro Pilati.

using Duality.Editor;
using Duality.Input;
using Duality.Resources;
using SnowyPeak.Duality.Plugin.Frozen.UI.Properties;
using System;
using System.Linq;

namespace SnowyPeak.Duality.Plugin.Frozen.UI.Widgets
{
    /// <summary>
    /// A RadioButton Widget
    /// </summary>

    [EditorHintImage(ResNames.ImageRadioButton)]
    [EditorHintCategory(ResNames.CategoryWidgets)]
    public class RadioButton : CheckButton
    {
        private string _radioGroup;

        /// <summary>
        /// The RadioGroup this button is part of
        /// </summary>
        public string RadioGroup
        {
            get { return _radioGroup; }
            set { _radioGroup = value; }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="e"></param>
        public override void MouseUp(MouseButtonEventArgs e)
        {
            if (Status != WidgetStatus.Disabled)
            {
                if (e.Button == MouseButton.Left && _isMouseOver && !IsChecked)
                {
                    IsChecked = true;
                }
            }
        }

        protected override void OnUpdate(float inSecondsPast)
        {
            if ((_dirtyFlags & DirtyFlags.Value) != DirtyFlags.None)
            {
                if (IsChecked && !String.IsNullOrWhiteSpace(RadioGroup))
                {
					foreach (RadioButton button in this.GameObj.ParentScene.FindComponents<RadioButton>().Where(rb => rb.RadioGroup == this.RadioGroup && rb != this))
                    {
                        button.IsChecked = false;
                    }
                }
            }

            base.OnUpdate(inSecondsPast);
        }
    }
}